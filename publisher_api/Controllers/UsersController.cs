using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using publisher_api.Dto;
using publisher_api.Services;
using rabbit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace publisher_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IRabbitService _rabbitService;
        public UsersController(IUsersService userService, IRabbitService rabbitService)
        {
            _usersService = userService;
            _rabbitService = rabbitService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken token)
        {
            return Ok(await _usersService.GetUsers(token));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _usersService.GetUser(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserDto userDto)
        {
            userDto.UserId = Guid.NewGuid();
            var user = await _usersService.AddUser(userDto);
            _rabbitService.Enqueue(JsonConvert.SerializeObject(userDto));
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UserDto userDto)
        {
            await _usersService.Update(id, userDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _usersService.Delete(id);
            return NoContent();
        }

    }

}
