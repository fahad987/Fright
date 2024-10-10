using System.Net;
using Ensure.Application.IService;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Ensure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourierCompanyController : ControllerBase
{
    private readonly IConnections _connections;
    private readonly ICourierCompanyService _courierCompanyService;

    public CourierCompanyController(IConnections connections, ICourierCompanyService courierCompanyService)
    {
        _connections = connections;
        _courierCompanyService = courierCompanyService;
    }
    
    [HttpPost("AddCourierCompany")]
    public async Task<ActionResult<Response<CourierCompany>>> AddCourierCompanyAsync([FromForm] CourierCompany model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _courierCompanyService.AddCourierCompanyAsync(model);
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
    
    [HttpPut("UpdateCourierCompany")]
    public async Task<ActionResult<Response<CourierCompany>>> UpdateCourierCompanyAsync([FromForm] CourierCompany model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _courierCompanyService.UpdateCourierCompanyAsync(model);
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
    
    [HttpPost("GetAllCourierCompany")]
    public async Task<ActionResult<Response<CourierCompany>>> GetAllCourierCompanyAsync(CourierCompanyFilter model)
    {
        try
        {
            var response = await _courierCompanyService.GetAllCourierCompanyAsync(model);
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(response));
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
    
    [HttpGet("GetCourierCompanyById")]
    public async Task<ActionResult<Response>> GetCourierCompanyByIdAsync(Guid id)
    {
        try
        {
            var result = await _courierCompanyService.GetCourierCompanyByIdAsync(id);
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(result));
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
    
    [HttpPut("UpdateCourierCompanyIsActiveStatus")]
    public async Task<ActionResult<Response>> UpdateCourierCompanyIsActiveStatusAsync(Guid id,bool isActive)
    {
        try
        {
            _connections.con.BeginTransaction();
            await _courierCompanyService.UpdateCourierCompanyIsActiveStatusAsync(id, isActive);
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