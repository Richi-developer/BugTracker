using AutoMapper;
using BugTracker.Core.Requests;
using BugTracker.Data.Database;
using BugTracker.Data.Model;
using BugTracker.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace BugTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BugsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Получение бага по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var bug = await _mediator.Send(new GetBugByIdRequest(id));
            return Ok(bug);
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
            var headers = await _mediator.Send(new GetBugsRequest(nameOrDescriptionContains, authorContains, skip, count));
            return Ok(headers);
        }

        /// <summary>
        /// Создание нового бага
        /// </summary>
        /// <param name="bugDto"></param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BugDto bugDto)
        {
            var bug = await _mediator.Send(new CreateBugRequest(bugDto));
            return Ok(bug);
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
            var bug = await _mediator.Send(new UpdateBugRequest(id, bugDto));
            return Ok(bug);
        }

        /// <summary>
        /// Обновление статуса бага
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("{id}/status")]
        public async Task<IActionResult> SetStatus(int id, string status)
        {
            var bug = await _mediator.Send(new UpdateBugStatusRequest(id, status));
            return Ok(bug);
        }

        /// <summary>
        /// Удаление бага
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            
            return Ok();
        }
    }
}
