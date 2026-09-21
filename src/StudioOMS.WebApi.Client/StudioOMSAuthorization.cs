using StudioOMS.Extensions;
using StudioOMS.Login;
using StudioOMS.Serializer;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization.Metadata;

namespace StudioOMS;


public sealed record class StudioOMSAuthorization : IEquatable<StudioOMSAuthorization>
{
    public required string Scheme { get; init; }
    public required string Param { get; init; }


    private StudioOMSAuthorization() { }
    public override string ToString() => $"{Scheme} {Scheme[..3]}...{Scheme[^3..]}";

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(string.GetHashCode(Scheme, StringComparison.OrdinalIgnoreCase));
        hashCode.Add(Param);

        return hashCode.ToHashCode();
    }
    public bool Equals(StudioOMSAuthorization? other)
    {
        if (!string.Equals(Scheme, other?.Scheme, StringComparison.OrdinalIgnoreCase)) return false;
        if (!string.Equals(Param, other?.Param)) return false;
        return true;
    }




    public static StudioOMSAuthorization Create(string scheme, string param)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(scheme);
        ArgumentException.ThrowIfNullOrWhiteSpace(param);

        return new StudioOMSAuthorization()
        {
            Scheme = scheme.Trim(),
            Param = param.Trim()
        };
    }
}