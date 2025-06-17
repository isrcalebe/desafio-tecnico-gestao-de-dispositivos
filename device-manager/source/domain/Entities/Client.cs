using DeviceManager.Common.Modeling;

namespace DeviceManager.Domain.Entities;

/*
    Nesse commit, manterei a estrutura do Client, Device, e Event simples, sem relacionamentos complexos.
    Quando tivermos uma base mais estável, trocaremos as propriedades de string por tipos de valor mais específicos (eg. ValueObject),
    e deixaremos o Client mais imutável, evitando setters públicos, o que é uma boa prática em DDD.

    Nos próximos commits, trocarei a função de criação das entidades para usar o Result Pattern. Por enquanto, manteremos o Create devolvendo a entidade diretamente.
*/

public class Client : AggregateRoot
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<Device> Devices { get; set; } = [];

    private Client()
        : base(Guid.CreateVersion7())
    {
    }

    public static Client Create(string name, string email, string? phone = null, bool status = true)
        => new()
        {
            Name = name,
            Email = email,
            Phone = phone,
            Status = status
        };
}
