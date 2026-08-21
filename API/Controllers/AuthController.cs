using API.DTOs;
using Infrastructure.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]

    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly UserManager<User> _userManager;

        public AuthController(AuthService authService, UserManager<User> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        /*[HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
        {
            var (success, roles) = await _authService.ValidateUserAsync(loginDto.Username, loginDto.Password);

            if (!success)
                return Unauthorized("Invalid username or password.");

            var user = await _userManager.FindByNameAsync(loginDto.Username);
            var token = await _authService.GenerateJwtToken(user, roles);

            return Ok(new { Token = token });
        }*/

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Перевірка пароля користувача
            if (!await _userManager.CheckPasswordAsync(user, login.Password))
            {
                return Unauthorized("Invalid credentials.");
            }

            // Генерація JWT токена
            var token = _authService.GenerateJwtToken(user.Id, login.Username);

            // Повертаємо токен разом з userId
            return Ok(new
            {
                Token = token,
                UserId = user.Id
            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterUserAsync(
                model.UserName, model.Password, model.FirstName, model.LastName, model.DateOfBirth);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok("Register succesfull");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // JWT Logout можна виконувати без дій на сервері, бо токени зберігаються у клієнта
            return Ok("Logged out successfully.");
        }


        [HttpPost("validate-token")]
        public IActionResult ValidateToken([FromBody] string jwt)
        {
            if (string.IsNullOrWhiteSpace(jwt))
                return BadRequest("Token is missing.");

            var isValid = _authService.IsTokenValid(jwt);
            if (!isValid)
                return BadRequest("Invalid token format.");

            return Ok("Token is valid.");
        }

        [HttpPost("get-user-id")]
        public IActionResult GetUserIdFromToken([FromBody] string jwt)
        {
            if (string.IsNullOrWhiteSpace(jwt))
                return BadRequest("Token is missing.");

            // Використовуємо сервіс для перевірки валідності токена
            var isValid = _authService.IsTokenValid(jwt);
            if (!isValid)
                return BadRequest("Invalid token.");

            // Витягуємо userId
            var userId = _authService.GetUserIdFromToken(jwt);
            if (string.IsNullOrEmpty(userId))
                return BadRequest("User ID not found in the token.");

            return Ok(new { UserId = userId });
        }


        [HttpGet("is-admin/{userId}")]
        public async Task<IActionResult> IsAdmin(string userId)
        {
            // Знаходимо користувача за його Id
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Перевіряємо, чи є у користувача роль "Admin"
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            return Ok(new { IsAdmin = isAdmin });
        }

    }




}
