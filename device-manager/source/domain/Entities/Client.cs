using DeviceManager.Common;
using DeviceManager.Common.Modeling;
using DeviceManager.Domain.ValueObjects;

namespace DeviceManager.Domain.Entities;

/*
    Nesse commit, manterei a estrutura do Client, Device, e Event simples, sem relacionamentos complexos.
    Quando tivermos uma base mais estável, trocaremos as propriedades de string por tipos de valor mais específicos (eg. ValueObject),
    e deixaremos o Client mais imutável, evitando setters públicos, o que é uma boa prática em DDD.

    Nos próximos commits, trocarei a função de criação das entidades para usar o Result Pattern. Por enquanto, manteremos o Create devolvendo a entidade diretamente.
*/

public class Client : AggregateRoot
{
    public ClientName Name { get; private set; }

    public Email Email { get; private set; }

    public PhoneNumber? Phone { get; private set; }

    public bool Status { get; set; }

    public virtual ICollection<Device> Devices { get; set; } = [];

#pragma warning disable CS8618
    private Client()
        : base(Guid.CreateVersion7())
#pragma warning restore CS8618
    {
    }

    public static Result<Client, Error> Create(string name, string email, string? phone = null, bool status = true)
    {
        var client = new Client();

        var nameResult = ClientName.Create(name);
        if (nameResult.IsFailure)
            return nameResult.Error;

        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            return emailResult.Error;

        client.Name = nameResult.Value;
        client.Email = emailResult.Value;

        if (!string.IsNullOrWhiteSpace(phone))
        {
            var phoneResult = PhoneNumber.Create(phone);
            if (phoneResult.IsFailure)
                return phoneResult.Error;

            client.Phone = phoneResult.Value;
        }

        client.Status = status;

        return client;
    }

    public static Result<Client, Error> Create(ClientName name, Email email, PhoneNumber? phone = null, bool status = true)
    {
        var client = new Client
        {
            Name = name,
            Email = email,
            Phone = phone,
            Status = status
        };

        return client;
    }
}
