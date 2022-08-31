
using System.ComponentModel.DataAnnotations;

namespace ICTAZEVoting.Shared.Requests;

public class RoleRequest
{
    public string Id { get; set; }

    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
}