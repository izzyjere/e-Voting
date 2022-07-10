using AutoMapper;
using ICTAZEVoting.Core.Models;
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