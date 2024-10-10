using System.Data;
using Dapper;
using Ensure.Application.IHelper;
using Ensure.Application.IRepository;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Ensure.Entities.Enum;

namespace Ensure.Infrastructure.Repository;

public class CourierCompanyRepo : ICourierCompanyRepo
{
    private readonly IConnections _connections;
    private readonly ActiveSession _activeSession;
    private readonly IUploadHelper _uploadHelper;
    private readonly ISurchargeRepo _surchargeRepo;

    public CourierCompanyRepo(IConnections connections, IUploadHelper uploadHelper, ActiveSession activeSession, ISurchargeRepo surchargeRepo)
    {
        _connections = connections;
        _uploadHelper = uploadHelper;
        _activeSession = activeSession;
        _surchargeRepo = surchargeRepo;
    }

    public async Task<CourierCompany> AddCourierCompanyAsync(CourierCompany model)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@imageId",await _uploadHelper.GetUploadIdAsync(model.imageFile,model.imageId));
        parameters.Add("@name",model.name);
        parameters.Add("@contactNumber",model.contactNumber);
        parameters.Add("@email",model.email);
        parameters.Add("@tariffType",model.tariffType);
        parameters.Add("@fuel",model.fuel);
        parameters.Add("@security",model.security);
        parameters.Add("@civilAviation",model.civilAviation);
        parameters.Add("@createBy",_activeSession.userId);
        parameters.Add("@createDate",DateTime.UtcNow);
        var result = await _connections.con.QueryAsync<CourierCompany>("[dbo].[CourierCompanyAdd]", parameters);
        result.surcharges = await _surchargeRepo.AddSurchargeAsync(model.surcharges, result.id);
        return result;

    }
    
    public async Task<CourierCompany> UpdateCourierCompanyAsync(CourierCompany model)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@id",model.id);
        parameters.Add("@imageId",await _uploadHelper.GetUploadIdAsync(model.imageFile,model.imageId));
        parameters.Add("@name",model.name);
        parameters.Add("@contactNumber",model.contactNumber);
        parameters.Add("@email",model.email);
        parameters.Add("@tariffType",model.tariffType);
        parameters.Add("@fuel",model.fuel);
        parameters.Add("@security",model.security);
        parameters.Add("@civilAviation",model.civilAviation);
        var result = await _connections.con
            .QueryAsync<CourierCompany>("[dbo].[CourierCompanyUpdate]", parameters);
        result.surcharges = await _surchargeRepo.UpdateSurchargeAsync(model.surcharges,model.id);
        return result;
    }
    
    public async Task<List<CourierCompany>> GetAllCourierCompanyAsync(CourierCompanyFilter model)
    {
        var parameters = new DynamicParameters();
        var query = "SELECT * from CourierCompany where ";
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
        var result = (await _connections.con.QueryListWithOutTransactionAsync<CourierCompany>(query, parameters, CommandType.Text)).ToList();
        var courier = await _surchargeRepo.GetAllSurchargeAsync(
            result.Select(x => x.id).ToList());
        foreach (var row in result)
        {
            row.surcharges = courier.Where(x => x.airlineId == row.id).ToList();
        }

        return result;
    }
    
    public async Task<CourierCompany> GetCourierCompanyByIdAsync(Guid courierCompanyId)
    {
        var prams = new DynamicParameters();
        prams.Add("@courierCompanyId", courierCompanyId);
        var result= await _connections.con
            .QueryWithOutTransactionAsync<CourierCompany>("[dbo].[CourierCompanyGetById]", prams);
        result.surcharges = await _surchargeRepo.GetAllSurchargeAsync(courierCompanyId);
        return result;
    }
    
    public async Task UpdateCourierCompanyIsActiveStatusAsync(Guid courierCompanyId,bool isActive)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@courierCompanyId",courierCompanyId);
        parameters.Add("@isActive",isActive);
        await _connections.con.ExecuteAsync("[dbo].[CourierCompanyIsActiveStatusUpdate]", parameters);
    }
}