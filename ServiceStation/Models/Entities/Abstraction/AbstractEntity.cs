using System.ComponentModel.DataAnnotations;

namespace ServiceStation.Models.Entities.Abstraction;

public abstract class AbstractEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
}