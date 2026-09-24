using StudioOMS.Me;
using StudioOMS.WebApi.Responses;
using StudioOMS.WebApi.Serializer;
using System.Text.Json;

namespace StudioOMS.Serializer;


[TestClass]
public class SerializerTests
{
    [TestMethod]
    public void TestMethod1()
    {
        var response = Response.Success(new LoginResult() { Token = "123465" });

        var json = JsonSerializer.Serialize(response, ResponseJsonSerializerContext.Default.ResponseLoginResult);
        var parserdResponse = JsonSerializer.Deserialize(json, ResponseJsonSerializerContext.Default.ResponseLoginResult);

        Assert.AreEqual(response, parserdResponse);
    }
}
