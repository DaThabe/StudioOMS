using StudioOMS.Messaging;

namespace StudioOMS.Employees;


public sealed class EmployeeCreateRequest : IRequest
{
    public required EmployeeId Id { get; init; }
    public required string Name { get; init; }


    public static implicit operator EmployeeCreateRequest(EmployeeCreateDto dto)
    {
        return new()
        {
            Id = new EmployeeId(dto.Id),
            Name = dto.Name
        };
    }

    internal sealed class Handler(IEmployeeRepository employeeRepository) : IRequestHandler<EmployeeCreateRequest>
    {
        public async ValueTask HandleAsync(EmployeeCreateRequest request, CancellationToken cancellationToken = default)
        {
            var user = Employee.Create(request.Id);
            user.Rename(request.Name);

            await employeeRepository.SaveAsync(user, cancellationToken);
        }
    }
}