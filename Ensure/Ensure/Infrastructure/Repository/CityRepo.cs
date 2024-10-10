using System.Data;
using Dapper;
using Ensure.Application.IRepository;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Ensure.Entities.Enum;

namespace Ensure.Infrastructure.Repository;

public class CityRepo :ICityRepo
{
    private readonly IConnections _connection;

    public CityRepo(IConnections connection)
    {
        _connection = connection;
    }


    public async Task<List<City>> GetAllCityAsync(CityFilter model)
    {
        var parameters = new DynamicParameters();
        var query = "SELECT * from vwCity where ";
        if (!string.IsNullOrEmpty(model.search))
        {
            parameters.Add("@search", $"%{model.search}%");
            query += "(name like @search or code like @search or country like @search) and ";
        }
        if (model.countryId != Guid.Empty)
        {
            parameters.Add("@countryId", model.countryId);
            query += "(countryId=@countryId) and ";
        }
        if (model.isActive != IsActiveEnum.Both)
        {
            parameters.Add("@isActive",model.isActive);
            query += "(isActive=@isActive) and ";
        }
        query += " [name] IS NOT NULL order by [name]";
        if (model.pageNo > 0)
            query += Util.DBPaging(model.pageSize, model.pageNo);
        return (await _connection.con
            .QueryListWithOutTransactionAsync<City>(query, parameters,CommandType.Text))
            .ToList();

    }
    private async Task<bool> ExistsAsync(string name,Guid cityId)
    {
        var parameters = new DynamicParameters();
        var query = "SELECT count(*) from [City] where ";
        if (cityId != Guid.Empty)
        {
            parameters.Add("@cityId", cityId);
            query += " (id!=@cityId) and ";
        }
        parameters.Add("@name", name);
        query += " [name]=@name ";
        var result = await _connection.con
            .QueryWithOutTransactionAsync<int>
                (query, parameters, CommandType.Text);
        return result > 0;
    }
    public async Task<City> AddCityAsync(City model)
    {
        if (await ExistsAsync(model.name, Guid.Empty))
            throw new Exception("City already exists");
        var parameters = new DynamicParameters();
        parameters.Add("@countryId",model.countryId);
        parameters.Add("@name",model.name);
        parameters.Add("@code",model.code);
        return await _connection.con
            .QueryAsync<City>("[dbo].[CityAdd]", parameters);
    }
    public async Task<City> UpdateCityAsync(City model)
    {
        if (await ExistsAsync(model.name, model.id))
            throw new Exception("City already exists");
        var parameters = new DynamicParameters();
        parameters.Add("@id",model.id);
        parameters.Add("@countryId",model.countryId);
        parameters.Add("@name",model.name);
        parameters.Add("@code",model.code);
        return await _connection.con
            .QueryAsync<City>("[dbo].[CityUpdate]", parameters);
    }
    public async Task UpdateCityIsActiveStatusAsync(Guid cityId,bool isActive)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@cityId",cityId);
        parameters.Add("@isActive",isActive);
        await _connection.con.ExecuteAsync("[dbo].[CityIsActiveStatusUpdate]", parameters);
    }
}