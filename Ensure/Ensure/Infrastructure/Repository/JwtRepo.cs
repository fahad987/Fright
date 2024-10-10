using Dapper;
using Ensure.Application.IRepository;
using Ensure.DbContext;
using Ensure.Entities.Domain;

namespace Ensure.Infrastructure.Repository;

public class JwtRepo : IJwtRepo
{
    private readonly IConnections _connections;

    public JwtRepo(IConnections connections)
    {
        _connections = connections;
    }

    public async Task AddJwtRefreshTokenAsync(RefreshToken model)
    {
        var prams = new DynamicParameters();
        prams.Add("@id", Guid.NewGuid());
        prams.Add("@userId", model.userId);
        prams.Add("@token", model.token);
        prams.Add("@jwtId", model.jwtId);
        prams.Add("@expiryDate", model.expiryDate);
        prams.Add("@createDate", model.createDate);
        await _connections.con.QueryAsync<RefreshToken>("[dbo].[JwtRefreshTokenAdd]", prams);
    }
}