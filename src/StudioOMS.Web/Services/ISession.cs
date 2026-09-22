using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace StudioOMS.Services;


public interface ISession
{
    string UserId { get; }
    IStudioOMSClient Client { get; }
    bool IsAuthenticated { get; }


    ValueTask SignInAsync(string token, CancellationToken cancellationToken = default);
    ValueTask SignOutAsync(CancellationToken cancellationToken = default);
}


internal sealed class SessionStateProvider(
        IStudioOMSClientFactory factory,
        IJSRuntime js
    ) : AuthenticationStateProvider, ISession
{
    public string UserId
    {
        get => field ?? throw new InvalidOperationException("未登录");
        private set;
    }
    public IStudioOMSClient Client
    {
        get => field ?? throw new InvalidOperationException("未登录");
        private set;
    }

    public bool IsAuthenticated { get; private set; }


    // 登入
    async ValueTask ISession.SignInAsync(string token, CancellationToken cancellationToken)
    {
        await js.InvokeVoidAsync("localStorage.setItem", "oms_token", token);

        if (await TryRecoverAsync(token, cancellationToken))
            NotifyAuthenticationStateChanged(Task.FromResult(Authenticated()));
    }

    // 登出
    async ValueTask ISession.SignOutAsync(CancellationToken cancellationToken)
    {
        await js.InvokeVoidAsync("localStorage.removeItem", "oms_token");

        Client = null!;
        UserId = null!;
        IsAuthenticated = false;

        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous()));
    }

    // 获取授权状态
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (IsAuthenticated) return Authenticated();

        // 加载缓存
        var token = await js.InvokeAsync<string?>("localStorage.getItem", "oms_token");
        if (string.IsNullOrEmpty(token)) return Anonymous();

        // 恢复状态
        var result = await TryRecoverAsync(token, default);
        return result ? Authenticated() : Anonymous();
    }

    // 尝试恢复状态
    private async Task<bool> TryRecoverAsync(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = factory.CreateWithAuthentication(new("Bearer", token));
            var meInfo = await client.Me.GetInfoAsync(cancellationToken);

            Client = client;
            UserId = meInfo.Id;

            return IsAuthenticated = true;
        }
        catch (Exception)
        {
            Client = null!;
            UserId = null!;

            return IsAuthenticated = false;
        }
    }

    // 匿名
    private static AuthenticationState Anonymous()
    {

        return new(new ClaimsPrincipal(new ClaimsIdentity()));
    }
    // 已认证
    private static AuthenticationState Authenticated()
    {
        var identity = new ClaimsIdentity([new Claim(ClaimTypes.Name, "user")], "oms");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }
}