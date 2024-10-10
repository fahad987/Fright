using System.Net;
using Ensure.Application.IService;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Ensure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AirportController: ControllerBase
{
    private readonly IAirportService _airportService;
    private readonly IConnections _connections;


    public AirportController(IAirportService airportService, IConnections connections)
    {
        _airportService = airportService;
        _connections = connections;
    }

    [HttpPost("GetAllAirport")]
    public async Task<ActionResult<Response<Airport>>> GetAllAirportAsync(AirportFilter model)
    {
        try
        {
            var response = await _airportService.GetAllAirportAsync(model);
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(response));
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
    [HttpPost("AddAirport")]
    public async Task<ActionResult<Response<Airport>>> AddAirportAsync(Airport model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _airportService.AddAirportAsync(model);
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
    [HttpPut("UpdateAirport")]
    public async Task<ActionResult<Response<Airport>>> UpdateAirportAsync(Airport model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _airportService.UpdateAirportAsync(model);
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
    [HttpPut("UpdateAirportIsActiveStatus")]
    public async Task<ActionResult<Response>> UpdateAirportIsActiveStatusAsync(Guid id,bool isActive)
    {
        try
        {
            _connections.con.BeginTransaction();
            await _airportService.UpdateAirportIsActiveStatusAsync(id,isActive);
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