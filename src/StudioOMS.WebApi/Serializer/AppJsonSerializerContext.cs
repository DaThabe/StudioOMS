using StudioOMS.Orders;
using StudioOMS.Orders.Timing;
using System.Text.Json.Serialization;


[JsonSerializable(typeof(OrderAssignEmployeeRequest))]
[JsonSerializable(typeof(TimingOrderConsumeRequest))]
[JsonSerializable(typeof(TimingOrderCreateRequest))]
public partial class AppJsonSerializerContext : JsonSerializerContext;