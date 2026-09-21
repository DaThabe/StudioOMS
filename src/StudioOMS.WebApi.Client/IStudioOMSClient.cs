using StudioOMS.Clients;
using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;
using StudioOMS.Users;

namespace StudioOMS;


public interface IStudioOMSClient
{
    ValueTask<LoginResult> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default);


    IUser User { get; }
    IEmployee Employee { get; }
    IClient Client { get; }
    IOrder Order { get; }



    public interface IUser
    {
        ValueTask<UserCreateResult> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken = default);
        ValueTask ChangePasswordAsync(UserChangePasswordDto dto, CancellationToken cancellationToken = default);
    }

    public interface IEmployee
    {

        ValueTask<EmployeeCreateResult> ECreateAsync(EmployeeCreateDto dto, CancellationToken cancellationToken = default);
        ValueTask RenamAsync(EmployeeRenameDto dto, CancellationToken cancellationToken = default);
    }
    public interface IClient
    {

        ValueTask<ClientCreateResult> CreateAsync(ClientCreateDto dto, CancellationToken cancellationToken = default);
        ValueTask RenamAsync(ClientRenameDto dto, CancellationToken cancellationToken = default);
    }
    public interface IOrder
    {
        ValueTask AssignEmployeeAsync(OrderAssignEmployeeDto dto, CancellationToken cancellationToken = default);
        ValueTask<OrderListResult> ListAsync(OrderListDto dto, CancellationToken cancellationToken = default);


        ValueTask<OrderCreateResult> CreateTimingAsync(TimingOrderCreateDto dto, CancellationToken cancellationToken = default);
        ValueTask ConsumeTimingAsync(TimingOrderConsume dto, CancellationToken cancellationToken = default);
    }
}
