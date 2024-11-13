using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamAppAPI.Data;
using TeamAppAPI.Model;

namespace TeamAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _dapper;


        public UsersController(IConfiguration config)
        {
            _dapper = new ApplicationDbContext(config);
        }


        [HttpGet("GetUsers")]
        public async Task<IEnumerable<Users>> GetUsersFromDbAsync()
        {
            IEnumerable<Users> users = await _dapper.SelectAllAsync<Users>("SELECT * FROM Users");
            return users;
        }


    }
}
