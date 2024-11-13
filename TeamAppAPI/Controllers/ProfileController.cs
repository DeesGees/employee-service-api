using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamAppAPI.Data;
using TeamAppAPI.Model;
using TeamAppAPI.Services;

namespace TeamAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _dapper;
        private readonly Methods _methods;

        public ProfileController(IConfiguration config, Methods methods)
        {
            _dapper = new ApplicationDbContext(config);
            _methods = methods;
        }


        [HttpGet]
        [Route("/api/Profile")]
        public async Task<IActionResult> Index([FromQuery] string user)
        {
            if (string.IsNullOrEmpty(user))
            {
                return Unauthorized(new { message = "User is not logged in." });
            }

            var data = await _dapper.LoadSingleDataAsync<Users>(@"SELECT * FROM Users where Name = @Name", new { Name = user });
            return Ok(data);
        }
    }
}
