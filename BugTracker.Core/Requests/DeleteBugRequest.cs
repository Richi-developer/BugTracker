using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Data.Model;
using MediatR;

namespace BugTracker.Core.Requests
{
    public class DeleteBugRequest: IRequest
    {
        public int Id { get; }

        public DeleteBugRequest(int id) => Id = id;
    }
}
