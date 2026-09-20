using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudioOMS.Users;

namespace StudioOMS.EfCore.ValueConverters;

internal sealed class UserId_String_Converter : ValueConverter<UserId, string>
{
    public UserId_String_Converter() : base(
        id => id.ToString(),
        value => UserId.Parse(value))
    {
    }
}
