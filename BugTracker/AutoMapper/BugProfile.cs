using AutoMapper;
using BugTracker.Dto;
using BugTracker.Model;

namespace BugTracker.AutoMapper
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
