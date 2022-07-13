global using AutoMapper;

global using ICTAZEVoting.Core.Models;
global using ICTAZEVoting.Shared.Responses.Identity;
using ICTAZEVoting.Shared.Models;
using ICTAZEVoting.Shared.Responses.Audit;
using ICTAZEVoting.Shared.Responses.Domain;

namespace ICTAZEVoting.Core.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, Role>().ReverseMap();
        }       

    }
    public class ElectionProfile : Profile
    {
        public ElectionProfile()
        {
            CreateMap<ElectionResponse, Election>().ReverseMap();
        }

    }
    public class ElectionPositionProfile : Profile
    {
        public ElectionPositionProfile()
        {
            CreateMap<ElectionPositionResponse, ElectionPosition>().ReverseMap();
        }

    }
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, User>().ReverseMap();
        }
    }
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
    public class CandidateProfile : Profile
    {
        public CandidateProfile()
        {
            CreateMap<Candidate, CandidateResponse>().ReverseMap();
        }
    }
}
