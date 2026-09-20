using StudioOMS.Employees;
using StudioOMS.Security;
using StudioOMS.Users;

namespace StudioOMS.Requests.Users;


public sealed class UserCreateRequest : IRequest<UserId>
{
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required EmployeeId EmployeeId { get; init; }


    public static UserCreateRequest FromDto(UserCreateDto dto)
    {
        return new()
        {
            Username = dto.Username,
            Password = dto.Password,
            EmployeeId = EmployeeId.Parse(dto.EmployeeId)
        };
    }

    public static implicit operator UserCreateRequest(UserCreateDto dto)
    {
        return new UserCreateRequest()
        {
            Username = dto.Username,
            Password = dto.Password,
            EmployeeId = EmployeeId.Parse(dto.EmployeeId)
        };
    }

    internal sealed class Handler(IUserRepository userRepository, IPasswordHasher passwordHasher) : IRequestHandler<UserCreateRequest, UserId>
    {
        public async ValueTask<UserId> HandleAsync(UserCreateRequest request, CancellationToken cancellationToken = default)
        {
            var passwordHash = await passwordHasher.HashAsync(request.Password, cancellationToken);

            var user = User.Create(request.Username, passwordHash, request.EmployeeId);
            await userRepository.SaveAsync(user, cancellationToken);

            return user.Id;
        }
    }
}