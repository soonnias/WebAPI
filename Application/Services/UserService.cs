using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            /*var userDTOs = new List<UserDTO>();

            foreach (var user in users)
            {
                var roles = await _userRepository.GetRolesAsync(user);

                userDTOs.Add(new UserDTO
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles
                });
            }*/

            return users;
        }

        public async Task DeleteUserAsync(string id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task ChangeUserRoleAsync(string id, string newRole)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user != null)
            {
                await _userRepository.ChangeUserRoleAsync(user, newRole);
            }
        }
    }
}
