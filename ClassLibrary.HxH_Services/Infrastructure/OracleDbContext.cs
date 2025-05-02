﻿using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ClassLibrary.HxH_Services.Infrastructure;

public class OracleDbContext
{
    private readonly string _connectionString;
    private readonly ILogger<OracleDbContext> _logger;

    public OracleDbContext(string connectionString, ILogger<OracleDbContext> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    // INSERT, UPDATE, DELETE, TRUNCATE, CREATE TABLE, ALTER TABLE, BEGIN NULL; END;
    // Return affected Rows
    public async Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken, string query, params OracleParameter[] parameters)
    {
        try
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new OracleCommand(query, connection);
            if (parameters.Length > 0)
                command.Parameters.AddRange(parameters);

            _logger.LogInformation("[OracleDbContext] Query execute correctly: {Query}", query);
            return await command.ExecuteNonQueryAsync(cancellationToken);
        }
        catch (OracleException ex)
        {
            _logger.LogError(ex, "[OracleDbContext] Oracle error in ExecuteNonQueryAsync with query: {Query}", query);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[OracleDbContext] Error in ExecuteNonQueryAsync with query: {Query}", query);
            throw;
        }
    }

    // SELECT return a DataTable
    public async Task<DataTable> ExecuteQueryAsync(CancellationToken cancellationToken, string query, params OracleParameter[] parameters)
    {
        try
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new OracleCommand(query, connection);
            if (parameters.Length > 0)
                command.Parameters.AddRange(parameters);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var resultTable = new DataTable();
            resultTable.Load(reader);

            _logger.LogInformation("[OracleDbContext] Query executed: {Query} with Parameters: {@Parameters}", query, parameters);
            return resultTable;
        }
        catch (OracleException ex)
        {
            _logger.LogError(ex, "[OracleDbContext] Oracle error in ExecuteNonQueryAsync with query: {Query}", query);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[OracleDbContext] Error in ExecuteNonQueryAsync with query: {Query} and Parameters: {@Parameters}", query, parameters);
            throw;
        }
    }

    // SELECT return a single value type T
    // SELECT COUNT(Row1) FROM Table1
    // INSERT INTO Table1 (Row1) VALUES (1) RETURNING id INTO :id
    public async Task<T> ExecuteScalarAsync<T>(CancellationToken cancellationToken, string query, params OracleParameter[] parameters)
    {
        try
        {
            using var connection = new OracleConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            using var command = new OracleCommand(query, connection);
            if (parameters.Length > 0)
                command.Parameters.AddRange(parameters);

            var result = await command.ExecuteScalarAsync(cancellationToken);
            return (T)Convert.ChangeType(result, typeof(T));
        }
        catch (OracleException ex)
        {
            _logger.LogError(ex, "[OracleDbContext] Oracle error in ExecuteNonQueryAsync with query: {Query}", query);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[OracleDbContext] Error in ExecuteScalarAsync with query: {Query}", query);
            throw;
        }
    }

}
