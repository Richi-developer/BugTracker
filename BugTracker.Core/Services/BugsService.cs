﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
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
    internal class BugsService : IRequestHandler<GetBugByIdRequest, Result<Bug?>>
        , IRequestHandler<GetBugsRequest, Result<BugHeader[]>>
        , IRequestHandler<CreateBugRequest, Result<Bug>>
        , IRequestHandler<UpdateBugRequest, Result<Bug>>
        , IRequestHandler<UpdateBugStatusRequest, Result<Bug>>
        , IRequestHandler<DeleteBugRequest, Result>

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
        public async Task<Result<Bug?>> Handle(GetBugByIdRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var bug = await _db.Bugs.FindAsync(request.Id, cancellationToken);
                if (bug == null)
                    return Result<Bug?>.NotFound();
                return Result<Bug?>.Success(bug);
            }
            catch (Exception e)
            {
                return Result<Bug?>.Error(e.Message);
            }
        }

        /// <summary>
        /// Обработка запроса на получение списка багов по условию поиска
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<BugHeader[]>> Handle(GetBugsRequest request, CancellationToken cancellationToken)
        {
            try
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
            catch (Exception e)
            {
                return Result<BugHeader[]>.Error(e.Message);
            }
        }

        /// <summary>
        /// Обработка запроса на создание бага
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<Bug>> Handle(CreateBugRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (!Validate(request.BugDto, out var message))
                    throw new Exception(message);
                var bug = _mapper.Map<Bug>(request.BugDto);
                await _db.Bugs.AddAsync(bug, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
                return Result<Bug>.Success(bug);
            }
            catch (Exception e)
            {
                return Result<Bug>.Error(e.Message);
            }
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
        public async Task<Result<Bug>> Handle(UpdateBugRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (!Validate(request.BugDto, out var message))
                    throw new Exception(message);
                var existingBug = await _db.Bugs.FindAsync(request.Id);
                if (existingBug == null)
                    throw new Exception($"Нет бага с id:{request.Id}");
                existingBug.Name = request.BugDto.Name;
                existingBug.Description = request.BugDto.Description;
                await _db.SaveChangesAsync(cancellationToken);
                return Result<Bug>.Success(existingBug);
            }
            catch (Exception e)
            {
                return Result<Bug>.Error(e.Message);
            }
        }

        /// <summary>
        /// Обработка запроса на обновление статуса бага
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<Bug>> Handle(UpdateBugStatusRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (!BugStatuses.GetAllAvailableStatuses().Contains(request.BugStatusNormalized))
                    throw new Exception(
                        $"Статус '{request.BugStatus}' не поддерживается, доступные значения: {string.Join(", ", BugStatuses.GetAllAvailableStatuses())}");
                var existingBug = await _db.Bugs.FindAsync(request.Id);
                if (existingBug == null)
                    throw new Exception($"Нет бага с id:{request.Id}");
                existingBug.Status = request.BugStatusNormalized;
                await _db.SaveChangesAsync(cancellationToken);
                return Result<Bug>.Success(existingBug);
            }
            catch (Exception e)
            {
                return Result<Bug>.Error(e.Message);
            }
        }

        /// <summary>
        /// Обработка запроса на удаление бага
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result> Handle(DeleteBugRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var existingBug = await _db.Bugs.FindAsync(request.Id);
                if (existingBug == null)
                    throw new Exception($"Нет бага с id:{request.Id}");
                var currentUser = Environment.UserName;
                if (string.Equals(currentUser, existingBug.Author))
                    throw new Exception("Нельзя удалить баг, автором которого вы не являетесь");
                _db.Bugs.Remove(existingBug);
                await _db.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Error(e.Message);
            }
        }
    }
}
