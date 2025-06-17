
using System;
using AdminSystem.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
namespace AdminSystem.Model.Repositories;

public class CustomerRepository : BaseRepository
{
    public CustomerRepository(IConfiguration configuration) : base(configuration)
    {
        // Additional initialization if needed
    }

    public bool DeleteCustomer(int id)
    {
        using var conn = new NpgsqlConnection(_connectionString); // Use _connectionString from BaseRepository
        using var cmd = new NpgsqlCommand("DELETE FROM customers WHERE id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        return DeleteData(conn, cmd); // Use inherited DeleteData method
    }

    public Customer GetCustomerById(int id)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        using var cmd = new NpgsqlCommand("SELECT * FROM customers WHERE customerid = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = GetData(conn, cmd); // Use inherited GetData method
        if (reader.Read())
        {
            return new Customer
            {
                CustomerId = (int)reader["customerid"],
                Name = reader["name"].ToString(),
                Email = reader["email"].ToString(),
                Password = reader["password"].ToString(),
                Creation = (DateTime)reader["creation"]
            };
        }
        return null;
    }

    public int InsertCustomer(Customer customer)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        using var cmd = new NpgsqlCommand("INSERT INTO customers (name, email, password) VALUES (@name, @email, @password) RETURNING customerid", conn);
        cmd.Parameters.AddWithValue("@name", customer.Name);
        cmd.Parameters.AddWithValue("@email", customer.Email);
        cmd.Parameters.AddWithValue("@password", customer.Password);
        
        return (int)InsertData(conn, cmd); // Use inherited InsertData method
    }

    public bool UpdateCustomer(Customer customer)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        using var cmd = new NpgsqlCommand("UPDATE customers SET name = @name, email = @email, password = @password WHERE customerid = @id", conn);
        cmd.Parameters.AddWithValue("@id", customer.CustomerId);
        cmd.Parameters.AddWithValue("@name", customer.Name);
        cmd.Parameters.AddWithValue("@email", customer.Email);
        cmd.Parameters.AddWithValue("@password", customer.Password);
        
        return UpdateData(conn, cmd); // Use inherited UpdateData method
    }
}
