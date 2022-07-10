using AutoMapper;

using ICTAZEVoting.Core.Models;
using ICTAZEVoting.Shared.Responses.Audit;

namespace ICTAZEVoting.Core.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}