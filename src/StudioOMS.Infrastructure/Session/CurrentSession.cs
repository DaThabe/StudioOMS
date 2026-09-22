using StudioOMS.Employees;
using StudioOMS.Exceptions;
using StudioOMS.Users;

namespace StudioOMS.Session;


internal sealed class CurrentSession : ICurrentSession
{
    public UserId UserId
    {
        get => field ?? throw new NotAuthenticatedException();
        private set;
    }

    public EmployeeId EmployeeId
    {
        get => field ?? throw new NotAuthenticatedException();
        private set;
    }


    public void Set(UserId userId, EmployeeId employeeId)
    {
        if (userId is null || employeeId is null)
            throw new ArgumentException("UserId 和 EmployeeId 不能为空");


        UserId = userId;
        EmployeeId = employeeId;
    }
}