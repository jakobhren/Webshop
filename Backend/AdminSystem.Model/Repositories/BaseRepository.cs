namespace AdminSystem.Model.Repositories;
using Npgsql;
using Microsoft.Extensions.Configuration;
using System.Data;

public class BaseRepository
{
    protected string _connectionString;

    public BaseRepository(IConfiguration configuration)
    {
        // Initialize _connectionString from configuration
        _connectionString = configuration.GetConnectionString("AppProgDb");
    }

    protected NpgsqlDataReader GetData(NpgsqlConnection conn, NpgsqlCommand cmd)
    {
    if (conn.State != ConnectionState.Open)
        conn.Open(); // Open safely only if not already open

    cmd.Connection = conn;
    return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }
    protected bool InsertData(NpgsqlConnection conn, NpgsqlCommand cmd)
    {
    conn.Open();
    cmd.ExecuteNonQuery();
    return true;
    }
    protected bool UpdateData(NpgsqlConnection conn, NpgsqlCommand cmd)
    {
    conn.Open();
    cmd.ExecuteNonQuery();
    return true;
    }
    protected bool DeleteData(NpgsqlConnection conn, NpgsqlCommand cmd)
    {
    conn.Open();
    cmd.ExecuteNonQuery();
    return true;
    }
}
