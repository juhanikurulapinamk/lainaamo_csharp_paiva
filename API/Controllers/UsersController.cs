// API/Controllers/UsersController.cs

using API.CustomExceptions;
using API.Dtos;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserService _userService) : ControllerBase
    {
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
    }
}