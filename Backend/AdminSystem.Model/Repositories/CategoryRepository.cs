using System;
using AdminSystem.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace AdminSystem.Model.Repositories

{
    public class CategoryRepository : BaseRepository
    {
        public CategoryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public Category GetCategoryById(int id)
        {
            NpgsqlConnection dbConn = null;
            try
            {
                dbConn = new NpgsqlConnection(_connectionString); // Use _connectionString from BaseRepository

                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM category WHERE categoryid = @categoryid";
                cmd.Parameters.Add("@categoryid", NpgsqlDbType.Integer).Value = id;

                var data = GetData(dbConn, cmd);
                if (data != null && data.Read())
                {
                    return new Category(Convert.ToInt32(data["categoryid"]))
                    {
                        CategoryName = data["categoryname"].ToString(),
                    };
                }
                return null;
            }
            finally
            {
                dbConn?.Close();
            }
        }

        public List<Category> GetCategories()
        {
            NpgsqlConnection dbConn = null;
            var categories = new List<Category>();
            try
            {
                dbConn = new NpgsqlConnection(_connectionString); // Use _connectionString from BaseRepository
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM category";

                var data = GetData(dbConn, cmd);
                if (data != null)
                {
                    while (data.Read())
                    {
                        Category c = new Category(Convert.ToInt32(data["categoryid"]))
                        {
                            CategoryName = data["categoryname"].ToString(),
                        };
                        categories.Add(c);
                    }
                }
                return categories;
            }
            finally
            {
                dbConn?.Close();
            }
        }

        public bool InsertCategory(Category category)
        {
            NpgsqlConnection dbConn = null;
            try
            {
                dbConn = new NpgsqlConnection(_connectionString); // Use _connectionString from BaseRepository
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO category (categoryname)
                    VALUES (@categoryname) RETURNING categoryid
                ";
                cmd.Parameters.AddWithValue("@categoryname", NpgsqlDbType.Text, category.CategoryName);

                int insertedId = InsertData(dbConn, cmd); // Assuming InsertData returns the inserted category ID

                return insertedId > 0; // Returns true if the category was successfully inserted
            }
            finally
            {
                dbConn?.Close();
            }
        }

        public bool UpdateCategory(Category category)
        {
            NpgsqlConnection dbConn = new NpgsqlConnection(_connectionString); // Use _connectionString from BaseRepository
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                UPDATE category SET
                    categoryname = @categoryname
                WHERE categoryid = @categoryid
            ";
            cmd.Parameters.AddWithValue("@categoryname", NpgsqlDbType.Text, category.CategoryName);
            cmd.Parameters.AddWithValue("@categoryid", NpgsqlDbType.Integer, category.CategoryId);

            return UpdateData(dbConn, cmd); // Assuming UpdateData returns a boolean for success
        }

        public bool DeleteCategory(int id)
        {
            NpgsqlConnection dbConn = new NpgsqlConnection(_connectionString); // Use _connectionString from BaseRepository
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                DELETE FROM category
                WHERE categoryid = @categoryid
            ";
            cmd.Parameters.AddWithValue("@categoryid", NpgsqlDbType.Integer, id);

            return DeleteData(dbConn, cmd); // Assuming DeleteData returns a boolean for success
        }
    }
}
