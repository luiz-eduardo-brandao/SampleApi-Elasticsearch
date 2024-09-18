using Microsoft.AspNetCore.Mvc;
using SampleApi.Elastic.Models;
using SampleApi.Elastic.Services;

namespace SampleApi.Elastic.Controllers
{
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IElasticService _elasticService;

        public UsersController(ILogger<UsersController> logger, IElasticService elasticService)
        {
            _logger = logger;
            _elasticService = elasticService;
        }

        [HttpPost("create-index")]
        public async Task<IActionResult> CreateIndex(string indexName)
        {
            await _elasticService.CreateIndexIfNotExistsAsync(indexName);
            return Ok($"Index {indexName} created or already exists");
        }

        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            var result = await _elasticService.AddOrUpdate(user);
            return result ? Ok("User added or updated successfully")
                : StatusCode(500, "Erro adding or updating user");
        }

        [HttpPost("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            var result = await _elasticService.AddOrUpdate(user);
            return result ? Ok("User added or updated successfully")
                : StatusCode(500, "Erro adding or updating user");
        }

        [HttpGet("get-user/{key}")]
        public async Task<IActionResult> GetUser(string key)
        {
            var user = await _elasticService.Get(key);
            return user != null ? Ok(user) : NotFound("User not found");
        }

        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _elasticService.GetAll();

            return users != null ? Ok(users) : StatusCode(500, "Error retrieving users");
        }

        [HttpDelete("delete-user/{key}")]
        public async Task<IActionResult> DeleteUser(string key)
        {
            var result = await _elasticService.Remove(key);
            return result ? Ok("User deleted successfully") : StatusCode(500, "Error deleting user");
        }
    }
}
