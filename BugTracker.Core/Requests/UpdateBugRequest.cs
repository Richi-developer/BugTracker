using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using BugTracker.Data.Model;
using BugTracker.Dto;
using MediatR;

namespace BugTracker.Core.Requests
{
    public class UpdateBugRequest: IRequest<Result<Bug>>
    {
        public int Id { get; }
        public BugDto BugDto { get; }

        public UpdateBugRequest(int id, BugDto bugDto)
        {
            Id = id;
            BugDto = bugDto;
        }
    }
}
