using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserService userService) : ControllerBase
{
    private readonly UserService _userService = userService;

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserEntity>> GetUser(int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        return user;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserEntity>>> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(UserEntity user)
    {
        await _userService.AddUserAsync(user);
        return CreatedAtAction(nameof(GetUser), new { userId = user.Id }, user);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(int userId, UserEntity user)
    {
        if (userId != user.Id)
        {
            return BadRequest();
        }

        await _userService.UpdateUserAsync(user);
        return NoContent();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await _userService.DeleteUserAsync(userId);
        return NoContent();
    }
}
