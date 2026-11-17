// API/Controllers/UsersController.cs

using API.CustomExceptions;
using API.Dtos;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService _userService, IMapper _mapper) : ControllerBase
    {

        
    // PUT api/users/5/role
    [HttpPut("{id}/role")]
    [Authorize(Policy = "RequireAdminRole")]
    public async Task<ActionResult<UserDto>> SetRole(int id, SetRoleReq req)
    {
        try
        {
            var user = await _userService.SetRole(id, req.Role);

            return Ok(_mapper.Map<UserDto>(user));
        }
        catch (NotFoundException e)
        {
            return NotFound(new { title = "user not found", detail = e.Message });
        }
    }





        
    // kun attribuutille ei anneta parametria, niin käytetään controllerin roottia. esim: http://localhost:5108/api/users
        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _userService.GetAll();
            
            
            
            

            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

            return Ok(userDtos);

        }

        [HttpPost("register")]
        
        public async Task<ActionResult<RegisterRes>> Register(RegisterReq requestData)
        {

            try
            {
                var user = await _userService.Register(requestData.UserName, requestData.Password);
                return Ok(new RegisterRes
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Role = user.Role
                });
            }
            // toistaiseksi missään ei vielä heitetä tätä omaa poikkeusta
            // otetaan se käyttöön hetken päästä, kun tarkistetaan käyttäjänimen yksilöllisyys
            catch (UserRegistrationException e)
            {
                // 409 Conflict: pyyntö oli kelvollinen, mutta käyttäjänimi on jo varattu
                return Conflict(new { title = "Error creating user", detail = e.Message });
            }

            catch (Exception)
            {
                // Tuntemattoman virheen viestiä ei palauteta asiakkaalle, koska se voi
                // paljastaa sisäisiä tietoja (esim. SQL-lauseita). Tästä lisää
                // luvussa Keskitetty virheiden käsittely.
                return Problem(
                    title: "Error creating user",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        
        // UsersController.cs, Register-actionin alle

        [HttpPost("login")]
        public async Task<ActionResult<LoginRes>> Login(LoginReq requestData)
        {
            try
            {
                var token = await _userService.Login(requestData.UserName, requestData.Password);

                return new LoginRes { Token = token };
            }
            catch (NotFoundException e)
            {
                // Sama vastaus sekä tuntemattomalle käyttäjälle että väärälle salasanalle,
                // jotta vastauksesta ei voi päätellä, mitkä käyttäjänimet ovat olemassa
                return NotFound(new { title = "user not found", detail = e.Message });
            }
        }
    }
}