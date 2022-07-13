using ICTAZEVoting.Shared.Models;
using ICTAZEVoting.Shared.Responses.Domain;

namespace ICTAZEVoting.Core.Mappings
{
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
}
