﻿using ICTAZEVoting.Shared.Models;
using ICTAZEVoting.Shared.Responses;
using ICTAZEVoting.Shared.Responses.Domain;
using ICTAZEVoting.Shared.Wrapper;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ICTAZEVoting.Shared.Interfaces
{
    public interface IVoterService
    {
        Task<IResult<List<string>>> Register(Voter entity);
        Task<IResult> Update(Voter entity);
        Task<List<VoterResponse>> GetAll();      
        Task<Voter> GetById(string id);
        Task<IResult> Delete(string id);
        Task<IResult<VoterVerificationResponse>> VerifyVoter(VoterVerificationRequest request);
        Task<VoterResponse> GetByUserId(string id);
    }
}
