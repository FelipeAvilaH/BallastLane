public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserEntity> GetUserByIdAsync(int userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    public async Task<IEnumerable<UserEntity>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task AddUserAsync(UserEntity user)
    {
        user.Password = Security.CalculateMD5Hash(user.Password);
        await _userRepository.AddAsync(user);
    }

    public async Task UpdateUserAsync(UserEntity user)
    {
        await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteUserAsync(int userId)
    {
        await _userRepository.DeleteAsync(userId);
    }

    public async Task<UserEntity> GetByUsername(string username)
    {
       return await _userRepository.GetByUserName(username);
    }
}
