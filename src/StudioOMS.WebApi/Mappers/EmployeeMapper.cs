using StudioOMS.Customers;
using StudioOMS.Employees;

namespace StudioOMS.Mappers;

public static class EmployeeMapper
{
    public static EmployeeCreateRequest ToRequest(this EmployeeCreateDto dto) => new()
    {
        Name = EmployeeName.From(dto.Name),
        Roles = dto.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Enum.Parse<EmployeeRole>).ToHashSet()
    };
    public static EmployeeCreateResult ToEmployeeCreateResult(this EmployeeId value) => new()
    {
        EmployeeId = value.ToString()
    };


    public static CustomerRenameRequest ToRequest(this EmployeeRenameDto dto, Guid employeeId) => new()
    {
        Id = CustomertId.From(employeeId),
        Name = CustomerName.From(dto.Name)
    };
}
