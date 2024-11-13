using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TeamAppAPI.Data;
using TeamAppAPI.Model;
using TeamAppAPI.Services;
using TeamAppAPI.ViewModel;

namespace TeamAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _dapper;
        private readonly Methods _methods;

        public LoginController(IConfiguration config, Methods methods)
        {
            _dapper = new ApplicationDbContext(config);
            _methods = methods; 
        }


        [HttpPost("Login")]
        public async Task<IActionResult> LoginApi(LoginViewModel loginData)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = _methods.HashPassword(loginData.Password);
                var user = await _dapper.LoadSingleDataAsync<Users>("SELECT * FROM Users WHERE Email = @Email", new { Email = loginData.Email });

                if (user == null)
                {
                    return NotFound(new { message = "This user does not exist!" });
                }

                var role = await _dapper.LoadSingleDataAsync<Roles>("SELECT r.Name FROM Roles r JOIN UserRoles ur ON ur.RoleId = r.Id WHERE ur.UserId = @UserId", new { UserId = user.Id });

                if (user != null && user.Email == loginData.Email && user.Password == hashedPassword)
                {


                    return Ok(new
                    {
                        message = "Login successful",
                        user = user, 
                        role = role?.Name
                    });
                }
                else
                {
                    return Unauthorized(new { message = "The Email or Password are not correct." });
                }
            }

            return BadRequest(new { message = "Invalid data" });
        }


    }
}
