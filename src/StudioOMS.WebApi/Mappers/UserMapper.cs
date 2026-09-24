using StudioOMS.Employees;
using StudioOMS.Users;

namespace StudioOMS.Mappers;

public static class UserMapper
{
    public static UserCreateRequest ToRequest(this UserCreateDto dto) => new()
    {
        Username = Username.From(dto.Username),
        Password = Password.From(dto.Password),
        EmployeeId = EmployeeId.Parse(dto.EmployeeId)
    };



    public static UserCreateResult ToUserCreateResult(this UserId id) => new()
    {
        UserId = id.ToString()
    };
}