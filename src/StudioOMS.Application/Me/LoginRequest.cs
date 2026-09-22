using StudioOMS.Messaging;
using StudioOMS.Session;
using StudioOMS.Users;

namespace StudioOMS.Me;


public sealed class LoginRequest : IRequest<SessionToken>
{
    public required Username Username { get; init; }
    public required string Password { get; init; }


    internal sealed class Handler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            ISessionManager sessionService
        ) : IRequestHandler<LoginRequest, SessionToken>, IAllowAnonymous
    {
        public async ValueTask<SessionToken> HandleAsync(LoginRequest request,
            CancellationToken cancellationToken = default)
        {
            var entity = await userRepository.FindByUsername(request.Username, cancellationToken)
                ?? throw new InvalidOperationException("用户名或密码错误");

            var pass = await passwordHasher.VerifyAsync(request.Password, entity.PasswordHash, cancellationToken);
            if (!pass) throw new InvalidOperationException("用户名或密码错误");

            return await sessionService.SignInAsync(entity.Id, cancellationToken);
        }
    }
}