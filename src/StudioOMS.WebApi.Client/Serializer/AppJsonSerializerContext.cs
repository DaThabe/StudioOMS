using StudioOMS.Clients;
using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;
using StudioOMS.Serializer.Converters;
using StudioOMS.Users;
using System.Text.Json.Serialization;

namespace StudioOMS.Serializer;


[JsonSourceGenerationOptions(
    UseStringEnumConverter = true,
    Converters = [
        typeof(DateTimeOffsetConverter)
    ]
)]
/******************** User ********************/
[JsonSerializable(typeof(UserCreateDto))]
[JsonSerializable(typeof(LoginDto))]
[JsonSerializable(typeof(LoginResult))]

/******************** Employee ********************/
[JsonSerializable(typeof(EmployeeCreateDto))]

/******************** Client ********************/
[JsonSerializable(typeof(ClientCreateDto))]

/******************** Order ********************/
[JsonSerializable(typeof(OrderAssignEmployeeDto))]
// Timing
[JsonSerializable(typeof(TimingOrderCreateDto))]
[JsonSerializable(typeof(TimingOrderConsumeDto))]
[JsonSerializable(typeof(OrderListResult))]
public partial class AppJsonSerializerContext : JsonSerializerContext;