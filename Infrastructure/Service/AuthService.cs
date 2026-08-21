using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Domain.Interfaces;
using AutoMapper;

namespace Infrastructure.Service
{
    public class AuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            ICartRepository cartRepository,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _cartRepository = cartRepository;
            _mapper = mapper; // Додаємо AutoMapper
        }

        /// <summary>
        /// Перевіряє логін та пароль користувача.
        /// </summary>
        public async Task<(bool Success, IEnumerable<string> Roles)> ValidateUserAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return (false, null);

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return (true, roles);
            }

            return (false, null);
        }

        /// <summary>
        /// Генерує JWT токен для авторизованого користувача.
        /// </summary>
        public async Task<string> GenerateJwtTokenAsync(User user, IEnumerable<string> roles, IEnumerable<Claim> additionalClaims = null)
        {
            // Основні claims
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };

            // Додаємо ролі до claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Додаємо додаткові claims (якщо є)
            if (additionalClaims != null)
            {
                claims.AddRange(additionalClaims);
            }

            // Параметри для створення токена
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(10),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateJwtToken(string userId, string userName)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        // Реєстрація нового користувача
        public async Task<(bool Success, string Message)> RegisterUserAsync(
    string username, string password, string firstName, string lastName, DateTime dateOfBirth, string role = "Customer")
        {
            var user = new User
            {
                UserName = username,
                Email = username,
                FirstName = firstName,
                LastName = lastName,
                DateOfBitrh = dateOfBirth
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                // Збираємо опис усіх помилок
                var errors = string.Join("; ", result.Errors.Select(e => $"{e.Code}: {e.Description}"));
                return (false, $"Failed to register user: {errors}");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join("; ", roleResult.Errors.Select(e => $"{e.Code}: {e.Description}"));
                return (false, $"User registered, but failed to assign role: {errors}");
            }

            // Додати створення кошика для користувача
            var cart = new Infrastructure.Models.Cart
            {
                UserId = user.Id // Встановлюємо ID користувача
            };

            try
            {
                // Використання AutoMapper для перетворення в Domain.Models.Cart
                var domainCart = _mapper.Map<Domain.Models.Cart>(cart);
                await _cartRepository.AddCartAsync(domainCart);
            }
            catch (Exception ex)
            {
                return (false, $"User registered, but failed to create cart: {ex.Message}");
            }

            return (true, "User registered successfully");
        }

        public bool IsTokenValid(string jwt)
        {
            try
            {
                // Спроба розбору токена
                var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(jwt);

                // Якщо токен розібрано успішно, він валідний за форматом
                return true;
            }
            catch (Exception ex)
            {
                // Логування помилки (наприклад, токен має неправильний формат)
                Console.WriteLine($"Invalid JWT Token: {ex.Message}");
                return false;
            }
        }

        public string GetUserIdFromToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty", nameof(token));

            try
            {
                var handler = new JwtSecurityTokenHandler();
                if (handler.CanReadToken(token))
                {
                    var jwtToken = handler.ReadJwtToken(token);
                    var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
                    return userIdClaim?.Value;
                }

                return null;
            }
            catch (Exception ex)
            {
                // Логування помилки, якщо потрібно
                Console.WriteLine($"Error reading token: {ex.Message}");
                return null;
            }
        }


    }
}
