using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace Ensure.Entities.Constant;

public static class Util
{
    private static IOptions<Settings> _settings;

    private static IHttpContextAccessor _httpContextAccessor;

    public static int pageSize { get; set; } = 20;
    public static void Configure(IHttpContextAccessor contextAccessor, IOptions<Settings> settings)
    {
        _httpContextAccessor = contextAccessor;
        _settings = settings;
    }
    public static string GetStringSplit<T>(IEnumerable<T> list)
    {
        return $" SELECT VALUE FROM string_split('{GetString(list)}',',') ";
    }
    public static string DBPaging(int? size = null, int pageNo = 0)
    {
        size ??= pageSize;

        return pageNo == 0
            ? " "
            : " OFFSET(" + pageNo + " - 1) * " + size + " ROWS FETCH NEXT " + size + " ROWS ONLY ";
    }
    public static Response<T> BuildResponse<T>(T data,bool status=true,string message = "success")
    {
        return new Response<T>(data, message,status);
    }
    public static string GetString<T>(IEnumerable<T> list)
    {
        return string.Join(",", list);
    }
    public static string Hash(string password)
    {
        var bytes = new UTF8Encoding().GetBytes(password);
        byte[] hashBytes;
        using (var algorithm = new SHA512Managed())
        {
            hashBytes = algorithm.ComputeHash(bytes);
        }

        return Convert.ToBase64String(hashBytes);
    }
    public static Response BuildResponse(string message = "success",bool status=true)
    {
        return new Response(message, status);
    }
}