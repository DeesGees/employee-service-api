using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design.Serialization;
using System.Security.Claims;
using TeamAppAPI.Data;
using TeamAppAPI.Model;

namespace TeamAppAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly ApplicationDbContext _dapper;

        public EquipmentController(IConfiguration config)
        {
            _dapper = new ApplicationDbContext(config);
        }


        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] string userId)
        {

            if (userId == null)
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            var data = await _dapper.LoadDataAsync<Equipment>("SELECT * FROM Equipment WHERE UserId = @UserId", new { UserId = userId });

            if (data == null || !data.Any())
            {
                return NotFound(new { message = "No equipment found for the user." });
            }

            return Ok(data);

        }

        [HttpGet]
        [Route("/api/Equipment/Edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            if(id == null)
            {
                return NotFound(new { message = "Item not found" });
            }

            var data = await _dapper.LoadSingleDataAsync<Equipment>("SELECT * FROM Equipment Where Id = @Id", new { Id = id });

            if (data == null)
            {
                return NotFound(new { message = "No equipment found for editing." });
            }

            return Ok(data);
        }


        [HttpPost]
        [Route("/api/Equipment/Edit")]
        public async Task<IActionResult> Edit([FromBody]Equipment eqp)
        {
            if (eqp == null)
            {
                return BadRequest(new { message = "No data was found!" });

            }

            int result = await _dapper.ExecuteQueryAsync("UPDATE Equipment SET TypeOfEqp = @TypeOfEqp, Model = @Model, SerialNumber = @SerialNumber, InUse = @InUse, UserId = @UserId WHERE Id = @Id", new { TypeOfEqp = eqp.TypeOfEqp, Model = eqp.Model, SerialNumber = eqp.SerialNumber, InUse = eqp.InUse, UserId = eqp.UserId, Id = eqp.Id });


            if(result > 0)
            {
                return Ok(new { message = "Item was updated" });
            }
            else
            {
                return NotFound(new { message = "Equipment not found." });
            }
        }


        [HttpGet]
        [Route("/api/Equipment/Delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(id == null)
            {
                return BadRequest(new { message = "No data was found!" });
            }

            var data = await _dapper.LoadSingleDataAsync<Equipment>("SELECT * FROM Equipment WHERE Id = @Id", new { Id = id });

            if (data == null)
            {
                return NotFound(new { message = "No equipment found for editing." });
            }

            return Ok(data);
        }


        [HttpDelete]
        [Route("/api/Equipment/DeleteItem/{id}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int id)
        {
            if (id == 0)
            {
                return BadRequest(new { message = "Invalid ID provided!" });
            }

            var rowsAffected = await _dapper.ExecuteQueryAsync("DELETE FROM Equipment WHERE Id = @Id", new { Id = id });

            if (rowsAffected > 0)
            {
                return Ok(new { message = "Equipment deleted successfully!" });
            }
            else
            {
                return NotFound(new { message = "No equipment found to delete." });
            }
        }


        [HttpGet]
        [Route("/api/Equipment/Add")]
        public async Task<IActionResult> Add()
        {
            var data = await _dapper.SelectAllAsync<Users>("SELECT * FROM Users");

            if(data != null)
            {
                return Ok(data);
            }
            else
            {
                return NotFound(new {message = "Useres were not found!"});
            }
        }


        [HttpPost]
        [Route("/api/Equipment/AddItem")]
        public async Task<IActionResult> Add([FromBody]Equipment eqp)
        {
            if(eqp == null)
            {
                return BadRequest(new {message = "Item could not be added!"});
            }

            var data = await _dapper.ExecuteQueryAsync("INSERT INTO Equipment(TypeOfEqp, Model, SerialNumber, InUse, UserId) VALUES(@TypeOfEqp, @Model, @SerialNumber, @InUse, @UserId)", new { TypeOfEqp = eqp.TypeOfEqp, @Model = eqp.Model, @SerialNumber = eqp.SerialNumber, InUse = eqp.InUse, UserId = eqp.UserId });

            if(data > 0)
            {
                return Ok(data);
            }
            else
            {
                return BadRequest(new {message = "Item could not be added!"});
            }


        }

    }
}
