using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using super_1.Data;
using super_1.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace super_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SleepController : ControllerBase
    {
        private readonly TestDbContext _context;

        public SleepController(TestDbContext context)
        {
            _context = context;
        }

        // Метод для получения пользователя по токену
        private User GetUserByToken(string token)
        {
            return _context.Users.FirstOrDefault(u => u.AuthToken == token);
        }

        // Начало сна
        [HttpPost("start")]
        public async Task<ActionResult<int>> StartSleep([FromHeader] string token)
        {
            var user = GetUserByToken(token);
            if (user == null)
            {
                return Unauthorized("Неавторизованный доступ.");
            }

            var sleepRecord = new SleepRecord
            {
                UserId = user.Id,
                SleepStart = DateTime.Now,
                SleepEnd = null
            };

            _context.SleepRecords.Add(sleepRecord);
            await _context.SaveChangesAsync();

            return Ok(sleepRecord.Id);
        }

        // Завершение сна
        [HttpPost("end")]
        public async Task<ActionResult> EndSleep([FromHeader] string token, [FromQuery] int id)
        {
            var user = GetUserByToken(token);
            if (user == null)
            {
                return Unauthorized("Неавторизованный доступ.");
            }

            var sleepRecord = await _context.SleepRecords.FindAsync(id);
            if (sleepRecord == null || sleepRecord.UserId != user.Id)
            {
                return NotFound("Запись о сне не найдена.");
            }

            sleepRecord.SleepEnd = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(sleepRecord);
        }
    }
}
