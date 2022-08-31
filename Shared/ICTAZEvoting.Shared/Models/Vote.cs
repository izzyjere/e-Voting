
using System;

namespace ICTAZEVoting.Shared.Models;

public class Vote 
{
    public Guid PositionId { get; set; }    
    public Guid CandidateId { get; set; }  
}
