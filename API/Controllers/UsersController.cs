using Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // Отримати всіх користувачів
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                // Логування помилки
                Console.WriteLine($"Error in {nameof(GetAllUsers)}: {ex.Message}");

                // Повернення детальної помилки
                return StatusCode(500, new
                {
                    Message = "An error occurred while processing your request.",
                    Error = ex.Message,
                    Details = ex.StackTrace
                });
            }
        }

        // Отримати інформацію про користувача за ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _userService.GetAllUsersAsync(); // Замість цього необхідно отримати конкретного користувача за ID
                var userDetails = user.FirstOrDefault(u => u.Id == id);
                if (userDetails == null)
                    return NotFound();

                return Ok(userDetails);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in {nameof(GetUserById)}: {ex.Message}");
                return StatusCode(500, new
                {
                    Message = "An error occurred while processing your request.",
                    Error = ex.Message,
                    Details = ex.StackTrace
                });
            }
        }

        // Видалити користувача
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in {nameof(DeleteUser)}: {ex.Message}");
                return StatusCode(500, new
                {
                    Message = "An error occurred while processing your request.",
                    Error = ex.Message,
                    Details = ex.StackTrace
                });
            }
        }

        // Змінити роль користувача
        [HttpPut("{id}/role")]
        public async Task<IActionResult> ChangeUserRole(string id, [FromBody] string newRole)
        {
            if (string.IsNullOrEmpty(newRole))
                return BadRequest("Role is required.");

            try
            {
                await _userService.ChangeUserRoleAsync(id, newRole);
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in {nameof(ChangeUserRole)}: {ex.Message}");
                return StatusCode(500, new
                {
                    Message = "An error occurred while processing your request.",
                    Error = ex.Message,
                    Details = ex.StackTrace
                });
            }
        }
    }
}
