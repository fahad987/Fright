using System.Data;
using Dapper;
using Ensure.Application.IRepository;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;

namespace Ensure.Infrastructure.Repository;

public class BranchRepo : IBranchRepo
{
    private readonly IConnections _connection;
    private readonly ActiveSession _activeSession;
    
    public BranchRepo(IConnections connection, ActiveSession activeSession)
    {
        _connection = connection;
        _activeSession = activeSession;
    }
    public async Task<bool> ExistsAsync(Branch model)
    {
        var parameters = new DynamicParameters();
        var query = "SELECT count(*) from Branch where ";
        if (model.id != Guid.Empty)
        {
            parameters.Add("@id", model.id);
            query += " (id!=@id) and ";
        }

        parameters.Add("@name", model.name);
        query += " [name]=@name and isActive=1 ";
        var result = await _connection.con.QueryWithOutTransactionAsync<int>(query, parameters, CommandType.Text);
        return result > 0;
    }

    public async Task<Branch> AddBranchAsync(Branch model)
    {
        if (await ExistsAsync(model))
            throw new Exception("Branch already exists");
        var parameters = new DynamicParameters();
        parameters.Add("@id", Guid.NewGuid());
        parameters.Add("@name", model.name);
        parameters.Add("@code", model.code);
        parameters.Add("@address", model.address);
        parameters.Add("@cityId",model.cityId);
        parameters.Add("@createBy",_activeSession.userId);
        parameters.Add("@createDate", DateTime.UtcNow);
        return await _connection.con.QueryAsync<Branch>("[dbo].[BranchAdd]", parameters);
    }

    public async Task<Branch> UpdateBranchAsync(Branch model)
    {
        if (await ExistsAsync(model))
            throw new Exception("Branch already exists");
        var parameters = new DynamicParameters();
        parameters.Add("@id", model.id);
        parameters.Add("@name", model.name);
        parameters.Add("@code", model.code);
        parameters.Add("@address", model.address);
        parameters.Add("@cityId",model.cityId);
        return await _connection.con.QueryAsync<Branch>("[dbo].[BranchUpdate]", parameters);
    }

    public async Task<List<Branch>> GetAllBranchAsync()
    {
        return (await _connection.con
            .QueryListWithOutTransactionAsync<Branch>("[dbo].[BranchGetAll]")).ToList();
    }

    public async Task RemoveBranchAsync(Guid id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@id",id);
        await _connection.con.ExecuteAsync("[dbo].[BranchRemove]", parameters);
    }


   
}