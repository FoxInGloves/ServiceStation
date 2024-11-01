using System.ComponentModel.DataAnnotations;

namespace ServiceStation.Models.Entities.Abstraction;

public abstract class AbstractEntity
{
    //TODO спросить у гпт стоит ли модели делать record
    public Guid Id { get; set; } = Guid.NewGuid();
}