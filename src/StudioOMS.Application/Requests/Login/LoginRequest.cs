using StudioOMS.Security;
using StudioOMS.Security.Session;
using StudioOMS.Users;

namespace StudioOMS.Requests.Login;


public sealed class LoginRequest : IRequest<SessionToken>
{
    public required string Username { get; init; }
    public required string Password { get; init; }


    public static LoginRequest FromDto(LoginDto dto)
    {
        return new()
        {
            Username = dto.Username,
            Password = dto.Password
        };
    }

    internal sealed class Handler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ISessionService sessionService
        ) : IRequestHandler<LoginRequest, SessionToken>, IAllowAnonymous
    {
        public async ValueTask<SessionToken> HandleAsync(LoginRequest request,
            CancellationToken cancellationToken = default)
        {
            var user = await userRepository.FindByUsername(request.Username, cancellationToken)
                ?? throw new InvalidOperationException($"用户名或密码错误");

            var pass = await passwordHasher.VerifyAsync(request.Password, user.PasswordHash, cancellationToken);
            if (!pass) throw new InvalidOperationException($"用户名或密码错误");

            return await sessionService.CreateAsync(user.Id, user.EmployeeId, cancellationToken);
        }
    }
}