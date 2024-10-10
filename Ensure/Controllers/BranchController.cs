using System.Net;
using Ensure.Application.IService;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Ensure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BranchController: ControllerBase
{
    private readonly IBranchService _branchService;
    private readonly IConnections _connections;

    public BranchController(IBranchService branchService, IConnections connections)
    {
        _branchService = branchService;
        _connections = connections;
    }

    [HttpGet("GetAllBranch")]
    public async Task<ActionResult<Response<List<Branch>>>> GetAllBranchAsync()
    {
        try
        {
            var response = await _branchService.GetAllBranchAsync();
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(response));
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
    [HttpPost("AddBranch")]
    public async Task<ActionResult<Response<Branch>>> AddBranchAsync(Branch model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _branchService.AddBranchAsync(model);
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
    [HttpPut("UpdateBranch")]
    public async Task<ActionResult<Response<Branch>>> UpdateBranchAsync(Branch model)
    {
        try
        {
            _connections.con.BeginTransaction();
            var response = await _branchService.UpdateBranchAsync(model);
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
    [HttpDelete("RemoveBranch")]
    public async Task<ActionResult<Response>> RemoveBranchAsync(Guid id)
    {
        try
        {
            _connections.con.BeginTransaction();
            await _branchService.RemoveBranchAsync(id);
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