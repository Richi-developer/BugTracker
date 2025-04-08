using AutoMapper;
using BugTracker.Database;
using BugTracker.Dto;
using BugTracker.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BugTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        private readonly IMapper _mapper;

        public BugsController(IMapper mapper)
        {
            _mapper = mapper;
        }

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
        /// Получение заголовков багов постранично 
        /// </summary>
        /// <param name="count">количество на странице, по-умолчанию 10</param>
        /// <param name="skip">сколько страниц пропустить, по-умолчанию 0</param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<IActionResult> GetList(int count = 10, int skip = 0) 
            => await Search(count: count, skip: skip);
        
        /// <summary>
        /// Поиск заголовков багов постранично 
        /// </summary>
        /// <param name="nameOrDescriptionContains">Фраза для поиска в имени или описании бага</param>
        /// <param name="authorContains">Фраза для поиска в имени автора бага</param>
        /// <param name="count">количество на странице, по-умолчанию 10</param>
        /// <param name="skip">сколько страниц пропустить, по-умолчанию 0</param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<IActionResult> Search(string? nameOrDescriptionContains = null,
            string? authorContains = null,
            int count = 10, int skip = 0)
        {
            await using var db = new DatabaseContext();
            var bugsQuery = db.Bugs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(nameOrDescriptionContains))
                bugsQuery = bugsQuery.Where(b => b.Name.Contains(nameOrDescriptionContains));
            if (!string.IsNullOrWhiteSpace(authorContains))
                bugsQuery = bugsQuery.Where(b => b.Author != null && b.Author.Contains(authorContains));
            var headers = bugsQuery
                .Skip(skip * count).Take(count)
                .Select(b => new { b.Id, b.Name, b.Author })
                .AsNoTracking()
                .ToArray();
            return Ok(headers);
        }

        /// <summary>
        /// Создание нового бага
        /// </summary>
        /// <param name="bugDto"></param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BugDto bugDto)
        {
            if (!Validate(bugDto, out var message))
                return BadRequest(message);
            var bug = _mapper.Map<Bug>(bugDto);
            await using var db = new DatabaseContext();
            await db.Bugs.AddAsync(bug);
            await db.SaveChangesAsync();
            return Ok(bug);
        }

        private bool Validate(BugDto bug, out string? message)
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
        /// <param name="bugDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BugDto bugDto)
        {
            if(!Validate(bugDto, out var message))
                return BadRequest(message);
            await using var db = new DatabaseContext();
            var existingBug = await db.Bugs.FindAsync(id);
            if (existingBug == null)
                return BadRequest($"Нет бага с id:{id}");
            existingBug.Name = bugDto.Name;
            existingBug.Description = bugDto.Description;
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
            var currentUser = Environment.UserName;
            if(string.Equals(currentUser, existingBug.Author))
                return Forbid("Нельзя удалить баг, автором которого вы не являетесь");
            db.Bugs.Remove(existingBug);
            await db.SaveChangesAsync();
            return Ok();
        }

        

    }
}
