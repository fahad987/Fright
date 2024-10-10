using System.Data;
using Dapper;
using Ensure.Application.IRepository;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Ensure.Entities.Enum;
using EnsureFreightInc.Entities.Domain;

namespace Ensure.Infrastructure.Repository;

public class CountryRepo : ICountryRepo
{
    private readonly IConnections _connection;

    public CountryRepo(IConnections connection)
    {   
        _connection = connection;
    }

    public async Task<List<Country>> GetAllCountryAsync(CountryFilter model)
    {
        var parameters = new DynamicParameters();
        var query = "SELECT * from [Country] where ";
        if (!string.IsNullOrEmpty(model.search))
        {
            parameters.Add("@search", $"%{model.search}%");
            query += "([name] like @search or [code] like @search) and ";
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
            .QueryListWithOutTransactionAsync<Country>
                (query, parameters, CommandType.Text)).ToList();
    }

    private async Task<bool> ExistsAsync(string name,Guid countryId)
    {
        var parameters = new DynamicParameters();
        var query = "SELECT count(*) from [Country] where ";
        if (countryId != Guid.Empty)
        {
            parameters.Add("@countryId", countryId);
            query += " (id!=@countryId) and ";
        }
        parameters.Add("@name", name);
        query += " [name]=@name ";
        var result = await _connection.con
            .QueryWithOutTransactionAsync<int>
                (query, parameters, CommandType.Text);
        return result > 0;
    }
    public async Task<Country> AddCountryAsync(Country model)
    {
        if (await ExistsAsync(model.name, Guid.Empty))
            throw new Exception("Country already exists");
        var parameters = new DynamicParameters();
        parameters.Add("@name",model.name);
        parameters.Add("@code",model.code);
        return await _connection.con
            .QueryAsync<Country>("[dbo].[CountryAdd]", parameters);
    }
    public async Task<Country> UpdateCountryAsync(Country model)
    {
        if (await ExistsAsync(model.name, model.id))
            throw new Exception("Country already exists");
        var parameters = new DynamicParameters();
        parameters.Add("@id",model.id);
        parameters.Add("@name",model.name);
        parameters.Add("@code",model.code);
        return await _connection.con
            .QueryAsync<Country>("[dbo].[CountryUpdate]", parameters);
    }
    public async Task UpdateCountryIsActiveStatusAsync(Guid countryId,bool isActive)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@countryId",countryId);
        parameters.Add("@isActive",isActive);
        await _connection.con.ExecuteAsync("[dbo].[CountryIsActiveStatusUpdate]", parameters);
    }
}