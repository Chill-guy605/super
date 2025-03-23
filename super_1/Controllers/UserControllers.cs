using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using super_1.Data;
using super_1.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace super_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TestDbContext _context;

        public UserController(TestDbContext context)
        {
            _context = context;
        }

        // Регистрация пользователя
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Login == request.Login))
            {
                return BadRequest("Пользователь с таким логином уже существует.");
            }

            var passwordHash = HashPassword(request.Password);

            var user = new User
            {
                Login = request.Login,
                PasswordHash = passwordHash,
                AuthToken = Guid.NewGuid().ToString()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        // Аутентификация пользователя
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .Where(u => u.Login == request.Login)
                .FirstOrDefaultAsync();

            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized("Неверный логин или пароль.");
            }

            return Ok(new { Token = user.AuthToken });
        }

        // Метод для хэширования пароля
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Метод для проверки пароля
        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            var hash = HashPassword(enteredPassword);
            return hash == storedPasswordHash;
        }
    }

    // Класс для данных регистрации
    public class RegisterRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    // Класс для данных логина
    public class LoginRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
