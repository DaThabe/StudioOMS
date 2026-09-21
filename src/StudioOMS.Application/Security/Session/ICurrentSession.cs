namespace StudioOMS.Security.Session;


/// <summary>
/// 当前会话
/// </summary>
public interface ICurrentSession
{
    SessionToken? Token { get; set; }
    SessionInfo? Info { get; set; }
}