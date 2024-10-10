using Ensure.Entities.Constant;
using Microsoft.Extensions.Options;

namespace Ensure.DbContext;

public class Connections : IConnections
{
    public Connections(IOptions<Settings> settings)
    {
        con = DbManagerFactory.CreateInstance(settings.Value.connectionString.con);
    }
    
    public void Dispose()
    {
        con.Dispose();
    }

    public IDbManager con { get; }
}