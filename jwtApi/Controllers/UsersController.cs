using AutoMapper;
using jwtApi.Dto;
using jwtApi.Entities;
using jwtApi.Helpers;
using jwtApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Collections.Generic;

namespace jwtApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;

            Log.Debug("Create Users Controller");
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserAuthenticationDto request)
        {
            var user = _userService.Authenticate(request.Username, request.Password);

            var result = _mapper.Map<UserAuthenticatedDto>(user);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserNewRegisterDto userDto)
        {
            // map dto to entity
            var user = _mapper.Map<User>(userDto);

            // save 
            var newUser = _userService.Create(user, userDto.Password);
            var result = _mapper.Map<UserAuthenticatedDto>(newUser);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();

            var result = _mapper.Map<List<UserDto>>(users);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User,Limited")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult Update(int id, [FromBody]UserNewRegisterDto userDto)
        {
            // map dto to entity and set id
            var user = _mapper.Map<User>(userDto);
            user.Id = id;

            // save 
            var updatedUser = _userService.Update(user, userDto.Password);
            var result = _mapper.Map<UserDto>(updatedUser);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }
    }
}