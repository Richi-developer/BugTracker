

using AutoMapper;
using BugTracker.Data.Model;
using BugTracker.Dto;

namespace BugTracker.Core.AutoMapper
{
    public class BugProfile: Profile
    {
        public BugProfile()
        {
            CreateMap<BugDto, Bug>()
                .ForMember(bug => bug.Author, expression => expression.MapFrom(dto => Environment.UserName));
        }
    }
}
