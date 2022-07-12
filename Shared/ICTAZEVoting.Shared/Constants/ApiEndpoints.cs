using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Constants
{
    public static class ApiEndpoints
    {
        public const string GetRoles = "roles";
        public const string AddRole = "roles/add";
        public const string GetUsers = "users";
        public const string Login = "token";
        public const string PictureUpload = "profile-picture";
        public const string UserProfile = "userprofile";
        public const string AddElectionType = "elections/types/add";
        public const string EditElectionType = "elections/types/update";
        public const string DeleteElectionType = "elections/types/delete";
        public const string GetElectionTypes = "elections/types";
        public const string AddPoliticalParty = "elections/parties/add";
        public const string EditPoliticalParty = "elections/parties/update";
        public const string DeletePoliticalParty = "elections/parties/delete";
        public const string GetPoliticalParties= "elections/parties";
        public const string AddVoter = "voters/add";
        public const string GetVoters = "voters";
        public const string UpdateVoter = "voters/update";
        public const string DeleteVoter = "voters/delete";
        public const string AddElection = "elections/add";
        public const string EditElection = "elections/update";
        public const string DeleteElection = "elections/delete";
        public const string GetElections = "election";
        public const string AddConstituency = "constituency/add";
        public const string EditConstituency = "constituency/update";
        public const string DeleteConstituency = "constituency/delete";
        public const string GetConstituencies = "constituencies";
        public const string AddPolingStation = "polingstation/add";
        public const string EditPolingStation = "polingstation/update";
        public const string DeletePolingStation = "polingstation/delete";
        public const string GetPolingStations = "polingstations";
        public const string AddCandidate = "candidate/add";
        public const string EditCandidate = "candidate/update";
        public const string DeleteCandidate = "candidate/delete";
        public const string GetCandidates = "candidates";


    }
}
