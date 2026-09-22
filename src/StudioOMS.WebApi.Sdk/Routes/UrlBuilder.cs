using System.Text;

namespace StudioOMS.Routes;

public sealed class UrlBuilder
{
    private readonly string _baseUrl;
    private readonly List<string> _segments = [];
    private readonly List<KeyValuePair<string, string?>> _query = [];

    public UrlBuilder(Uri uri)
    {
        _baseUrl = uri.GetLeftPart(UriPartial.Path).TrimEnd('/');

        // Query 属性带前导 '?'，去掉后按 & 拆
        var query = uri.Query;
        if (query.Length > 0 && query[0] == '?')
            query = query[1..];

        if (query.Length == 0) return;

        foreach (var pair in query.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var eq = pair.IndexOf('=');
            _query.Add(eq >= 0
                ? new(Uri.UnescapeDataString(pair[..eq]), Uri.UnescapeDataString(pair[(eq + 1)..]))
                : new(Uri.UnescapeDataString(pair), null));
        }
    }
    public UrlBuilder(string baseUrl)
    {
        var idx = baseUrl.IndexOf('?');
        if (idx >= 0)
        {
            _baseUrl = baseUrl[..idx].TrimEnd('/');

            foreach (var pair in baseUrl[(idx + 1)..].Split('&', StringSplitOptions.RemoveEmptyEntries))
            {
                var eq = pair.IndexOf('=');
                _query.Add(eq >= 0
                    ? new(Uri.UnescapeDataString(pair[..eq]), Uri.UnescapeDataString(pair[(eq + 1)..]))
                    : new(Uri.UnescapeDataString(pair), null));
            }
        }
        else
        {
            _baseUrl = baseUrl.TrimEnd('/');
        }
    }


    /// <summary>添加一个路径段，会自动做 URL 转义</summary>
    public UrlBuilder AddPath(string segment)
    {
        if (string.IsNullOrEmpty(segment)) return this;
        _segments.Add(Uri.EscapeDataString(segment));
        return this;
    }
    /// <summary>添加一些路径段，会自动做 URL 转义</summary>
    public UrlBuilder AddPaths(params IEnumerable<string> segments)
    {
        foreach (var i in segments) AddPath(i);
        return this;
    }

    /// <summary>添加一个 query 参数（null 值会被跳过或写成空，见下）</summary>
    public UrlBuilder AddQuery(string key, string? value)
    {
        if (string.IsNullOrEmpty(key)) return this;
        _query.Add(new(key, value));
        return this;
    }

    /// <summary>批量添加 query（比如从一个 DTO 来）</summary>
    public UrlBuilder AddQuery(IEnumerable<KeyValuePair<string, string?>> values)
    {
        foreach (var kv in values) AddQuery(kv.Key, kv.Value);
        return this;
    }


    public override string ToString()
    {
        var sb = new StringBuilder(_baseUrl);

        foreach (var seg in _segments)
            sb.Append('/').Append(seg);

        if (_query.Count > 0)
        {
            sb.Append('?');
            for (int i = 0; i < _query.Count; i++)
            {
                if (i > 0) sb.Append('&');
                sb.Append(Uri.EscapeDataString(_query[i].Key));
                sb.Append('=');
                sb.Append(Uri.EscapeDataString(_query[i].Value ?? string.Empty));
            }
        }

        return sb.ToString();
    }

    public Uri ToUri() => new(ToString(), UriKind.RelativeOrAbsolute);
    public static implicit operator string(UrlBuilder builder) => builder.ToString();
    public static implicit operator Uri(UrlBuilder builder) => builder.ToUri();
}
