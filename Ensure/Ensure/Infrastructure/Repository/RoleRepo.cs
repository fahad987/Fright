using System.Data;
using Dapper;
using Ensure.Application.IRepository;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Ensure.Entities.Enum;

namespace Ensure.Infrastructure.Repository;

public class RoleRepo : IRoleRepo
{
    private readonly IConnections _connection;
    private readonly ActiveSession _activeSession;

    public RoleRepo(IConnections connection, ActiveSession activeSession)
    {
        _connection = connection;
        _activeSession = activeSession;
    }

    public async Task<Role> AddRoleAsync(Role model)
    {
        if (await ExistsAsync(model))
            throw new Exception("Role already exists");
        var parameters = new DynamicParameters();
        parameters.Add("@id", Guid.NewGuid());
        parameters.Add("@name", model.name);
        parameters.Add("@createBy", _activeSession.userId);
        parameters.Add("@createDate", DateTime.UtcNow);
        var result= await _connection.con.QueryAsync<Role>("[dbo].[RoleAdd]", parameters);
        model.id = result.id;
        result.permissions = await AddRolePermissionAsync(model);
        return result;
    }
    private async Task<List<RolePermission>> AddRolePermissionAsync(Role model)
    {
        if (!model.permissions.Any()) return new List<RolePermission>();
        var parameters = new DynamicParameters();
        parameters.Add("@roleId",model.id);
        parameters.Add("@permissions",Util.GetString(model.permissions.Select(x=>(byte)x.permissionId)));
        return (await _connection.con
                .QueryListAsync<RolePermission>("[dbo].[RolePermissionAdd]", parameters))
            .ToList();
    }
    public async Task<Role> UpdateRoleAsync(Role model)
    {
        if (await ExistsAsync(model))
            throw new Exception("Role already exists");
        var parameters = new DynamicParameters();
        parameters.Add("@id", model.id);
        parameters.Add("@name", model.name);
        parameters.Add("@updateBy", _activeSession.userId);
        parameters.Add("@updateDate", DateTime.UtcNow);
        var result= await _connection.con.QueryAsync<Role>("[dbo].[RoleUpdate]", parameters);
        await RemoveRolePermissionAsync(model.id);
        result.permissions = await AddRolePermissionAsync(model);
        return result;
    }
    private async Task RemoveRolePermissionAsync(Guid roleId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@roleId", roleId);
        await _connection.con.ExecuteAsync("[dbo].[RolePermissionRemove]", parameters);
    }
    public async Task<List<Role>> GetAllRoleAsync(RoleFilter model)
    {
        var parameters = new DynamicParameters();
        var query = "SELECT * from [Role] where ";
        if (!string.IsNullOrEmpty(model.search))
        {
            parameters.Add("@search", $"%{model.search}%");
            query += "([name] like @search) and ";
        }
        if (model.isActive != IsActiveEnum.Both)
        {
            parameters.Add("@isActive",model.isActive);
            query += "(isActive=@isActive) and ";
        }
        query += " [name] IS NOT NULL order by [name] ";
        if (model.pageNo > 0)
            query += Util.DBPaging(model.pageSize, model.pageNo);
        var result= await _connection.con
            .QueryListWithOutTransactionAsync<Role>
                (query, parameters, CommandType.Text);
        /*var result = await _connection.con.QueryListWithOutTransactionAsync<Role>("[dbo].[RoleGetAll]");*/
        if (!result.Any())
            return new List<Role>();
        var permissions = await GetAllRolePermissionAsync(result.Select(x => x.id).ToList());
        foreach (var row in result)
        {
            row.permissions = permissions.Where(x => x.roleId == row.id).ToList();
        }

        return result.ToList();

    }
    public async Task<bool> RemoveRoleAsync(Guid id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@id",id);
        return await _connection.con.ExecuteAsync("[dbo].[RoleRemove]", parameters);
    }
    public async Task UpdateRoleIsActiveStatusAsync(Guid roleId,bool isActive)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@roleId",roleId);
        parameters.Add("@isActive",isActive);
        await _connection.con.ExecuteAsync("[dbo].[RoleIsActiveStatusUpdate]", parameters);
    }
    private async Task<bool> ExistsAsync(Role model)
    {
        var parameters = new DynamicParameters();
        var query = "SELECT count(*) from [Role] where ";
        if (model.id != Guid.Empty)
        {
            parameters.Add("@id", model.id);
            query += " (id!=@id) and ";
        }
        parameters.Add("@name", model.name);
        query += " [name]=@name  and isActive=1 ";
        var result = await _connection.con
            .QueryWithOutTransactionAsync<int>
                (query, parameters, CommandType.Text);
        return result > 0;
    }
    private async Task<List<RolePermission>> GetAllRolePermissionAsync(List<Guid> roleIds)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@roleIds",Util.GetString(roleIds));
        var result = await _connection.con
            .QueryListWithOutTransactionAsync<RolePermission>("[dbo].[RolePermissionGetAll]", parameters);
        return result.ToList();
    }
    
}