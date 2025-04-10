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
    public class UpdateBugStatusRequest: IRequest<Result<Bug>>
    {
        public int Id { get; }
        public string BugStatus { get; }
        public string BugStatusNormalized => BugStatus.ToLower();

        public UpdateBugStatusRequest(int id, string bugStatus)
        {
            Id = id;
            BugStatus = bugStatus;
        }
    }
}
