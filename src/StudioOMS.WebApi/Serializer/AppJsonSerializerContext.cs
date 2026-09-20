using StudioOMS.Clients;
using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;
using StudioOMS.Users;
using System.Text.Json.Serialization;


[JsonSourceGenerationOptions(
    UseStringEnumConverter = true)]
/******************** User ********************/
[JsonSerializable(typeof(UserCreateDto))]
[JsonSerializable(typeof(UserLoginDto))]

/******************** Employee ********************/
[JsonSerializable(typeof(EmployeeCreateDto))]

/******************** Client ********************/
[JsonSerializable(typeof(ClientCreateDto))]

/******************** Order ********************/
[JsonSerializable(typeof(OrderAssignEmployeeDto))]
// Timing
[JsonSerializable(typeof(TimingOrderCreateDto))]
[JsonSerializable(typeof(TimingOrderConsumeDto))]
public partial class AppJsonSerializerContext : JsonSerializerContext;