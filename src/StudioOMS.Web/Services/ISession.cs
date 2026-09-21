using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace StudioOMS.Web.Services;


public interface ISession
{
    IStudioOMSClient? Client { get; }


    Task SignInAsync(string token);
    Task SignOutAsync();
}

public static class ISessionExtensions
{
    extension(ISession session)
    {
        public bool IsSign => session.Client is not null;

        public IStudioOMSClient GetRequiredClient()
        {
            if (session.Client is null)
                throw new InvalidOperationException("未登录");

            return session.Client;
        }
    }
}



internal sealed class SessionStateProvider(IStudioOMSClientFactory factory, IJSRuntime js) : AuthenticationStateProvider, ISession
{
    private IStudioOMSClient? _client;
    public IStudioOMSClient? Client => _client;


    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_client is not null)
            return BuildAuthenticatedState();

        var token = await js.InvokeAsync<string?>("localStorage.getItem", "oms_token");
        if (string.IsNullOrEmpty(token))
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        _client = factory.CreateWithAuthentication(new("Bearer", token));
        return BuildAuthenticatedState();
    }

    public async Task SignInAsync(string token)
    {
        await js.InvokeVoidAsync("localStorage.setItem", "oms_token", token);
        _client = factory.CreateWithAuthentication(new("Bearer", token));
        NotifyAuthenticationStateChanged(Task.FromResult(BuildAuthenticatedState()));
    }

    public async Task SignOutAsync()
    {
        await js.InvokeVoidAsync("localStorage.removeItem", "oms_token");
        _client = null;
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
    }

    private static AuthenticationState BuildAuthenticatedState()
    {
        var identity = new ClaimsIdentity([new Claim(ClaimTypes.Name, "user")], "oms");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }
}