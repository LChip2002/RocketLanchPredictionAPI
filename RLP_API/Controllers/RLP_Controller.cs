using Microsoft.AspNetCore.Mvc;
using RLP_API.Models;
using RLP_API.Services;
using RLP_DB.Models;
using Newtonsoft.Json;

namespace RLP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RLP_Controller : ControllerBase
    {
        private readonly LaunchQueryService _launchQueryService;

        // Constructor
        public RLP_Controller()
        {
            // Initialization code here
            _launchQueryService = new LaunchQueryService();
        }

        // GetLaunches endpoint
        [HttpGet]
        [Route("GetLaunches")]
        public async Task<IActionResult> GetLaunches([FromQuery] LaunchQuery launchQuery)
        {
            // Retrieve launch data from the database
            List<LaunchEntry> launchEntries = await _launchQueryService.GetLaunchDataAsync(launchQuery);

            // Checks if any entries are returned
            if (launchEntries.Count == 0)
            {
                return NoContent();
            }

            // Return the launch data
            return Ok(launchEntries);
        }

        // GET: api/RLP_Controller/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok($"Hello from RLP_Controller with id: {id}");
        }

        // POST: api/RLP_Controller
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            return CreatedAtAction(nameof(Post), new { value });
        }

        // PUT: api/RLP_Controller/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return NoContent();
        }

        // // DELETE: api/RLP_Controller/5
        // [HttpDelete("{id}")]
        // public IActionResult Delete(int id)
        // {
        //     return NoContent();
        // }
    }
}