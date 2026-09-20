namespace StudioOMS.Users;


public interface IUserRepository : IRepository<User, UserId>
{
    ValueTask<User?> FindByUsername(string username, CancellationToken cancellationToken = default);
}