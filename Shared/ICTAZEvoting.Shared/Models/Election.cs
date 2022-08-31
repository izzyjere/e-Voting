using ICTAZEVoting.Shared.Contracts;
using ICTAZEVoting.Shared.Enums;

using ICTAZEVoting.Shared.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICTAZEVoting.Shared.Models;

public class Election : Entity<Guid>
{
    [Required]
    public DateTime? ElectionDate { get; set; }
    [Required(ErrorMessage ="Please provide a description for the election.")]
    public string Name { get; set; }
    [Required]
    public Guid ElectionTypeId { get; set; }
    [Required,Range(1,48)]
    public double Duration { get; set; }
    public ElectionStatus Status {
        get
        {
            if(ClosingTime<= DateTime.Now)
            {
                return ElectionStatus.Closed;
            }
            else if(ClosingTime>= DateTime.Now && ElectionDate<=DateTime.Now)
            {
                return ElectionStatus.Open;
            }
            else
            {
                return ElectionStatus.Pending;
            }
        }
    }
    public bool IsCurrent { get=>Status == ElectionStatus.Open; }
    public List<ElectionVoter> Voters { get; set; }
    public List<ElectionPosition> Positions { get; set; }
    public DateTime ClosingTime => ElectionDate.Value.AddHours(Duration);
    public ElectionType Type { get; set; }         
    public Election()
    {
        Id = Guid.NewGuid();
        ElectionDate = DateTime.Today.AddDays(1);
        Name = $"{DateTime.Today.Year} General Elections";
        ElectionTypeId = default;
    }
}
