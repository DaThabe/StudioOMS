using Moq;
using RichardSzalay.MockHttp;
using StudioOMS.Me;
using StudioOMS.Messaging;
using StudioOMS.Responses;
using StudioOMS.Serializer;
using System.Text.Json;

namespace StudioOMS;


[TestClass]
public class StudioOMSAuthenticationTests
{
    [TestMethod]
    public async Task LoginAsync()
    {
        var loginResultResponse = Response.Success(new LoginResult { Token = "123" });
        var loginResultResponseJson = JsonSerializer.Serialize(loginResultResponse, ResponseJsonSerializerContext.Default.ResponseLoginResult);

        // Response
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When(HttpMethod.Post, "https://localhost/api/login")
            .Respond("application/json", loginResultResponseJson);

        // HttpClient
        var httpClient = mockHttp.ToHttpClient();
        httpClient.BaseAddress = new Uri("https://localhost");

        // IHttpClientFactory
        var factory = new Mock<IHttpClientFactory>();
        factory.Setup(f => f.CreateClient("StudioOMS")).Returns(httpClient);



        // Act
        var authentication = new StudioOMSAuthentication(factory.Object);
        var result = await authentication.LoginAsync(new() { Username = "123", Password = "123" }, TestContext.CancellationToken);

        // Assert
        Assert.AreEqual(loginResultResponse, result);
    }

    public TestContext TestContext { get; set; }
}
