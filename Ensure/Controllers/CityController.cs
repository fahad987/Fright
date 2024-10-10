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
public class CityController: ControllerBase
{
    private readonly ICityService _cityService;
    private readonly IConnections _connections;

    public CityController(ICityService cityService, IConnections connections)
    {
        _cityService = cityService;
        _connections = connections;
    }

    [HttpPost("GetAllCity")]
    public async Task<ActionResult<Response<Country>>> GetAllCitAsync(CityFilter model)
    {
        try
        {
            var response = await _cityService.GetAllCityAsync(model);
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(response));
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
    [HttpPost("AddCity")]
    public async Task<ActionResult<Response<City>>> AddCityAsync(City model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _cityService.AddCityAsync(model);
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
    [HttpPut("UpdateCity")]
    public async Task<ActionResult<Response<City>>> UpdateCityAsync(City model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _cityService.UpdateCityAsync(model);
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
    [HttpPut("UpdateCityIsActiveStatus")]
    public async Task<ActionResult<Response>> UpdateCityIsActiveStatusAsync(Guid id,bool isActive)
    {
        try
        {
            _connections.con.BeginTransaction();
            await _cityService.UpdateCityIsActiveStatusAsync(id,isActive);
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