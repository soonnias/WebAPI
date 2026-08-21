using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task DeleteUserAsync(string id);
        Task<IEnumerable<string>> GetRolesAsync(User user);
        Task ChangeUserRoleAsync(User user, string newRole);
    }
}
