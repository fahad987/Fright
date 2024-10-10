using System.Data;
using Dapper;
using Ensure.Application.IRepository;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Ensure.Entities.Enum;

namespace Ensure.Infrastructure.Repository;

public class AirportRepo :IAirportRepo
{
    private readonly IConnections _connection;

    public AirportRepo(IConnections connection)
    {
        _connection = connection;
    }

    public async Task<List<Airport>> GetAllAirportAsync(AirportFilter model)
    {
        var parameters = new DynamicParameters();
        var query = "SELECT * from vwAirport where ";
        if (!string.IsNullOrEmpty(model.search))
        {
            parameters.Add("@search", $"%{model.search}%");
            query += "(name like @search or code like @search or country like @search or country like @search or countryCode like @search or cityCode like @search) and ";
        }
        if (model.countries.Any())
            query += $" (countryId IN ({Util.GetStringSplit(model.countries)})) and ";
        if (model.cities.Any())
            query += $" (cityId IN ({Util.GetStringSplit(model.cities)})) and ";
        if (model.isActive != IsActiveEnum.Both)
        {
            parameters.Add("@isActive",model.isActive);
            query += "(isActive=@isActive) and ";
        }
        query +=" [name] IS NOT NULL order by [name]";
        return (await _connection.con
            .QueryListWithOutTransactionAsync<Airport>(query,parameters,CommandType.Text)).ToList();
    }
    private async Task<bool> ExistsAsync(string name,Guid airportId)
    {
        var parameters = new DynamicParameters();
        var query = "SELECT count(*) from [Airport] where ";
        if (airportId != Guid.Empty)
        {
            parameters.Add("@airportId", airportId);
            query += " (id!=@airportId) and ";
        }
        parameters.Add("@name", name);
        query += " [name]=@name ";
        var result = await _connection.con
            .QueryWithOutTransactionAsync<int>
                (query, parameters, CommandType.Text);
        return result > 0;
    }
    public async Task<Airport> AddAirportAsync(Airport model)
    {
        if (await ExistsAsync(model.name, Guid.Empty))
            throw new Exception("Airport already exists");
        var parameters = new DynamicParameters();
        parameters.Add("@cityId",model.cityId);
        parameters.Add("@name",model.name);
        parameters.Add("@code",model.code);
        return await _connection.con
            .QueryAsync<Airport>("[dbo].[AirportAdd]", parameters);
    }
    public async Task<Airport> UpdateAirportAsync(Airport model)
    {
        if (await ExistsAsync(model.name, model.id))
            throw new Exception("Airport already exists");
        var parameters = new DynamicParameters();
        parameters.Add("@id",model.id);
        parameters.Add("@cityId",model.cityId);
        parameters.Add("@name",model.name);
        parameters.Add("@code",model.code);
        return await _connection.con
            .QueryAsync<Airport>("[dbo].[AirportUpdate]", parameters);
    }
    public async Task UpdateAirportIsActiveStatusAsync(Guid airportId,bool isActive)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@airportId",airportId);
        parameters.Add("@isActive",isActive);
        await _connection.con.ExecuteAsync("[dbo].[AirportIsActiveStatusUpdate]", parameters);
    }
}