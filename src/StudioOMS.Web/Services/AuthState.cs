namespace StudioOMS.Web.Services;


public interface IAuthState
{
    IStudioOMSClient? Client { get; }
    bool IsAuthenticated { get; }
    event Action? OnChange;


    void SetClient(IStudioOMSClient Client);
    void Clear();
}

internal sealed class AuthState : IAuthState
{
    public IStudioOMSClient? Client { get; private set; }
    public bool IsAuthenticated => Client is not null;
    public event Action? OnChange;



    public void SetClient(IStudioOMSClient client)
    {
        Client = client;
        OnChange?.Invoke();
    }

    public void Clear()
    {
        Client = null;
        OnChange?.Invoke();
    }
}
