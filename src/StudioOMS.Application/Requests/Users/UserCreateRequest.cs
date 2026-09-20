using StudioOMS.Employees;
using StudioOMS.Security;
using StudioOMS.Users;

namespace StudioOMS.Requests.Users;


public sealed class UserCreateRequest : IRequest
{
    public required string Username { get; init; }
    public required string Password { get; init; }
    public required EmployeeId EmployeeId { get; init; }

    public UserId Id { get; init; } = UserId.Create();


    public static implicit operator UserCreateRequest(UserCreateDto dto)
    {
        return new UserCreateRequest()
        {
            Username = dto.Username,
            Password = dto.Password,
            EmployeeId = new EmployeeId(dto.EmployeeId)
        };
    }

    internal sealed class Handler(IUserRepository userRepository, IPasswordHasher passwordHasher) : IRequestHandler<UserCreateRequest>
    {
        public async ValueTask HandleAsync(UserCreateRequest request, CancellationToken cancellationToken = default)
        {
            var passwordHash = await passwordHasher.HashAsync(request.Password, cancellationToken);

            var user = User.Create(request.Id, request.Username, passwordHash, request.EmployeeId);

            await userRepository.SaveAsync(user, cancellationToken);
        }
    }
}