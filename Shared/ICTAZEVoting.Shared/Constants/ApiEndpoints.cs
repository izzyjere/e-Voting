using System;
using System.Collections.Generic;
using System.Text;

namespace ICTAZEVoting.Shared.Constants
{
    public static class ApiEndpoints
    {
        public const string GetRoles = "roles";
        public const string GetPendingElections = "elections/pending";
        public const string AddRole = "roles/add";
        public const string GetUsers = "users";
        public const string GetUserRoles = "users/roles";
        public const string Login = "token";
        public const string FileUpload = "upload";
        public const string PictureUpload = "profile-picture";
        public const string UserProfile = "userprofile";
        public const string AddElectionType = "elections/types/add";
        public const string EditElectionType = "elections/types/update";
        public const string DeleteElectionType = "elections/types/delete";
        public const string GetElectionTypes = "elections/types";
        public const string AddPoliticalParty = "elections/parties/add";
        public const string EditPoliticalParty = "elections/parties/update";
        public const string DeletePoliticalParty = "elections/parties/delete";
        public const string GetPoliticalParties = "elections/parties";
        public const string AddVoter = "voters/add";
        public const string GetVoters = "voters";
        public const string UpdateVoter = "voters/update";
        public const string DeleteVoter = "voters/delete";
        public const string AddElection = "elections/add";
        public const string EditElection = "elections/update";
        public const string DeleteElection = "elections/delete";
        public const string GetElections = "elections";
        public const string AddConstituency = "constituencies/add";
        public const string EditConstituency = "constituencies/update";
        public const string DeleteConstituency = "constituencies/delete";
        public const string GetConstituencies = "constituencies";
        public const string AddPollingStation = "constituencies/polling-station/add";
        public const string EditPollingStation = "constituencies/polling-station/update";
        public const string DeletePolingStation = "constituencies/polling-station/delete";
        public const string GetPollingStations = "constituencies/polling-stations";
        public const string AddCandidate = "candidate/add";
        public const string EditCandidate = "candidate/update";
        public const string DeleteCandidate = "candidate/delete";
        public const string GetCandidates = "candidates";
        public const string AddSystemAdmin = "system-admins/add";
        public const string EditSystemAdmin = "system-admins/update";
        public const string DeleteSystemAdmin = "system-admins/delete";
        public const string GetSystemAdmins = "system-admins";   
        public const string GetSystemAdminByUserId = "system-admins/getbyuserid";
        public const string GetPollingStationsByUserId = "polling-stations/getbyuserid";
    }
}
