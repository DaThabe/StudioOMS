using StudioOMS.Orders;
using StudioOMS.Orders.Timing;
using System.Text.Json.Serialization;


[JsonSerializable(typeof(OrderAssignEmployeeDto))]
[JsonSerializable(typeof(TimingOrderConsumeDto))]
[JsonSerializable(typeof(TimingOrderCreateDto))]
public partial class AppJsonSerializerContext : JsonSerializerContext;