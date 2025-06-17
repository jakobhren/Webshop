using System;
using System.Collections.Generic;
using AdminSystem.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace AdminSystem.Model.Repositories
{
    public class OrderItemRepository : BaseRepository
    {
        // Use constructor from BaseRepository that accepts IConfiguration
        public OrderItemRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public OrderItem GetOrderItemById(int id)
        {
            NpgsqlConnection dbConn = null;
            try
            {
                dbConn = new NpgsqlConnection(_connectionString);  // Use ConnectionString from BaseRepository

                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM orderitem WHERE orderitemid = @orderitemid";
                cmd.Parameters.Add("@orderitemid", NpgsqlDbType.Integer).Value = id;

                var data = GetData(dbConn, cmd);
                if (data != null && data.Read())
                {
                    return new OrderItem(Convert.ToInt32(data["orderitemid"]))
                    {
                        OrderId = (int)data["orderid"],
                        EbookId = (int)data["ebookid"],
                    };
                }
                return null;
            }
            finally
            {
                dbConn?.Close();
            }
        }

        public List<OrderItem> GetOrderItems()
        {
            NpgsqlConnection dbConn = null;
            var orderItems = new List<OrderItem>();
            try
            {
                dbConn = new NpgsqlConnection(_connectionString);  // Use ConnectionString from BaseRepository
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM orderitem";

                var data = GetData(dbConn, cmd);
                if (data != null)
                {
                    while (data.Read())
                    {
                        OrderItem item = new OrderItem(Convert.ToInt32(data["orderitemid"]))
                        {
                            OrderId = (int)data["orderid"],
                            EbookId = (int)data["ebookid"],
                        };
                        orderItems.Add(item);
                    }
                }
                return orderItems;
            }
            finally
            {
                dbConn?.Close();
            }
        }

        public int CreateOrderItem(OrderItem item)
        {
            NpgsqlConnection dbConn = null;
            try
            {
                dbConn = new NpgsqlConnection(_connectionString);  // Use ConnectionString from BaseRepository
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO ""orderitem"" (orderid, ebookid)
                    VALUES (@orderid, @ebookid)
                    RETURNING orderitemid;";

                cmd.Parameters.AddWithValue("@orderid", NpgsqlDbType.Integer, item.OrderId);
                cmd.Parameters.AddWithValue("@ebookid", NpgsqlDbType.Integer, item.EbookId);

                dbConn.Open();  // Open connection
                var result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);  // Return the inserted orderitemid
            }
            finally
            {
                dbConn?.Close();
            }
        }

        public bool UpdateOrderItem(OrderItem item)
        {
            NpgsqlConnection dbConn = null;
            try
            {
                dbConn = new NpgsqlConnection(_connectionString);  // Use ConnectionString from BaseRepository
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = @"
                    UPDATE orderitem SET
                        orderid = @orderid,
                        ebookid = @ebookid
                    WHERE orderitemid = @orderitemid";

                cmd.Parameters.AddWithValue("@orderid", NpgsqlDbType.Integer, item.OrderId);
                cmd.Parameters.AddWithValue("@ebookid", NpgsqlDbType.Integer, item.EbookId);
                cmd.Parameters.AddWithValue("@orderitemid", NpgsqlDbType.Integer, item.OrderItemId);

                dbConn.Open();  // Open connection
                return UpdateData(dbConn, cmd);  // Assuming UpdateData returns a boolean for success
            }
            finally
            {
                dbConn?.Close();
            }
        }

        public bool DeleteOrderItem(int id)
        {
            NpgsqlConnection dbConn = null;
            try
            {
                dbConn = new NpgsqlConnection(_connectionString);  // Use ConnectionString from BaseRepository
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = @"
                    DELETE FROM orderitem
                    WHERE orderitemid = @orderitemid";

                cmd.Parameters.AddWithValue("@orderitemid", NpgsqlDbType.Integer, id);

                dbConn.Open();  // Open connection
                return DeleteData(dbConn, cmd);  // Assuming DeleteData returns a boolean for success
            }
            finally
            {
                dbConn?.Close();
            }
        }
    }
}
