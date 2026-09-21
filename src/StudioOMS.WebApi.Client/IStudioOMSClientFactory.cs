namespace StudioOMS;


public interface IStudioOMSClientFactory
{
    IStudioOMSClient CreateWithAuthentication(string scheme, string? parameter);
    IStudioOMSClient CreateWithAuthentication(Uri baseUrl, string scheme, string? parameter);


    Task<IStudioOMSClient> LoginAsync(Uri baseUrl, string username, string password, CancellationToken cancellationToken = default);
    Task<IStudioOMSClient> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
}


internal sealed class StudioOMSClientFactory(Uri defaultBaseUrl) : IStudioOMSClientFactory
{
    public IStudioOMSClient CreateWithAuthentication(Uri baseUrl, string scheme, string? parameter) =>
        new StudioOMSClient(baseUrl, new(scheme, parameter));
    public IStudioOMSClient CreateWithAuthentication(string scheme, string? parameter) =>
        CreateWithAuthentication(defaultBaseUrl, scheme, parameter);

    public async Task<IStudioOMSClient> LoginAsync(Uri baseUrl, string username, string password, CancellationToken cancellationToken = default) =>
        await StudioOMSClient.LoginAsync(baseUrl, new() { Username = username, Password = password }, cancellationToken);

    public Task<IStudioOMSClient> LoginAsync(string username, string password, CancellationToken cancellationToken = default) =>
        LoginAsync(defaultBaseUrl, username, password, cancellationToken);
}