using StudioOMS.Employees;
using StudioOMS.Messaging;

namespace StudioOMS.Users;


public sealed class UserCreateRequest : IRequest
{
    public required UserId Id { get; init; }
    public required string Password { get; init; }
    public required string Name { get; init; }
    public required EmployeeId EmployeeId { get; init; }


    public static implicit operator UserCreateRequest(UserCreateDto dto)
    {
        return new UserCreateRequest()
        {
            Id = new UserId(dto.Id),
            Name = dto.Name,
            Password = dto.Password,
            EmployeeId = new EmployeeId(dto.EmployeeId)
        };
    }

    internal sealed class Handler(IUserRepository userRepository, IPasswordHasher passwordHasher) : IRequestHandler<UserCreateRequest>
    {
        public async ValueTask HandleAsync(UserCreateRequest request, CancellationToken cancellationToken = default)
        {
            var passwordHash = await passwordHasher.HashAsync(request.Password, cancellationToken);

            var user = User.Create(request.Id, passwordHash, request.EmployeeId);
            user.ChangeName(request.Name);

            await userRepository.SaveAsync(user, cancellationToken);
        }
    }
}