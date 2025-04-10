using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BugTracker.Core.Requests;
using BugTracker.Data.Database;
using BugTracker.Data.Model;
using BugTracker.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BugTracker.Core.Services
{
    internal class BugsService : IRequestHandler<GetBugByIdRequest, Bug?>
        , IRequestHandler<GetBugsRequest, BugHeader[]>
        , IRequestHandler<CreateBugRequest, Bug>
        , IRequestHandler<UpdateBugRequest, Bug>
        , IRequestHandler<UpdateBugStatusRequest, Bug>
        , IRequestHandler<DeleteBugRequest>

    {
        private readonly IMapper _mapper;
        private readonly DatabaseContext _db;

        public BugsService(IMapper mapper
        , DatabaseContext databaseContext)
        {
            _mapper = mapper;
            _db = databaseContext;
        }

        /// <summary>
        /// Обработка запроса на получение бага по ID
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Bug?> Handle(GetBugByIdRequest request, CancellationToken cancellationToken)
        {
            return await _db.Bugs.FindAsync(request.Id, cancellationToken);
        }

        /// <summary>
        /// Обработка запроса на получение списка багов по условию поиска
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<BugHeader[]> Handle(GetBugsRequest request, CancellationToken cancellationToken)
        {
            var bugsQuery = _db.Bugs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.NameOrDescriptionContains))
                bugsQuery = bugsQuery.Where(b => b.Name.Contains(request.NameOrDescriptionContains));
            if (!string.IsNullOrWhiteSpace(request.AuthorContains))
                bugsQuery = bugsQuery.Where(b => b.Author != null && b.Author.Contains(request.AuthorContains));
            var headers = await bugsQuery
                .Skip(request.Skip * request.Count).Take(request.Count)
                .ProjectTo<BugHeader>(_mapper.ConfigurationProvider)
                .AsNoTracking()
                .ToArrayAsync(cancellationToken: cancellationToken);
            return headers;
        }

        /// <summary>
        /// Обработка запроса на создание бага
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Bug> Handle(CreateBugRequest request, CancellationToken cancellationToken)
        {
            if (!Validate(request.BugDto, out var message))
                throw new Exception(message);
            var bug = _mapper.Map<Bug>(request.BugDto);
            await _db.Bugs.AddAsync(bug, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return bug;
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
        /// Обработка запроса на обновление бага
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Bug> Handle(UpdateBugRequest request, CancellationToken cancellationToken)
        {
            if (!Validate(request.BugDto, out var message))
                throw new Exception(message);
            var existingBug = await _db.Bugs.FindAsync(request.Id);
            if (existingBug == null)
                throw new Exception($"Нет бага с id:{request.Id}");
            existingBug.Name = request.BugDto.Name;
            existingBug.Description = request.BugDto.Description;
            await _db.SaveChangesAsync(cancellationToken);
            return existingBug;
        }

        /// <summary>
        /// Обработка запроса на обновление статуса бага
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Bug> Handle(UpdateBugStatusRequest request, CancellationToken cancellationToken)
        {
            if (!BugStatuses.GetAllAvailableStatuses().Contains(request.BugStatusNormalized))
                throw new Exception(
                    $"Статус '{request.BugStatus}' не поддерживается, доступные значения: {string.Join(", ", BugStatuses.GetAllAvailableStatuses())}");
            var existingBug = await _db.Bugs.FindAsync(request.Id);
            if (existingBug == null)
                throw new Exception($"Нет бага с id:{request.Id}");
            existingBug.Status = request.BugStatusNormalized;
            await _db.SaveChangesAsync(cancellationToken);
            return existingBug;
        }

        /// <summary>
        /// Обработка запроса на удаление бага
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Handle(DeleteBugRequest request, CancellationToken cancellationToken)
        {
            var existingBug = await _db.Bugs.FindAsync(request.Id);
            if (existingBug == null)
                throw new Exception($"Нет бага с id:{request.Id}");
            var currentUser = Environment.UserName;
            if (string.Equals(currentUser, existingBug.Author))
                throw new Exception("Нельзя удалить баг, автором которого вы не являетесь");
            _db.Bugs.Remove(existingBug);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
