using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Models;
using Domain.Interfaces;
using AutoMapper;


namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<Infrastructure.Models.User> _userManager;
        private readonly IMapper _mapper;

        public UserRepository(UserManager<Infrastructure.Models.User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = _userManager.Users;
            var userEntities = await Task.Run(() => users.ToList());
            return _mapper.Map<IEnumerable<User>>(userEntities);
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return _mapper.Map<User>(user);
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }

        public async Task<IEnumerable<string>> GetRolesAsync(User user)
        {
            var userEntity = _mapper.Map<Infrastructure.Models.User>(user);
            var roles = await _userManager.GetRolesAsync(userEntity);
            return roles;
        }

        public async Task ChangeUserRoleAsync(User user, string newRole)
        {
            var userEntity = _mapper.Map<Infrastructure.Models.User>(user);
            var currentRoles = await _userManager.GetRolesAsync(userEntity);
            await _userManager.RemoveFromRolesAsync(userEntity, currentRoles);
            await _userManager.AddToRoleAsync(userEntity, newRole);
        }
    }
}
