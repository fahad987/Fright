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
public class ServiceTypeController: ControllerBase
{
    private readonly IServiceTypeService _serviceTypeService;
    private readonly IConnections _connections;

    public ServiceTypeController(IConnections connections, IServiceTypeService serviceTypeService)
    {
        _connections = connections;
        _serviceTypeService = serviceTypeService;
    }


    [HttpPost("GetAllServiceType")]
    public async Task<ActionResult<Response<List<ServiceType>>>> GetAllServiceTypeAsync(ServiceTypeFilter model)
    {
        try
        {
            var response = await _serviceTypeService.GetAllServiceTypeAsync(model);
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(response));
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
    [HttpPut("UpdateServiceTypeIsActiveStatus")]
    public async Task<ActionResult<Response<ServiceType>>> UpdateServiceTypeIsActiveStatusAsync(Guid id,bool isActive)
    {
        try
        {
            _connections.con.BeginTransaction();
            await _serviceTypeService.UpdateServiceTypeIsActiveStatusAsync(id,isActive);
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
    [HttpPost("AddServiceType")]
    public async Task<ActionResult<Response<ServiceType>>> AddServiceTypeAsync(ServiceType model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _serviceTypeService.AddServiceTypeAsync(model);
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
    [HttpPut("UpdateServiceType")]
    public async Task<ActionResult<Response<ServiceType>>> UpdateServiceTypeAsync(ServiceType model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _serviceTypeService.UpdateServiceTypeAsync(model);
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
    [HttpDelete("RemoveServiceType")]
    public async Task<ActionResult<Response>> RemoveServiceTypeAsync(Guid id)
    {
        try
        {
            _connections.con.BeginTransaction();
            await _serviceTypeService.RemoveServiceTypeAsync(id);
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