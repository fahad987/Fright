using System.Net;
using Ensure.Application.IService;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Ensure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ControllerBase
{
    private readonly ILoginService _loginService;
    private readonly IConnections _connections;


    public AuthController(ILoginService loginService, IConnections connections)
    {
        _loginService = loginService;
        _connections = connections;
    }
    [HttpPost("Login")]
    public async Task<ActionResult<Response<object>>> LoginAsync(Login model)
    {
        try
        {
            
            var response = await _loginService.LoginAsync(model);
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(response));
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
}