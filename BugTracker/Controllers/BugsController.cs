using BugTracker.Database;
using BugTracker.Model;
using Microsoft.AspNetCore.Mvc;


namespace BugTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        /// <summary>
        /// Получение бага по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            await using var db = new DatabaseContext();
            return Ok(db.Bugs.FindAsync(id));
        }

        /// <summary>
        /// Создание нового бага
        /// </summary>
        /// <param name="bug"></param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Bug bug)
        {
            if (!Validate(bug, out var message))
                return BadRequest(message);
            await using var db = new DatabaseContext();
            await db.Bugs.AddAsync(bug);
            await db.SaveChangesAsync();
            return Ok(bug);
        }

        private bool Validate(Bug bug, out string? message)
        {
            message = null;
            if (string.IsNullOrEmpty(bug.Name))
            {
                message = "Наименование бага должно быть заполнено";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Обновление бага
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bug"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Bug bug)
        {
            if(!Validate(bug, out var message))
                return BadRequest(message);
            await using var db = new DatabaseContext();
            var existingBug = await db.Bugs.FindAsync(id);
            if (existingBug == null)
                return BadRequest($"Нет бага с id:{id}");
            existingBug.Name = bug.Name;
            existingBug.Author = bug.Author;
            existingBug.Description = bug.Description;
            await db.SaveChangesAsync();
            return Ok(existingBug);
        }

        /// <summary>
        /// Удаление бага
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await using var db = new DatabaseContext();
            var existingBug = await db.Bugs.FindAsync(id);
            if (existingBug == null)
                return BadRequest($"Нет бага с id:{id}");
            db.Bugs.Remove(existingBug);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
