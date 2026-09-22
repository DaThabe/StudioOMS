using StudioOMS.Employees;
using StudioOMS.Messaging;

namespace StudioOMS.Users;


public sealed class UserCreateRequest : IRequest<UserId>
{
    public required Username Username { get; init; }
    public required Password Password { get; init; }
    public required EmployeeId EmployeeId { get; init; }


    internal sealed class Handler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher
        ) : IRequestHandler<UserCreateRequest, UserId>, IAllowAnonymous
    {
        public async ValueTask<UserId> HandleAsync(UserCreateRequest request,
            CancellationToken cancellationToken = default)
        {
            var passwordHash = await passwordHasher.HashAsync(request.Password, cancellationToken);

            var entity = User.Create(request.Username, passwordHash, request.EmployeeId);
            await userRepository.SaveAsync(entity, cancellationToken);

            return entity.Id;
        }
    }
}