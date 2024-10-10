using System.Net;
using Ensure.Application.IService;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Ensure.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModuleController: ControllerBase
{
    private readonly IModuleService _moduleService;
    private readonly IConnections _connections;


    public ModuleController(IModuleService moduleService, IConnections connections)
    {
        _moduleService = moduleService;
        _connections = connections;
    }

    [HttpGet("GetAllModulePermission")]
    public async Task<ActionResult<Response<Module>>> GetAllModulePermissionAsync()
    {
        try
        {
            var response = await _moduleService.GetAllModulePermissionAsync();
            return StatusCode((int) HttpStatusCode.OK,Util.BuildResponse(response));
        }
        catch (Exception e)
        {
            return StatusCode((int) HttpStatusCode.BadRequest,
                Util.BuildResponse(e.Message,false));
        }
    }
   
}