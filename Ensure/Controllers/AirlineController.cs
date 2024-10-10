using System.Net;
using Ensure.Application.IService;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Ensure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AirlineController : ControllerBase
{
    private readonly IConnections _connections;
    private readonly IAirlineService _airlineService;

    public AirlineController(IConnections connections, IAirlineService airlineService)
    {
        _connections = connections;
        _airlineService = airlineService;
    }
    
    [HttpPost("AddAirline")]
    public async Task<ActionResult<Response<Airline>>> AddAirlineAsync([FromForm] Airline model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _airlineService.AddAirlineAsync(model);
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
    
    [HttpPut("UpdateAirline")]
    public async Task<ActionResult<Response<Airline>>> UpdateAirlineAsync([FromForm] Airline model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _airlineService.UpdateAirlineAsync(model);
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
    
    [HttpPost("GetAllAirline")]
    public async Task<ActionResult<Response<Airline>>> GetAllAirlineAsync(AirlineFilter model)
    {
        try
        {
            var response = await _airlineService.GetAllAirlineAsync(model);
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(response));
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
    
    [HttpGet("GetAirlineById")]
    public async Task<ActionResult<Response>> GetAirlineByIdAsync(Guid id)
    {
        try
        {
            var result = await _airlineService.GetAirlineByIdAsync(id);
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(result));
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
    
    [HttpPut("UpdateAirlineIsActiveStatus")]
    public async Task<ActionResult<Response>> UpdateAirlineIsActiveStatusAsync(Guid id,bool isActive)
    {
        try
        {
            _connections.con.BeginTransaction();
            await _airlineService.UpdateAirlineIsActiveStatusAsync(id, isActive);
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