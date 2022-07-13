global using AutoMapper;

using ICTAZEVoting.Core.Models;
using ICTAZEVoting.Shared.Responses.Identity;

namespace ICTAZEVoting.Core.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, Role>().ReverseMap();
        }
    }
}