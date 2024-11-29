using System.ComponentModel.DataAnnotations;

namespace ServiceStation.Models.Entities.Abstraction;

public abstract class AbstractEntity
{
    //TODO спросить у гпт стоит ли модели делать record
    //TODO сменить init
    public Guid Id { get; init; } = Guid.NewGuid();
}