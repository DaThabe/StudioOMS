namespace StudioOMS.Users;


public interface IUserRepository : IRepository<User, UserId>
{
    ValueTask<User?> FindByUsername(Username username, CancellationToken cancellationToken = default);
}