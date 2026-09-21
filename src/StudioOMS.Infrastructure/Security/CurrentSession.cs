using StudioOMS.Security.Session;

namespace StudioOMS.Security;


internal sealed class CurrentSession : ICurrentSession
{
    public SessionToken? Token { get; set; }
    public SessionInfo? Info { get; set; }
}