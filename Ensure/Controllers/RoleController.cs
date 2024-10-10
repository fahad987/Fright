using System.Net;
using Ensure.Application.IService;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Ensure.Controllers;

[ApiController]

[Route("api/[controller]")]
public class RoleController: ControllerBase
{
    private readonly IRoleService _roleService;
    private readonly IConnections _connections;

    public RoleController(IRoleService roleService, IConnections connections)
    {
        _roleService = roleService;
        _connections = connections;
    }
    [HttpPut("UpdateRoleIsActiveStatus")]
    public async Task<ActionResult<Response>> UpdateRoleIsActiveStatusAsync(Guid id,bool isActive)
    {
        try
        {
            _connections.con.BeginTransaction();
            await _roleService.UpdateRoleIsActiveStatusAsync(id,isActive);
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

    [HttpPost("GetAllRole")]
    public async Task<ActionResult<Response<List<Role>>>> GetAllRoleAsync(RoleFilter model)
    {
        try
        {
            var response = await _roleService.GetAllRoleAsync(model);
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(response));
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
    [HttpPost("AddRole")]
    public async Task<ActionResult<Response<Role>>> AddRoleAsync(Role model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _roleService.AddRoleAsync(model);
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
    [HttpPut("UpdateRole")]
    public async Task<ActionResult<Response<Role>>> UpdateRoleAsync(Role model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _roleService.UpdateRoleAsync(model);
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
    [HttpDelete("RemoveRole")]
    public async Task<ActionResult<Response>> RemoveRoleAsync(Guid id)
    {
        try
        {
            _connections.con.BeginTransaction();
            await _roleService.RemoveRoleAsync(id);
            _connections.con.CommitTransactionAndDispose();
            return StatusCode((int) HttpStatusCode.OK,
                Util.BuildResponse("Removed Successfully",true));
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
}