public interface IUserRepository : IRepository<UserEntity>
{
    Task<UserEntity> GetByUserName(string username);
}