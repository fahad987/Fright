using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Ensure.DbContext;

public class DbManager : IDbManager
{
     private readonly IDbConnection _connection;
        private readonly string _connectionString;
        private IDbConnection _tempConnection;
        private IDbTransaction _transaction;

        public DbManager(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqlConnection(connectionString);
        }

        public IDbConnection GetConnection()
        {
            return _connection;
        }

        public IDbTransaction GetTransaction()
        {
            return _transaction;
        }

        public void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
        }

        public void BeginTransaction()
        {
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
        }

        public void CommitTransactionAndDispose()
        {
            _transaction.Commit();
            _transaction.Dispose();
            _connection.Dispose();
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
        }

        public void RollbackTransactionAndDispose()
        {
            if (_transaction == null) return;
            _transaction.Rollback();
            _transaction.Dispose();
            _connection.Dispose();
        }

        public void DisposeConnection()
        {
            _connection.Dispose();
        }

        public async Task<IEnumerable<T>> QueryListAsync<T>(string sql, DynamicParameters parameters = null,
            CommandType commandType = CommandType.StoredProcedure)
        {
            return await _connection.QueryAsync<T>(sql, parameters, _transaction, commandType: commandType);
        }

        public async Task<IEnumerable<T>> QueryListWithOutTransactionAsync<T>(string sql,
            DynamicParameters parameters = null,
            CommandType commandType = CommandType.StoredProcedure)
        {
            _tempConnection = new SqlConnection(_connectionString);
            if (_tempConnection.State == ConnectionState.Closed)
                _tempConnection.Open();
            var result = await _tempConnection.QueryAsync<T>(sql, parameters, commandType: commandType);
            _tempConnection.Close();
            _tempConnection.Dispose();
            return result;
        }

        public async Task<T> QueryAsync<T>(string sql, DynamicParameters parameters = null,
            CommandType commandType = CommandType.StoredProcedure)
        {
            return await _connection.QueryFirstOrDefaultAsync<T>(sql, parameters, _transaction, commandType: commandType);
        }

        public async Task<T> QueryWithOutTransactionAsync<T>(string sql, DynamicParameters parameters = null,
            CommandType commandType = CommandType.StoredProcedure)
        {
            _tempConnection = new SqlConnection(_connectionString);
            if (_tempConnection.State == ConnectionState.Closed)
                _tempConnection.Open();
            var result = await _tempConnection.QueryFirstOrDefaultAsync<T>(sql, parameters, commandType: commandType);
            _tempConnection.Close();
            _tempConnection.Dispose();
            return result;
        }

       
        public async Task<bool> ExecuteAsync(string sp, DynamicParameters parameters = null,
            CommandType commandType = CommandType.StoredProcedure)
        {
            var result = await _connection.ExecuteAsync(sp, parameters, _transaction, commandType: commandType);
            return result > 0;
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _transaction?.Dispose();
        }
}