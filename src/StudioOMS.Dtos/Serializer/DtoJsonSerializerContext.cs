using StudioOMS.Customers;
using StudioOMS.Employees;
using StudioOMS.Me;
using StudioOMS.Orders;
using StudioOMS.Serializer.Converters;
using StudioOMS.Users;
using System.Text.Json.Serialization;

namespace StudioOMS.Serializer;


[JsonSourceGenerationOptions(
    PropertyNameCaseInsensitive = true,
    UseStringEnumConverter = true,
    Converters = [
        typeof(DateTimeOffsetConverter)
    ]
)]

/******************** Me ********************/
[JsonSerializable(typeof(LoginDto))]
[JsonSerializable(typeof(LoginResult))]
[JsonSerializable(typeof(ChangePasswordDto))]

/******************** User ********************/
[JsonSerializable(typeof(UserCreateDto))]
[JsonSerializable(typeof(UserCreateResult))]

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
public partial class DtoJsonSerializerContext : JsonSerializerContext;