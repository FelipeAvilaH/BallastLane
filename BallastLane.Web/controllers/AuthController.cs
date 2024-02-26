using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;
     private readonly UserService _userService;

    public AuthController(JwtTokenService jwtTokenService, UserService userService)
    {
        _jwtTokenService = jwtTokenService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
    {
        
        var user = await _userService.GetByUsername(model.Username);

        if(user == null)
            return BadRequest();

        var hash = Security.CalculateMD5Hash(model.Password);

        if(user.Password != hash)
            return BadRequest();

        var token = _jwtTokenService.GenerateJwtToken(model.Username);
        return Ok(new { token });
    }
}
