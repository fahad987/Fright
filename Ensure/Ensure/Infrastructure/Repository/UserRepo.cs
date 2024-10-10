using System.Data;
using Dapper;
using Ensure.Application.IHelper;
using Ensure.Application.IRepository;
using Ensure.DbContext;
using Ensure.Entities.Constant;
using Ensure.Entities.Domain;
using Ensure.Entities.Enum;
using EnsureFreightInc.Entities.Domain;

namespace Ensure.Infrastructure.Repository;

public class UserRepo : IUserRepo
{
    private readonly IConnections _connection;
    private readonly ActiveSession _activeSession;
    private readonly IUploadHelper _uploadHelper;
    

    public UserRepo(IConnections connection, ActiveSession activeSession, IUploadHelper uploadHelper)
    {
        _connection = connection;
        _activeSession = activeSession;
        _uploadHelper = uploadHelper;
    }
    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        var prams = new DynamicParameters();
        prams.Add("@userId", userId);
        var result= await _connection.con
            .QueryWithOutTransactionAsync<User>("[dbo].[UserGetById]", prams);
        result.roles = await GetAllUserRoleAsync(new List<Guid>() {userId}, new List<Guid>());
        return result;
    }
    public async Task<User> GetUserByEmailAsync(string email)
    {
        var prams = new DynamicParameters();
        prams.Add("@email", email);
        return await _connection.con.QueryWithOutTransactionAsync<User>
        ("SELECT * FROM [User] WHERE email = @email", prams,
            CommandType.Text);
    }
    private async Task<bool> ExistAsync(string email,Guid? userId=null)
    {
        var query = $"SELECT count(*) FROM [User] where ";
        var prams = new DynamicParameters();
        prams.Add("@email", email);
        if (userId != null || userId!=Guid.Empty)
        {
            prams.Add("@userId", userId);
            query += " (id!=@userId) and ";
        }
        query += " [email]=@email ";
        return (await _connection.con.QueryWithOutTransactionAsync<int>
        (query, prams,
            CommandType.Text))>0;
    }
    public async Task<User> AddUserAsync(User model)
    {
        if (await ExistAsync(model.email))
            throw new Exception("Email already registered");
        var parameters = new DynamicParameters();
        parameters.Add("@id", Guid.NewGuid());
        parameters.Add("@titleId",model.titleId);
        parameters.Add("@firstName",model.firstName);
        parameters.Add("@lastName",model.lastName);
        parameters.Add("@personalEmailAddress",model.personalEmailAddress);
        parameters.Add("@cnic",model.cnic);
        parameters.Add("@homePhoneNo",model.homePhoneNo);
        parameters.Add("@mobilePhoneNo",model.mobilePhoneNo);
        parameters.Add("@address",model.address);
        parameters.Add("@cityId",model.cityId);
        parameters.Add("@province",model.province);
        parameters.Add("@maritalStatusId",model.maritalStatusId);
        parameters.Add("@emergencyContactNo",model.emergencyContactNo);
        parameters.Add("@email",model.email);
        parameters.Add("@branchId",model.branchId);
        parameters.Add("@imageId",await _uploadHelper.GetUploadIdAsync(model.imageFile,model.imageId));
        parameters.Add("@createBy",_activeSession.userId);
        parameters.Add("@createDate",DateTime.UtcNow);
        var result=await _connection.con.QueryAsync<User>("[dbo].[UserAdd]", parameters);
        result.roles = await AddUserRoleAsync(model.roles, result.id);
        return result;
    }

    public async Task<User> UpdateUserAsync(User model)
    {
        if (await ExistAsync(model.email,model.id))
            throw new Exception("Email already registered");
        var parameters = new DynamicParameters();
        parameters.Add("@id", model.id);
        parameters.Add("@titleId",model.titleId);
        parameters.Add("@firstName",model.firstName);
        parameters.Add("@lastName",model.lastName);
        parameters.Add("@personalEmailAddress",model.personalEmailAddress);
        parameters.Add("@cnic",model.cnic);
        parameters.Add("@homePhoneNo",model.homePhoneNo);
        parameters.Add("@mobilePhoneNo",model.mobilePhoneNo);
        parameters.Add("@address",model.address);
        parameters.Add("@cityId",model.cityId);
        parameters.Add("@province",model.province);
        parameters.Add("@maritalStatusId",model.maritalStatusId);
        parameters.Add("@emergencyContactNo",model.emergencyContactNo);
        parameters.Add("@email",model.email);
        parameters.Add("@branchId",model.branchId);
        parameters.Add("@imageId",await _uploadHelper.GetUploadIdAsync(model.imageFile,model.imageId));
        parameters.Add("@updateBy",_activeSession.userId);
        parameters.Add("@updateDate",DateTime.UtcNow);
        var result=await _connection.con.QueryAsync<User>("[dbo].[UserUpdate]", parameters);
        await RemoveUserRoleAsync(model.id);
        result.roles = await AddUserRoleAsync(model.roles, model.id);
        return result;
    }

    private async Task<List<UserRole>> GetAllUserRoleAsync(List<Guid> users, List<Guid> roles)
    {
        if (!users.Any() && !roles.Any()) return new List<UserRole>();
        var parameters = new DynamicParameters();
        var query = "SELECT * from [vwUserRole] where ";
        if (users.Any())
            query += $" (userId IN ({Util.GetStringSplit(users)})) and ";
        if (roles.Any())
            query += $" (roleId IN ({Util.GetStringSplit(roles)})) and ";
        query += " id is not null ";
        return (await _connection.con
            .QueryListWithOutTransactionAsync<UserRole>
                (query, parameters, CommandType.Text)).ToList();

    }

    public async Task<List<User>> GetAllUserAsync(UserFilter model)
    {
        var parameters = new DynamicParameters();
        var query = "SELECT * from [vwUser] where ";
        if (!string.IsNullOrEmpty(model.search))
        {
            parameters.Add("@search", $"%{model.search}%");
            query +=
                "([firstName] like @search or [lastName] like @search or [cnic] like @search or [address] like @search or [email] like @search) and ";
        }

        if (model.branches.Any())
            query += $" (branchId IN ({Util.GetStringSplit(model.branches)})) and ";
        if (model.roles.Any())
            query +=
                $" (id IN (SELECT userId FROM UserRole where roleId In ({Util.GetStringSplit(model.roles)}))) and ";

        if (model.isActive != IsActiveEnum.Both)
        {
            parameters.Add("@isActive", model.isActive);
            query += "(isActive=@isActive) and ";
        }

        query += " [firstName] IS NOT NULL order by [firstName] ";
        if (model.pageNo > 0)
            query += Util.DBPaging(model.pageSize, model.pageNo);
        var result = (await _connection.con
            .QueryListWithOutTransactionAsync<User>
                (query, parameters, CommandType.Text)).ToList();
        if (!result.Any()) return result;
        var roles = await GetAllUserRoleAsync(
            result.Select(x => x.id).ToList(),
            new List<Guid>());
        foreach (var row in result)
        {
            row.roles = roles.Where(x => x.userId == row.id).ToList();
        }

        return result;
    }
    public async Task UpdateUserIsActiveStatusAsync(Guid userId,bool isActive)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@userId",userId);
        parameters.Add("@isActive",isActive);
        await _connection.con.ExecuteAsync("[dbo].[UserIsActiveStatusUpdate]", parameters);
    }

    public async Task<bool> RemoveUserAsync(Guid id)
    {
        throw new NotImplementedException();
    }
    private async Task<List<UserRole>> AddUserRoleAsync(List<UserRole> roles,Guid userId)
    {
        if (!roles.Any()) return new List<UserRole>();
        var parameters = new DynamicParameters();
        parameters.Add("@userId",userId);
        parameters.Add("@roles",Util.GetString(roles.Select(x=>x.roleId)));
        return (await _connection.con
                .QueryListAsync<UserRole>("[dbo].[UserRoleAdd]", parameters))
            .ToList();
    }
    private async Task RemoveUserRoleAsync(Guid userId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@userId",userId);
        await _connection.con.ExecuteAsync("[dbo].[UserRoleRemove]", parameters);
        
    }
}