using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Constants
{
    public static class ApiEndpoints
    {
        public const string GetRoles = "roles";
        public const string AddRole = "roles/add";
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
        public const string GetElection = "election";

       

    }
}
