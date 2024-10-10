using System.Data;
using Dapper;
using Ensure.Application.IRepository;
using Ensure.DbContext;
using Ensure.Entities.Constant;

namespace Ensure.Infrastructure.Repository;

public class AuthRepo : IAuthRepo
{
    private readonly IConnections _connection;

    public AuthRepo(IConnections connection)
    {
        _connection = connection;
    }

    public async Task AddAuthAsync(Guid id, string password)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@id", id);
        parameters.Add("@password",Util.Hash(password));
        await _connection.con.ExecuteAsync("[dbo].[AuthAdd]", parameters);
    }
    public async Task<bool> CheckPasswordAsync(Guid id, string password)
    {
        var prams = new DynamicParameters();
        prams.Add("@id", id);
        prams.Add("@password", Util.Hash(password));

        return (await _connection.con.QueryWithOutTransactionAsync<int>(
            "SELECT count(*) FROM Auth WHERE id = @id AND [password] = @password", 
            prams, CommandType.Text)) > 0;
    }

}