using Domain.Contracts.Repositories;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Server.Models;
using TicTacToe.Server.Services;

namespace TicTacToe.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;

    public UserController(IUserRepository userRepository, TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(UserCredentials credentials, CancellationToken cancellationToken)
    {
        await _userRepository.CreateAsync(credentials.Name, credentials.Password, cancellationToken);

        return Ok();
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> Login(UserCredentials credentials, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByNameAndPasswordAsync(
            credentials.Name, credentials.Password, cancellationToken);

        if (user is not null)
        {
            var token = _tokenService.GenerateToken(user.Id, user.Name);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            var tokenName = "token" + credentials.Side;

            Response.Cookies.Append(tokenName, token, cookieOptions);

            user.Token = token;

            return Ok(user);
        }
        else
        {
            return NotFound();
        }
    }
}