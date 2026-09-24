using StudioOMS.Customers;
using StudioOMS.Me;
using StudioOMS.Responses;
using System.Text.Json.Serialization;

namespace StudioOMS.Serializer;


[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    UseStringEnumConverter = true
)]
[JsonSerializable(typeof(Response<NullResponseData>))]
// Login
[JsonSerializable(typeof(Response<LoginResult>))]
// Customer
[JsonSerializable(typeof(Response<CustomerCreateResult>))]
public partial class ResponseJsonSerializerContext : JsonSerializerContext;