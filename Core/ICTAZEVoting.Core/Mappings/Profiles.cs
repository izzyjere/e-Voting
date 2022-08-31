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
    public class PolliingStationProfile:Profile
    {
        public PolliingStationProfile()
        {
            CreateMap<PollingStationResponse, PollingStation>().ReverseMap();
        }
    }
    public class ConstituencyProfile : Profile
    {
        public ConstituencyProfile()
        {
            CreateMap<ConstituencyResponse, Constituency>().ReverseMap();
        }
    }
    public class VoterProfile : Profile
    {
        public VoterProfile()
        {
            CreateMap<VoterResponse, Voter>().ReverseMap();
        }
    }
    public class ElectionProfile : Profile
    {
        public ElectionProfile()
        {
            CreateMap<ElectionResponse, Election>().ReverseMap()
                 .ForMember(dest => dest.VoterCount, source => source.MapFrom(source => source.Voters.Count));
        }

    }
    public class PoliticalPartyProfile : Profile
    {
        public PoliticalPartyProfile()
        {
            CreateMap<PoliticalPartyResponse, PoliticalParty>().ReverseMap();
        }
    }
    public class ElectionPositionProfile : Profile
    {
        public ElectionPositionProfile()
        {
            CreateMap<ElectionPositionResponse, ElectionPosition>().ReverseMap()
                .ForMember(dest => dest.CandidateCount, source => source.MapFrom(source => source.Candidates.Count)); ;
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
    public class PersonalDetailsProfile : Profile
    {
        public PersonalDetailsProfile()
        {
            CreateMap<PersonalDetailsResponse, PersonalDetails>().ReverseMap();
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
