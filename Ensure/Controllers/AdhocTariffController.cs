using System.Net;
using Ensure.Application.IService;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Ensure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdhocTariffController : ControllerBase
{
    private readonly IConnections _connections;
    private readonly IAdhocTariffService _adhocTariffService;

    public AdhocTariffController(IConnections connections, IAdhocTariffService adhocTariffService)
    {
        _connections = connections;
        _adhocTariffService = adhocTariffService;
    }
    
    [HttpPost("AddAdhocTariff")]
    public async Task<ActionResult<Response<AdhocTariff>>> AddAdhocTariffAsync(AdhocTariff model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _adhocTariffService.AddAdhocTariffAsync(model);
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
    
    [HttpPut("UpdateAdhocTariff")]
    public async Task<ActionResult<Response<AdhocTariff>>> UpdateAdhocTariffAsync(AdhocTariff model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _adhocTariffService.UpdateAdhocTariffAsync(model);
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
    
    [HttpPost("GetAllAdhocTariff")]
    public async Task<ActionResult<Response<AdhocTariff>>> GetAllAdhocTariffAsync(AdhocTariffFilter model)
    {
        try
        {
            var response = await _adhocTariffService.GetAllAdhocTariffAsync(model);
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(response));
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
    
    [HttpGet("GetAdhocTariffById")]
    public async Task<ActionResult<Response>> GetAdhocTariffByIdAsync(Guid id)
    {
        try
        {
            var result = await _adhocTariffService.GetAdhocTariffByIdAsync(id);
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(result));
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
    
    [HttpPut("UpdateAdhocTariffIsActiveStatus")]
    public async Task<ActionResult<Response>> UpdateAdhocTariffIsActiveStatusAsync(Guid id,bool isActive)
    {
        try
        {
            _connections.con.BeginTransaction();
            await _adhocTariffService.UpdateAdhocTariffIsActiveStatusAsync(id, isActive);
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
}