﻿using System;
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
    public class CreateBugRequest: IRequest<Result<Bug>>
    {
        public BugDto BugDto { get; }

        public CreateBugRequest(BugDto bugDto) => BugDto = bugDto;
    }
}
