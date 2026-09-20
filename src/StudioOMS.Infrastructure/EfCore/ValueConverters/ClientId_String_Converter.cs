using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudioOMS.Clients;

namespace StudioOMS.EfCore.ValueConverters;


internal sealed class ClientId_String_Converter : ValueConverter<ClientId, string>
{
    public ClientId_String_Converter() : base(
        id => id.ToString(),
        value => ClientId.Parse(value))
    {
    }
}