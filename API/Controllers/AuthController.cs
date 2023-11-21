using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API.Auth.Dtos;
using API.Auth.Model;
using Microsoft.AspNetCore.Identity;
using API.Auth;
using API.Auth.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
namespace API.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenService _jtwTokenService;

        public AuthController(UserManager<User> userManager, JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jtwTokenService = jwtTokenService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDto registerUserDto)
        {
            var user = await _userManager.FindByNameAsync(registerUserDto.UserName);
            if (user != null)
            {
                return BadRequest("Request invalid."); // or "username already taken"
            }
            var newUser = new User
            {
                Email = registerUserDto.Email,
                UserName = registerUserDto.UserName
            };
            var createdUserResult = await _userManager.CreateAsync(newUser, registerUserDto.Password);
            if (!createdUserResult.Succeeded)
            {
                // return BadRequest("Could not create a user.");
                return BadRequest(string.Join(", ", createdUserResult.Errors.Select(e => e.Description)));
            }

            await _userManager.AddToRoleAsync(newUser, UserRoles.Member);

            return CreatedAtAction(nameof(Register), new UserDto(newUser.Id, newUser.UserName, newUser.Email));

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
            {
                return BadRequest("Username or password is invalid."); // UnprocessableEntity instead?
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                return BadRequest("Username or password is invalid.");
            }

            // donk but idk y, will do for now
            user.ForceRelogin = false;
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jtwTokenService.CreateAccesssToken(user.UserName, user.Id, roles);
            var refreshToken = _jtwTokenService.CreateRefreshToken(user.Id);

            return CreatedAtAction(nameof(Login), new SuccessfulLoginDto(accessToken, refreshToken));
            // return Ok(new SuccessfulLoginDto(accessToken));
        }

        [HttpPost]
        [Route("accessToken")] // Figure out IResult vs IActionResult sometime hmm?
        public async Task<IResult> AccessToken(RefreshAccessTokenDto refreshAccessTokenDto)
        {
            if (!_jtwTokenService.TryParseRefreshToken(refreshAccessTokenDto.RefreshToken, out var claims))
            {
                return Results.UnprocessableEntity();
            }
            var userId = claims.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Results.UnprocessableEntity("Invalid token");
            }
            if(user.ForceRelogin)
            {
                return Results.UnprocessableEntity();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = _jtwTokenService.CreateAccesssToken(user.UserName, user.Id, roles);
            var refreshToken = _jtwTokenService.CreateRefreshToken(user.Id);

            return Results.Ok(new SuccessfulLoginDto(accessToken, refreshToken));
        }
    }
}