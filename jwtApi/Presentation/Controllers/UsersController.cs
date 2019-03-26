using jwtApi.Core.Application.Users.Commands.AuthenticateUserCommand;
using jwtApi.Core.Application.Users.Commands.CreateUserCommand;
using jwtApi.Core.Application.Users.Commands.DeleteUserCommand;
using jwtApi.Core.Application.Users.Commands.UpdateUserCommand;
using jwtApi.Core.Application.Users.Models;
using jwtApi.Core.Application.Users.Queries.GetAllUsersQuery;
using jwtApi.Core.Application.Users.Queries.GetUserByIdQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Threading.Tasks;

namespace jwtApi.Presentation.Controllers.Users.V1
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        public UsersController()
        {
            Log.Debug("Create Users Controller");
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticateUserViewModel>> Authenticate([FromBody]AuthenticateUserCommand authenticateUserCommand)
        {
            return Ok(await Mediator.Send(authenticateUserCommand));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserViewModel>> Register([FromBody]CreateUserCommand createUserCommand)
        {
            return Ok(await Mediator.Send(createUserCommand));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<UserViewModel[]>> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllUsersQuery()));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User,Limited")]
        public async Task<ActionResult<UserViewModel>> GetByIdAsync(int id)
        {
            return Ok(await Mediator.Send(new GetUserByIdQuery { Id = id }));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<UserViewModel>> Update(int id, [FromBody]UpdateUserCommand updateUserCommand)
        {
            updateUserCommand.Id = id;
            return Ok(await Mediator.Send(updateUserCommand));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteUserCommand { Id = id });
            return Ok();
        }
    }
}