using System;
using AdminSystem.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace AdminSystem.Model.Repositories
{
    public class OrderRepository : BaseRepository
    {
        public OrderRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public Order GetOrderById(int id)
        {
            NpgsqlConnection dbConn = null;
            try
            {
                dbConn = new NpgsqlConnection(_connectionString);

                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "select * from \"order\" where orderid = @orderid";
                cmd.Parameters.Add("@orderid", NpgsqlDbType.Integer).Value = id;

                var data = GetData(dbConn, cmd);
                if (data != null)
                {
                    if (data.Read())
                    {
                        return new Order(Convert.ToInt32(data["orderid"]))
                        {
                            CustomerEmail = data["customeremail"].ToString(),
                            OrderDate = Convert.ToDateTime(data["orderdate"]),
                            OrderTotal = Convert.ToDouble(data["ordertotal"]),
                            
                        };
                    }
                }
                return null;
            }
            finally
            {
                dbConn?.Close();
            }
        }
        public  List<Order>  GetCustomerOrders(string customeremail)
        {
            var conn = new NpgsqlConnection(_connectionString);
            var cmd = new NpgsqlCommand("SELECT * FROM \"order\" WHERE customeremail = @customeremail", conn);
            cmd.Parameters.AddWithValue("customeremail", customeremail);
            var data = GetData(conn, cmd);
            var orders = new List<Order>();

            while (data.Read())
            {
                orders.Add(new Order
                {
                    OrderId = Convert.ToInt32(data["orderid"]),
                    OrderDate = Convert.ToDateTime(data["orderdate"]),
                    OrderTotal = Convert.ToDouble(data["ordertotal"]),
                    CustomerEmail = data["customeremail"].ToString()
                });
            }

        return orders;
    }
            
        

        public List<Order> GetOrders()
        {
            NpgsqlConnection dbConn = null;
            var orders = new List<Order>();
            try
            {
                dbConn = new NpgsqlConnection(_connectionString);
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "select * from \"order\"";

                var data = GetData(dbConn, cmd);
                if (data != null)
                {
                    while (data.Read())
                    {
                        Order o = new Order(Convert.ToInt32(data["orderid"]))
                        {
                            
                            OrderDate = Convert.ToDateTime(data["orderdate"]),
                            OrderTotal = Convert.ToDouble(data["ordertotal"]),
                            CustomerEmail = data["customeremail"].ToString(),
                        };
                        orders.Add(o);
                    }
                }
                return orders;
            }
            finally
            {
                dbConn?.Close();
            }
        }

        public int CreateOrder(Order o)
        {
                
                using (var dbConn = new NpgsqlConnection(_connectionString))
                {
                dbConn.Open(); 
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = @"
                INSERT INTO ""order"" (orderdate, ordertotal, customeremail)
                VALUES (@orderdate, @ordertotal, @customeremail)
                RETURNING orderid;";
                cmd.Parameters.AddWithValue("@orderdate", NpgsqlDbType.Timestamp, DateTime.Now);
                cmd.Parameters.AddWithValue("@ordertotal", NpgsqlDbType.Double, o.OrderTotal);
                cmd.Parameters.AddWithValue("@customeremail", NpgsqlDbType.Text,
                string.IsNullOrEmpty(o.CustomerEmail) ? (object)DBNull.Value : o.CustomerEmail);
              
                var result  = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
               
            }
            }
            
        

        public bool UpdateOrder(Order o)
        {
            var dbConn = new NpgsqlConnection(_connectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
            update ""order"" set
            orderdate = @orderdate,
            ordertotal = @ordertotal,
            customeremail =@customeremail
            where
            orderid = @orderid";

              
                cmd.Parameters.AddWithValue("@orderdate", NpgsqlDbType.Text, o.OrderDate);
                cmd.Parameters.AddWithValue("@ordertotal", NpgsqlDbType.Double, o.OrderTotal);
                cmd.Parameters.Add("@customeremail", NpgsqlDbType.Text).Value =
                string.IsNullOrEmpty(o.CustomerEmail) ? DBNull.Value : o.CustomerEmail;
                cmd.Parameters.AddWithValue("@orderid", NpgsqlDbType.Integer, o.OrderId);

            bool result = UpdateData(dbConn, cmd);
            return result;
        }

        public bool DeleteOrder(int id)
        {
            var dbConn = new NpgsqlConnection(_connectionString);
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
            delete from ""order""
            where orderid = @orderid
            ";
            cmd.Parameters.AddWithValue("@orderid", NpgsqlDbType.Integer, id);

            bool result = DeleteData(dbConn, cmd);
            return result;
        }
    }
}
