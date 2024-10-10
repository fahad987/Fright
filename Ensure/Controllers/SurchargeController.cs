using System.Net;
using Ensure.Application.IService;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Ensure.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SurchargeController : ControllerBase
{
    private readonly IConnections _connections;
    private readonly ISurchargeService _surchargeService;
    
    public SurchargeController(IConnections connections, ISurchargeService surchargeService)
    {
        _connections = connections;
        _surchargeService = surchargeService;
    }
    
    [HttpPost("AddSurcharge")]
    public async Task<ActionResult<Response<Surcharge>>> AddSurchargeAsync( Surcharge model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _surchargeService.AddSurchargeAsync(model);
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