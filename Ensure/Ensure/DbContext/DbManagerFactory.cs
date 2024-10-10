namespace Ensure.DbContext;

public static class DbManagerFactory
{
    public static DbManager CreateInstance(string connectionString) => new DbManager(connectionString);
}