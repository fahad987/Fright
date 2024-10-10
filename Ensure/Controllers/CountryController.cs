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
public class CountryController: ControllerBase
{
    private readonly ICountryService _countryService;
    private readonly IConnections _connections;
    private readonly ILogger<CountryController> _logger;
    public CountryController(ICountryService countryService, IConnections connections, ILogger<CountryController> logger)
    {
        _countryService = countryService;
        _connections = connections;
        _logger = logger;
    }
    [HttpPost("GetAllCountry")]
    public async Task<ActionResult<Response<List<Country>>>> GetAllCountryAsync(CountryFilter model)
    {
        try
        {
            var response = await _countryService.GetAllCountryAsync(model);
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(response));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Message}", e.Message);
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
    [HttpPost("AddCountry")]
    public async Task<ActionResult<Response<Country>>> AddCountryAsync(Country model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _countryService.AddCountryAsync(model);
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
    [HttpPut("UpdateCountry")]
    public async Task<ActionResult<Response<Country>>> UpdateCountryAsync(Country model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _countryService.UpdateCountryAsync(model);
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
    [HttpPut("UpdateCountryIsActiveStatus")]
    public async Task<ActionResult<Response>> UpdateCountryIsActiveStatusAsync(Guid id,bool isActive)
    {
        try
        {
            _connections.con.BeginTransaction();
            await _countryService.UpdateCountryIsActiveStatusAsync(id,isActive);
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