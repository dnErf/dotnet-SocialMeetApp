using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using SocialMeetAPI.Data;
using SocialMeetAPI.Dtos;
using SocialMeetAPI.Models;

namespace SocialMeetAPI.Controllers
{
  // api/auth
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthRepository _repo;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public AuthController(
        IAuthRepository repo, 
        IConfiguration config,
        IMapper mapper
      )
    {
      _config = config;
      _mapper = mapper;
      _repo = repo;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
    {

      userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

      if (await _repo.UserExists(userForRegisterDto.Username))
        return BadRequest("Username already exists");

      /* without auto mapper
      var userToCreate = new User
      {
        Username = userForRegisterDto.Username
      };
      */

      var userToCreate = _mapper.Map<User>(userForRegisterDto);
      var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

      var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser);

      // return StatusCode(201); // recode to should be CreatedAtRoute()

      return CreatedAtRoute("GetUser", new { controller = "Users", id = createdUser.Id }, userToReturn);

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
    {

      var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

      if (userFromRepo == null)
        return Unauthorized();

      // create token
      var claims = new[]
      {
        new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
        new Claim(ClaimTypes.Name, userFromRepo.Username)
      };

      var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value) // token salt
      );

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(1),
        SigningCredentials = creds
      };

      var tokenHandler = new JwtSecurityTokenHandler();

      var token = tokenHandler.CreateToken(tokenDescriptor);

      var user = _mapper.Map<UserForListDto>(userFromRepo);

      if (token != null)
      {
        return Ok( new {
          token = tokenHandler.WriteToken(token),
          user
        });
      }
      else 
      {
        return Unauthorized();
      }
      
    }

  }
}