using Dapper;
using Ensure.Application.IRepository;
using Ensure.DbContext;
using Ensure.Entities.Domain;

namespace Ensure.Infrastructure.Repository;

public class AttachmentRepo: IAttachmentRepo
{
    private readonly IConnections _connection;

    public AttachmentRepo(IConnections connection)
    {
        _connection = connection;
    }

    public async Task<Attachment> AddAttachmentAsync(string path, string name)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@path", path);
        parameters.Add("@name", name);
        return await _connection.con.QueryAsync<Attachment>("[dbo].[AttachmentInsert]", parameters);
    }
}