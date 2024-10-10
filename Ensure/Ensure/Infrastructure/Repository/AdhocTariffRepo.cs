using System.Data;
using Dapper;
using Ensure.Application.IRepository;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Ensure.Entities.Enum;

namespace Ensure.Infrastructure.Repository;

public class AdhocTariffRepo : IAdhocTariffRepo
{
    private readonly IConnections _connections;
    private readonly ActiveSession _activeSession;

    public AdhocTariffRepo(IConnections connections, ActiveSession activeSession)
    {
        _connections = connections;
        _activeSession = activeSession;
    }

    public async Task<AdhocTariff> AddAdhocTariffAsync(AdhocTariff model)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@originId",model.originId);
        parameters.Add("@destinationId",model.destinationId);
        parameters.Add("@min",model.min);
        parameters.Add("@n",model.n);
        parameters.Add("@w450000",model.w450000);
        parameters.Add("@w100000",model.w100000);
        parameters.Add("@w200000",model.w200000);
        parameters.Add("@w300000",model.w300000);
        parameters.Add("@w500000",model.w500000);
        parameters.Add("@w1000000",model.w1000000);
        parameters.Add("@w500Document",model.w500Document);
        parameters.Add("@w100Document",model.w100Document);
        parameters.Add("@w1000",model.w1000);
        parameters.Add("@w2000",model.w2000);
        parameters.Add("@w3000",model.w3000);
        parameters.Add("@w4000",model.w4000);
        parameters.Add("@w50000",model.w50000);
        parameters.Add("@w6000",model.w6000);
        parameters.Add("@w7000",model.w7000);
        parameters.Add("@w8000",model.w8000);
        parameters.Add("@w9000",model.w9000);
        parameters.Add("@w15000",model.w15000);
        parameters.Add("@w2000",model.w2000);
        parameters.Add("@w20000",model.w20000);
        parameters.Add("@w25000",model.w25000);
        parameters.Add("@w30000",model.w30000);
        parameters.Add("@w35000",model.w35000);
        parameters.Add("@w40000",model.w40000);
        parameters.Add("@createBy",_activeSession.userId);
        parameters.Add("@createDate",DateTime.UtcNow);
        return await _connections.con.QueryAsync<AdhocTariff>("[dbo].[AdhocTariffAdd]", parameters);
        
    }

    public async Task<AdhocTariff> UpdateAdhocTariffAsync(AdhocTariff model)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@id",model.id);
        parameters.Add("@originId",model.originId);
        parameters.Add("@destinationId",model.destinationId);
        parameters.Add("@min",model.min);
        parameters.Add("@n",model.n);
        parameters.Add("@w450000",model.w450000);
        parameters.Add("@w100000",model.w100000);
        parameters.Add("@w200000",model.w200000);
        parameters.Add("@w300000",model.w300000);
        parameters.Add("@w500000",model.w500000);
        parameters.Add("@w1000000",model.w1000000);
        parameters.Add("@w500Document",model.w500Document);
        parameters.Add("@w100Document",model.w100Document);
        parameters.Add("@w1000",model.w1000);
        parameters.Add("@w2000",model.w2000);
        parameters.Add("@w3000",model.w3000);
        parameters.Add("@w4000",model.w4000);
        parameters.Add("@w50000",model.w50000);
        parameters.Add("@w6000",model.w6000);
        parameters.Add("@w7000",model.w7000);
        parameters.Add("@w8000",model.w8000);
        parameters.Add("@w9000",model.w9000);
        parameters.Add("@w15000",model.w15000);
        parameters.Add("@w2000",model.w2000);
        parameters.Add("@w20000",model.w20000);
        parameters.Add("@w25000",model.w25000);
        parameters.Add("@w30000",model.w30000);
        parameters.Add("@w35000",model.w35000);
        parameters.Add("@w40000",model.w40000);
        return await _connections.con.QueryAsync<AdhocTariff>("[dbo].[AdhocTariffUpdate]", parameters);
    }

    public async Task<List<AdhocTariff>> GetAllAdhocTariffAsync(AdhocTariffFilter model)
    {
        var parameters = new DynamicParameters();
        var query = "SELECT * from vwAdhocTariff where ";
        if (!string.IsNullOrEmpty(model.search))
        {
            parameters.Add("@search", $"%{model.search}%");
            query += "(originId like @search or destinationId like @search ) and ";
        }
        if (model.origins.Any())
            query += $" (originId IN ({Util.GetStringSplit(model.origins)})) and ";
        if (model.destination.Any())
            query += $" (destinationId IN ({Util.GetStringSplit(model.destination)})) and ";
        if (model.isActive != IsActiveEnum.Both)
        {
            parameters.Add("@isActive",model.isActive);
            query += "(isActive=@isActive) ";
        }

        return (await _connections.con
            .QueryListWithOutTransactionAsync<AdhocTariff>(query,parameters,CommandType.Text)).ToList();
    }

    public async Task<AdhocTariff> GetAdhocTariffByIdAsync(Guid adhocId)
    {
        var prams = new DynamicParameters();
        prams.Add("@adhocId", adhocId);
        var result= await _connections.con
            .QueryWithOutTransactionAsync<AdhocTariff>("[dbo].[AdhocTariffGetById]", prams);
      
        return result;
    }
    
    public async Task UpdateAdhocTariffIsActiveStatusAsync(Guid adhocId,bool isActive)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@adhocId",adhocId);
        parameters.Add("@isActive",isActive);
        await _connections.con.ExecuteAsync("[dbo].[AdhocTariffIsActiveStatusUpdate]", parameters);
    }
}