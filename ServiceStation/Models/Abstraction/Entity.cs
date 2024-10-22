using System.ComponentModel.DataAnnotations;

namespace ServiceStation.Models.Abstraction;

public class Entity
{
    [Key]
    public Guid Id { get; }  = Guid.NewGuid();
}