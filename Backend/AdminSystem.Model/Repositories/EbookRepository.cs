using System;
using System.Collections.Generic;
using AdminSystem.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;

namespace AdminSystem.Model.Repositories
{
    public class EbookRepository : BaseRepository
    {
        public EbookRepository(IConfiguration configuration) : base(configuration)
        {
        }

        // Method to fetch an Ebook by its ID
        public Ebook GetEbookById(int id)
        {
            NpgsqlConnection dbConn = null;
            try
            {
                dbConn = new NpgsqlConnection(_connectionString); // Use _connectionString from BaseRepository
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM ebook WHERE ebookid = @ebookid";
                cmd.Parameters.Add("@ebookid", NpgsqlDbType.Integer).Value = id;

                var data = GetData(dbConn, cmd);
                if (data != null && data.Read()) // Ensure data is read
                {
                    return new Ebook(Convert.ToInt32(data["ebookid"]))
                    {
                        Title = data["title"].ToString(),
                        Author = data["author"].ToString(),
                        PublicationYear = (int)data["publicationyear"],
                        Price = Convert.ToDecimal(data["price"]),
                        File_url = data["file_url"].ToString(),
                        Image_url = data["image_url"].ToString(),
                        CategoryId = (int)data["categoryid"]
                    };
                }
                return null;
            }
            finally
            {
                dbConn?.Close();
            }
        }

        // Method to fetch all ebooks
        public List<Ebook> GetEbooks()
        {
            NpgsqlConnection dbConn = null;
            var ebooks = new List<Ebook>();
            try
            {
                dbConn = new NpgsqlConnection(_connectionString); // Use _connectionString from BaseRepository
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = "SELECT * FROM ebook";

                var data = GetData(dbConn, cmd);
                if (data != null)
                {
                    while (data.Read()) // Loop through all rows
                    {
                        var ebook = new Ebook(Convert.ToInt32(data["ebookid"]))
                        {
                            Title = data["title"].ToString(),
                            Author = data["author"].ToString(),
                            PublicationYear = (int)data["publicationyear"],
                            Price = Convert.ToDecimal(data["price"]),
                            File_url = data["file_url"].ToString(),
                            Image_url = data["image_url"].ToString(),
                            CategoryId = (int)data["categoryid"]
                        };
                        ebooks.Add(ebook);
                    }
                }
                return ebooks;
            }
            finally
            {
                dbConn?.Close();
            }
        }

        // Method to insert a new Ebook
        public bool InsertEbook(Ebook ebook)
        {
            NpgsqlConnection dbConn = null;
            try
            {
                dbConn = new NpgsqlConnection(_connectionString); // Use _connectionString from BaseRepository
                var cmd = dbConn.CreateCommand();
                cmd.CommandText = @"
                    INSERT INTO ebook (title, author, publicationyear, price, file_url, image_url, categoryid)
                    VALUES (@title, @author, @publicationyear, @price, @file_url, @image_url, @categoryid) RETURNING ebookid"; // Return the inserted ID

                cmd.Parameters.AddWithValue("@title", NpgsqlDbType.Text, ebook.Title);
                cmd.Parameters.AddWithValue("@author", NpgsqlDbType.Text, ebook.Author);
                cmd.Parameters.AddWithValue("@publicationyear", NpgsqlDbType.Integer, ebook.PublicationYear);
                cmd.Parameters.AddWithValue("@price", NpgsqlDbType.Numeric, ebook.Price); // Use Numeric for precision
                cmd.Parameters.AddWithValue("@file_url", NpgsqlDbType.Text, ebook.File_url);
                cmd.Parameters.AddWithValue("@image_url", NpgsqlDbType.Text, ebook.Image_url);
                cmd.Parameters.AddWithValue("@categoryid", NpgsqlDbType.Integer, ebook.CategoryId);

                // Get the inserted ebook ID
                int insertedId = InsertData(dbConn, cmd); // Assuming InsertData returns the inserted ID

                return insertedId > 0; // Returns true if a valid ID is returned (i.e., insertion was successful)
            }
            finally
            {
                dbConn?.Close();
            }
        }


        // Method to update an existing Ebook
        public bool UpdateEbook(Ebook ebook)
        {
            NpgsqlConnection dbConn = new NpgsqlConnection(_connectionString); // Use _connectionString from BaseRepository
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                UPDATE ebook
                SET title = @title,
                    author = @author,
                    publicationyear = @publicationyear,
                    price = @price,
                    file_url = @file_url,
                    image_url = @image_url,
                    categoryid = @categoryid
                WHERE ebookid = @ebookid";

            cmd.Parameters.AddWithValue("@title", NpgsqlDbType.Text, ebook.Title);
            cmd.Parameters.AddWithValue("@author", NpgsqlDbType.Text, ebook.Author);
            cmd.Parameters.AddWithValue("@publicationyear", NpgsqlDbType.Integer, ebook.PublicationYear);
            cmd.Parameters.AddWithValue("@price", NpgsqlDbType.Numeric, ebook.Price); // Use Numeric for precision
            cmd.Parameters.AddWithValue("@file_url", NpgsqlDbType.Text, ebook.File_url);
            cmd.Parameters.AddWithValue("@image_url", NpgsqlDbType.Text, ebook.Image_url);
            cmd.Parameters.AddWithValue("@categoryid", NpgsqlDbType.Integer, ebook.CategoryId);
            cmd.Parameters.AddWithValue("@ebookid", NpgsqlDbType.Integer, ebook.EbookId);

            bool result = UpdateData(dbConn, cmd); // Use the base method for updating data
            return result;
        }

        // Method to delete an Ebook by its ID
        public bool DeleteEbook(int id)
        {
            NpgsqlConnection dbConn = new NpgsqlConnection(_connectionString); // Use _connectionString from BaseRepository
            var cmd = dbConn.CreateCommand();
            cmd.CommandText = @"
                DELETE FROM ebook
                WHERE ebookid = @ebookid";

            cmd.Parameters.AddWithValue("@ebookid", NpgsqlDbType.Integer, id);

            bool result = DeleteData(dbConn, cmd); // Use the base method for deleting data
            return result;
        }
    }
}
