using System.Data;
using Dapper;
using Ensure.Application.IHelper;
using Ensure.Application.IRepository;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Ensure.Entities.Enum;

namespace Ensure.Infrastructure.Repository;

public class AirlineRepo : IAirlineRepo
{
    private readonly IConnections _connections;
    private readonly ActiveSession _activeSession;
    private readonly IUploadHelper _uploadHelper;

    public AirlineRepo(IConnections connections, ActiveSession activeSession,
        IUploadHelper uploadHelper)
    {
        _connections = connections;
        _activeSession = activeSession;
        _uploadHelper = uploadHelper;
    }

    public async Task<Airline> AddAirlineAsync(Airline model)
    {
        if (await ExistsAsync(model.name,model.id))
            throw new Exception("Airline already exists");
        var parameters = new DynamicParameters();
        parameters.Add("@imageId",await _uploadHelper.GetUploadIdAsync(model.imageFile,model.imageId));
        parameters.Add("@name",model.name);
        parameters.Add("@contactNumber",model.contactNumber);
        parameters.Add("@email",model.email);
        parameters.Add("@code",model.code);
        parameters.Add("@prefixCode",model.prefixCode);
        parameters.Add("@tariffType",model.tariffType);
        parameters.Add("@fuel",model.fuel);
        parameters.Add("@security",model.security);
        parameters.Add("@civilAviation",model.civilAviation);
        parameters.Add("@createBy",_activeSession.userId);
        parameters.Add("@createDate",DateTime.UtcNow);
        var result = await _connections.con.QueryAsync<Airline>("[dbo].[AirlineAdd]", parameters);
        return result;
    }
    private async Task<bool> ExistsAsync(string name,Guid id)
    {
        var parameters = new DynamicParameters();
        var query = "SELECT count(*) from [Airline] where ";
        if (id != Guid.Empty)
        {
            parameters.Add("@id", id);
            query += " (id!=@id) and ";
        }
        parameters.Add("@name", name);
        query += " [name]=@name ";
        var result = await _connections.con
            .QueryWithOutTransactionAsync<int>
                (query, parameters, CommandType.Text);
        return result > 0;
    }
    
    public async Task<Airline> UpdateAirlineAsync(Airline model)
    {
        if (await ExistsAsync(model.name, model.id))
            throw new Exception("Airline already exists");
        var parameters = new DynamicParameters();
        parameters.Add("@id",model.id);
        parameters.Add("@imageId",await _uploadHelper.GetUploadIdAsync(model.imageFile,model.imageId));
        parameters.Add("@name",model.name);
        parameters.Add("@contactNumber",model.contactNumber);
        parameters.Add("@email",model.email);
        parameters.Add("@code",model.code);
        parameters.Add("@prefixCode",model.prefixCode);
        parameters.Add("@tariffType",model.tariffType);
        parameters.Add("@fuel",model.fuel);
        parameters.Add("@security",model.security);
        parameters.Add("@civilAviation",model.civilAviation);
        var result = await _connections.con
            .QueryAsync<Airline>("[dbo].[AirlineUpdate]", parameters);
       // result.surcharges = await _surchargeRepo.UpdateSurchargeAsync(model.surcharges,model.id);
        return result;
    }
    
    public async Task<List<Airline>> GetAllAirlineAsync(AirlineFilter model)
    {
        var parameters = new DynamicParameters();
        var query = "SELECT * from Airline where ";
        if (!string.IsNullOrEmpty(model.search))
        {
            parameters.Add("@search", $"%{model.search}%");
            query += "([name] like @search or [code] like @search) and ";
        }

        if (model.isActive != IsActiveAirlineEnum.Enable)
        {
            parameters.Add("@isActive",model.isActive);
            query += "(isActive=@isActive) and ";
        }
        
        query += " [name] IS NOT NULL order by [name] ";
        
        if (model.pageNo > 0)
            query += Util.DBPaging(model.pageSize, model.pageNo);

        var result = (await _connections.con.QueryListWithOutTransactionAsync<Airline>(query, parameters, CommandType.Text)).ToList();
        

        return result;
    }
    
    public async Task<Airline> GetAirlineByIdAsync(Guid airlineId)
    {
        var prams = new DynamicParameters();
        prams.Add("@airlineId", airlineId);
        var result= await _connections.con
            .QueryWithOutTransactionAsync<Airline>("[dbo].[AirlineGetById]", prams);
        
        return result;
    }
    
    public async Task UpdateAirlineIsActiveStatusAsync(Guid airlineId,bool isActive)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@airlineId",airlineId);
        parameters.Add("@isActive",isActive);
        await _connections.con.ExecuteAsync("[dbo].[AirlineIsActiveStatusUpdate]", parameters);
    }
    
}