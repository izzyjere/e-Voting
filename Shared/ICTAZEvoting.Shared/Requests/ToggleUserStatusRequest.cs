using System;

namespace ICTAZEVoting.Shared.Requests;

public class ToggleUserStatusRequest
{
    public bool ActivateUser { get; set; }
    public Guid UserId { get; set; }
}