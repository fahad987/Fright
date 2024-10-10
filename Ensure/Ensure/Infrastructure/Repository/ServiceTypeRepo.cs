using System.Data;
using Dapper;
using Ensure.Application.IRepository;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Ensure.Entities.Enum;
using EnsureFreightInc.Entities.Domain;

namespace Ensure.Infrastructure.Repository;

public class ServiceTypeRepo : IServiceTypeRepo
{
    private readonly IConnections _connection;
    private readonly ActiveSession _activeSession;

    public ServiceTypeRepo(IConnections connection, ActiveSession activeSession)
    {
        _connection = connection;
        _activeSession = activeSession;
    }

    public async Task<ServiceType> AddServiceTypeAsync(ServiceType model)
    {
        if (await ExistsAsync(model))
            throw new Exception("Service type already exists");
        var parameters = new DynamicParameters();
        parameters.Add("@rate", model.rate);
        parameters.Add("@name", model.name);
        parameters.Add("@createBy", _activeSession.userId);
        parameters.Add("@createDate", DateTime.UtcNow);
        return await _connection.con.QueryAsync<ServiceType>("[dbo].[ServiceTypeAdd]", parameters);
    }

    public async Task<ServiceType> UpdateServiceTypeAsync(ServiceType model)
    {
        if (await ExistsAsync(model))
            throw new Exception("Service type already exists");
        var parameters = new DynamicParameters();
        parameters.Add("@id", model.id);
        parameters.Add("@name", model.name);
        parameters.Add("@rate", model.rate);
        return await _connection.con.QueryAsync<ServiceType>("[dbo].[ServiceTypeUpdate]", parameters);
    }

    public async Task<List<ServiceType>> GetAllServiceTypeAsync(ServiceTypeFilter model)
    {
        var parameters = new DynamicParameters();
        var query = "SELECT * from [ServiceType] where ";
        if (!string.IsNullOrEmpty(model.search))
        {
            parameters.Add("@search", $"%{model.search}%");
            query += "([name] like @search or [rate] like @search) and ";
        }

        if (model.isActive != IsActiveEnum.Both)
        {
            parameters.Add("@isActive",model.isActive);
            query += "(isActive=@isActive) and ";
        }
        query += " [name] IS NOT NULL order by [name] ";
        if (model.pageNo > 0)
            query += Util.DBPaging(model.pageSize, model.pageNo);
        return (await _connection.con
            .QueryListWithOutTransactionAsync<ServiceType>
                (query, parameters, CommandType.Text)).ToList();
    }
    /*public async Task<List<ServiceType>> GetAllServiceTypeAsync()
        => (await _connection.con.QueryListWithOutTransactionAsync<ServiceType>
            ("[dbo].[ServiceTypeGetAll]")).ToList();*/

    public async Task<bool> RemoveServiceTypeAsync(Guid id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@id",id);
        return await _connection.con.ExecuteAsync("[dbo].[ServiceTypeRemove]", parameters);
    }
    public async Task UpdateServiceTypeIsActiveStatusAsync(Guid serviceTypeId,bool isActive)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@serviceTypeId",serviceTypeId);
        parameters.Add("@isActive",isActive);
        await _connection.con.ExecuteAsync("[dbo].[ServiceTypeIsActiveStatusUpdate]", parameters);
    }
    private async Task<bool> ExistsAsync(ServiceType model)
    {
        var parameters = new DynamicParameters();
        var query = "SELECT count(*) from [ServiceType] where ";
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
}