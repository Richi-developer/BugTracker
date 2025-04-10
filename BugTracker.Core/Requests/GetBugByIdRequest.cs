using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Data.Model;
using MediatR;

namespace BugTracker.Core.Requests
{
    public class GetBugByIdRequest: IRequest<Bug?>
    {
        public int Id { get; }

        public GetBugByIdRequest(int id) => Id = id;
    }
}
