using StudioOMS.Customers;
using StudioOMS.Employees;
using StudioOMS.Login;
using StudioOMS.Me;
using StudioOMS.Orders;
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


/******************** Login ********************/
[JsonSerializable(typeof(LoginDto))]
[JsonSerializable(typeof(LoginResult))]

/******************** User ********************/
[JsonSerializable(typeof(UserCreateDto))]
[JsonSerializable(typeof(UserCreateResult))]
[JsonSerializable(typeof(ChangePasswordDto))]

/******************** Employee ********************/
[JsonSerializable(typeof(EmployeeCreateDto))]
[JsonSerializable(typeof(EmployeeCreateResult))]

/******************** Customer ********************/
[JsonSerializable(typeof(CustomerCreateDto))]
[JsonSerializable(typeof(CustomerCreateResult))]

/******************** Order ********************/
[JsonSerializable(typeof(OrderAssignEmployeeDto))]
[JsonSerializable(typeof(OrderCreateResult))]
[JsonSerializable(typeof(OrderListResult))]
// Timing
[JsonSerializable(typeof(TimingOrderCreateDto))]
[JsonSerializable(typeof(TimingOrderConsumeDto))]
public partial class AppJsonSerializerContext : JsonSerializerContext;