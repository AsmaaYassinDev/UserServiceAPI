using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Collections.Concurrent;
using System.ComponentModel;
using UserServiceAPI.Models;
using UserServiceAPI.Services;

namespace UserServiceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
       
            private readonly IUserService _userService;

            public UserController(IUserService userService)
            {
                _userService = userService;
            }

            [HttpGet]
            public async Task<IActionResult> GetUsers()
            {
                var users = await _userService.GetUsersAsync();
                return Ok(users);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetUser(string id)
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null) return NotFound();
                return Ok(user);
            }

            [HttpPost]
            public async Task<IActionResult> CreateUser([FromBody] AppUser user)
            {
                await _userService.AddUserAsync(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
        }

    }
