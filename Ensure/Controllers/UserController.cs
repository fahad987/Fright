using System.Net;
using Ensure.Application.IService;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using EnsureFreightInc.Entities.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Ensure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController: ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConnections _connections;


    public UserController(IUserService userService, IConnections connections)
    {
        _userService = userService;
        _connections = connections;
    }
    [HttpPut("UpdateUserIsActiveStatus")]
    public async Task<ActionResult<Response>> UpdateUserIsActiveStatusAsync(Guid id,bool isActive)
    {
        try
        {
            _connections.con.BeginTransaction();
            await _userService.UpdateUserIsActiveStatusAsync(id,isActive);
            _connections.con.CommitTransactionAndDispose();
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse());
        }
        catch (Exception e)
        {
            _connections.con.RollbackTransactionAndDispose();
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
    [HttpGet("GetUserById")]
    public async Task<ActionResult<Response>> GetUserByIdAsync(Guid id)
    {
        try
        {
            var result=await _userService.GetUserByIdAsync(id);
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(result));
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }

    [HttpPost("AddUser")]
    public async Task<ActionResult<Response<User>>> AddUserAsync([FromForm] NewUser model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _userService.AddUserAsync(model);
            _connections.con.CommitTransactionAndDispose();
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(response));
        }
        catch (Exception e)
        {
            _connections.con.RollbackTransactionAndDispose();
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
    [HttpPost("GetAllUser")]
    public async Task<ActionResult<Response<List<User>>>> GetAllUserAsync(UserFilter model)
    {
        try
        {
            var response = await _userService.GetAllUserAsync(model);
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(response));
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
    [HttpPut("UpdateUser")]
    public async Task<ActionResult<Response<User>>> UpdateUserAsync([FromForm] User model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _userService.UpdateUserAsync(model);
            _connections.con.CommitTransactionAndDispose();
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(response));
        }
        catch (Exception e)
        {
            _connections.con.RollbackTransactionAndDispose();
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
}