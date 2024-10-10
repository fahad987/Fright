using Dapper;
using Ensure.Application.IRepository;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;

namespace Ensure.Infrastructure.Repository;

public class SurchargeRepo : ISurchargeRepo
{
    private readonly IConnections _connections;

    public SurchargeRepo(IConnections connections)
    {
        _connections = connections;
    }

    public async Task<(bool, string)> ValidateSurchargeModel(List<Surcharge> model)
    {
        var _list = new List<Surcharge>();
        foreach (var row in model)
        {
            if (_list.Any(x => x.name.ToLower() == row.name.ToLower() && x.amount == row.amount))
                return  (false, "Surcharge name and amount already exists");
            _list.Add(row);
        }

        return  (true, "");

    }
    public async Task<Surcharge> AddSurchargeAsync(Surcharge model)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@id",Guid.NewGuid());
        parameters.Add("@airlineId",model.airlineId);
        parameters.Add("@title",model.name);
        parameters.Add("@amount",model.amount);
        return await _connections.con.QueryAsync<Surcharge>("[dbo].[SurchargeAdd]", parameters);
    }
    
    public async Task<List<Surcharge>> AddSurchargeAsync(List<Surcharge> model, Guid airlineId)
    {
        var list = new List<Surcharge>();
        foreach (var row in model)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id",Guid.NewGuid());
            parameters.Add("@name",row.name);
            parameters.Add("@amount",row.amount);
            parameters.Add("@airlineId",airlineId);
            var result =  await _connections.con.QueryAsync<Surcharge>("[dbo].[SurchargeAdd]", parameters);
            list.Add(result);
        }
    
        return list;
    }

    public async Task<List<Surcharge>> UpdateSurchargeAsync(List<Surcharge> model,Guid airlineId)
    {
        var list = new List<Surcharge>();
        foreach (var row in model)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@airlineId", airlineId);
            parameters.Add("@name", row.name);
            parameters.Add("@amount", row.amount);
            var result = await _connections.con
                .QueryAsync<Surcharge>("[dbo].[SurchargeUpdate]", parameters);
            list.Add(result);

        }

        return list;
    }

    public async Task<List<Surcharge>> GetAllSurchargeAsync(List<Guid> airlineIds)
    {
        if (!airlineIds.Any()) return new();
        var parameters = new DynamicParameters();
        parameters.Add("@airlineIds",Util.GetString(airlineIds));
        var result= await _connections.con
            .QueryListWithOutTransactionAsync<Surcharge>("[dbo].[SurchargeGetAll]",
                parameters);
        return result.ToList();
    }

    public async Task<List<Surcharge>> GetAllSurchargeAsync(Guid airlineId)
        => await GetAllSurchargeAsync(new List<Guid>() {airlineId});
    
}