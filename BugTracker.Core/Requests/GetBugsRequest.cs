using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugTracker.Data.Model;
using MediatR;

namespace BugTracker.Core.Requests
{
    public class GetBugsRequest : IRequest<BugHeader[]>
    {
        public string? NameOrDescriptionContains { get; }
        public string? AuthorContains { get; }
        public int Skip { get; }
        public int Count { get; }

        public GetBugsRequest(string? nameOrDescriptionContains = null,
            string? authorContains = null, int skip = 0, int count = 10)
        {
            NameOrDescriptionContains = nameOrDescriptionContains;
            AuthorContains = authorContains;
            Skip = skip;
            Count = count;
        }
    }



}
