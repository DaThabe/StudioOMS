using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace StudioOMS.IdGenerator;


[Generator]
public sealed class IdGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var candidates = context.SyntaxProvider.ForAttributeWithMetadataName(
            "StudioOMS.IdAttribute",
            predicate: (node, _) => node is RecordDeclarationSyntax,
            transform: (ctx, _) => (RecordDeclarationSyntax)ctx.TargetNode);

        context.RegisterSourceOutput(candidates, (spc, record) =>
        {
            var name = record.Identifier.Text;
            var ns = GetNamespace(record)!;

            var source = GenerateSource(ns, name);
            spc.AddSource($"{name}.g.cs", source);
        });
    }

    private static string? GetNamespace(RecordDeclarationSyntax record)
    {
        var parent = record.Parent;
        while (parent is not null)
        {
            if (parent is BaseNamespaceDeclarationSyntax ns)
                return ns.Name.ToString();

            parent = parent.Parent;
        }
        return null;
    }

    private static string GenerateSource(string @namespace, string typeName)
    {
        return $$"""
#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace {{@namespace}};


public sealed partial record class {{typeName}} : ISpanParsable<{{typeName}}>
{
    private readonly Guid _value;
    private {{typeName}}(Guid value) => _value = value;

    public override string ToString() => _value.ToString("N");


    /// <inheritdoc/>
    /// <exception cref="ArgumentException"></exception>
    public static {{typeName}} From(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id 不可为空", nameof(id));

        return new(id);
    }
    public static bool TryFrom(Guid id, [MaybeNullWhen(false)] out {{typeName}}? result)
    {
        if (id == Guid.Empty)
        {
            result = default;
            return false;
        }

        result = new(id);
        return true;
    }
    public static {{typeName}} Create() =>
        new(Guid.CreateVersion7());


    /// <inheritdoc/>
    /// <exception cref="FormatException"></exception>
    public static {{typeName}} Parse(string guid) =>
        Parse(guid.AsSpan(), null);

    /// <inheritdoc/>
    /// <exception cref="FormatException"></exception>
    public static {{typeName}} Parse(string guid, IFormatProvider? provider) =>
        Parse(guid.AsSpan(), provider);
    
    /// <inheritdoc/>
    /// <exception cref="FormatException"></exception>
    public static {{typeName}} Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (!TryParse(s, provider, out var result))
        {
            throw new FormatException("Id 格式错误");
        }

        return result;
    }

    public static bool TryParse(string? guid, [MaybeNullWhen(false)] out {{typeName}} result) =>
        TryParse(guid.AsSpan(), null, out result);
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out {{typeName}} result) =>
        TryParse(s.AsSpan(), provider, out result);
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out {{typeName}} result)
    {
        if (!Guid.TryParse(s, out var guidResult))
        {
            result = default;
            return false;
        }

        return TryFrom(guidResult, out result);
    }
}
""";
    }
}