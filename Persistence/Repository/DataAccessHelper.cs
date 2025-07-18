﻿using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Persistence.Repository;

public class DataAccessHelper
{
    private readonly IConfiguration _config;

    public DataAccessHelper(IConfiguration config)
    {
        _config = config;
    }

    public async Task<List<T>> QueryData<T, U>(string storedProcedure, U parameters)
    {
        using (IDbConnection connection = new SqlConnection(_config.GetConnectionString("MSSQL")))
        {
            var rows = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            return rows.ToList();
        }
    }

    public async Task<List<T>> ExecuteSqlQuery<T, U>(string query, U parameters)
    {
        using (IDbConnection connection = new SqlConnection(_config.GetConnectionString("MSSQL")))
        {
            var rows = await connection.QueryAsync<T>(query, parameters, commandType: CommandType.Text);
            return rows.ToList();
        }
    }

    public async Task<int> ExecuteData<T>(string storedProcedure, T parameters)
    {
        using (IDbConnection connection = new SqlConnection(_config.GetConnectionString("MSSQL")))
        {
            return await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
    }

}

