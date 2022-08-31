using System;
using System.ComponentModel.DataAnnotations;

namespace ICTAZEVoting.Shared.Contracts;

public interface IEntity
{

}
public interface IEntity<TKey>
{
    TKey Id { get; }
}
public class Entity<TKey> : IEntity<TKey>
{
    [Key]
    public virtual TKey Id { get; set; }

}
public interface IAuditableEntity<TKey> : IEntity<TKey>
{
    string CreatedBy { get; set; }
    DateTime TimeStamp { get; set; }
    string? LastUpdatedBy { get; set; }
    DateTime? LastModifiedOn { get; set; }
    string? RemoteIp { get; set; }
}
public abstract class AuditableEntity<TKey> : Entity<TKey>, IAuditableEntity<TKey>
{
    public string CreatedBy { get; set; }
    public DateTime TimeStamp { get; set; }
    public string? LastUpdatedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public string? RemoteIp { get; set; }
}
