using AutoMapper;
using ICTAZEVoting.Core.Models;
using ICTAZEVoting.Shared.Responses.Identity;

namespace ICTAZEVoting.Core.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, User>().ReverseMap();
        }
    }
}