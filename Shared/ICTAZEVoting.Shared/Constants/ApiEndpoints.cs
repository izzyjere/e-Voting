using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Constants
{
    public static class ApiEndpoints
    {
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
    }
}
