using System.Data;
using Dapper;
using Ensure.Application.IRepository;
using Ensure.DbContext;
using Ensure.Entities.Domain;

namespace Ensure.Infrastructure.Repository;

public class ModuleRepo : IModuleRepo
{
    private readonly IConnections _connection;

    public ModuleRepo(IConnections connection)
    {
        _connection = connection;
    }

    /*public async Task<List<Module>> GetAllModuleAsync()
    {
        var parameters = new DynamicParameters();
        var query = "SELECT * from [Module] ";
        var result = await _connection.con
            .QueryListWithOutTransactionAsync<Module>
                (query, parameters, CommandType.Text);
        if (!result.Any())
            return new List<Module>();
        var permissions = await GetAllModulePermissionAsync(result.Select(x =>(int) x.id).ToList());
        foreach (var row in result)
        {
            row.permissions = permissions.Where(x => x.moduleId == row.id).ToList();
        }
        return result.ToList();
    }*/
    public async Task<List<ModulePermission>> GetAllModulePermissionAsync()
    {
        var parameters = new DynamicParameters();
        parameters.Add("@moduleIds");
        var query = $"SELECT * from [vwModule]";
        var result = await _connection.con
            .QueryListWithOutTransactionAsync<ModulePermission>
                (query, parameters, CommandType.Text);
        return result.ToList();
    }
}