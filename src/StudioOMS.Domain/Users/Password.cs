using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace StudioOMS.Users;


/// <summary>
/// 长度<see cref="MinLength"/>~<see cref="MaxLength"/> <br/>
/// 不包含控制字符和空白 <br/>
/// 大小写敏感
/// </summary>
public sealed record class Password
{
    public const int MinLength = 12;
    public const int MaxLength = 64;


    [JsonIgnore]
    private readonly string _value;
    private Password(string value) => _value = value;

    public string GetRawText() => _value;
    public override string ToString() => "***";



    public static Password From(ReadOnlySpan<char> value)
    {
        foreach (var c in value)
        {
            if (char.IsControl(c) || c == ' ')
                throw new ArgumentException("密码不可有空或空白字符");
        }

        if (value.Length < MinLength || value.Length > MaxLength)
        {
            throw new ArgumentException($"密码长度无效, 需要在 [{MinLength}~{MaxLength}] 之间", nameof(value));
        }

        var raw = string.Create(value.Length, value, static (span, source) => source.CopyTo(span));
        return new Password(raw);
    }
    public static Password Create()
    {
        const string chars = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789!@#$%";
        const int length = 16;

        var buffer = new char[length];
        for (var i = 0; i < length; i++)
        {
            buffer[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];
        }

        return From(buffer);
    }
}