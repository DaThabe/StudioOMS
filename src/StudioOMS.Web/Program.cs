using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StudioOMS.Web;
using StudioOMS.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 认证
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, StudioOMSAuthenticationStateProvider>();
builder.Services.AddScoped(sp => (StudioOMSAuthenticationStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());
builder.Services.AddCascadingAuthenticationState();

// 请求客户端
builder.Services.AddStudioOMSClient(new Uri("http://localhost:5281"));


//builder.Services.AddSingleton<IAuthState, AuthState>();
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5281") });

await builder.Build().RunAsync();
