using StudioOMS.Me;
using StudioOMS.Users;

namespace StudioOMS.Mappers;

public static class MeMapper
{
    public static ChangePasswordRequest ToRequest(this ChangePasswordDto dto)
    {
        return new()
        {
            CurrentPassword = dto.CurrentPassword,
            NewPassword = Password.From(dto.NewPassword)
        };
    }


    public static MeInfoResult ToInfoResult(this MeInfoResponse response)
    {
        return new()
        {
            Id = response.Id.ToString(),
            Username = response.Id.ToString(),
            EmployeeId = response.EmployeeId.ToString(),
            EmployeeName = response.EmployeeName.ToString()
        };
    }
}
