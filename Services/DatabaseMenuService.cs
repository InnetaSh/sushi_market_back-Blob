using _26._02_sushi_market_back.Models;
using Microsoft.Data.Sqlite;

namespace _26._02_sushi_market_back.Services
{
    public class DatabaseMenuService
    {
        private const string ConnStr = "Data Source=menuDB.db";

        public void SaveProduct(MenuModel menu)
        {
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Menu 
                ( BlobUrl,Count, Title) 
                VALUES 
                ( @BlobUrl, @Count, @Title)";

           
            cmd.Parameters.AddWithValue("@BlobUrl", menu.BlobUrl);
            cmd.Parameters.AddWithValue("@Count", menu.Count);
            cmd.Parameters.AddWithValue("@Title", menu.Title);

            cmd.ExecuteNonQuery();
        }

        public List<MenuModel> GetProducts()
        {
            var list = new List<MenuModel>();
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            string query = @"
                SELECT Id, BlobUrl, Count,Title
                FROM Menu ";

            using var cmd = new SqliteCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new MenuModel
                {
                    Id = reader.GetInt32(0),
                    BlobUrl = reader.GetString(1),
                    Count = reader.GetString(2),
                    Title = reader.GetString(3)
                });
            }

            return list;
        }
        public MenuModel? GetProductById(int id)
        {
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            string query = "SELECT * FROM Menu WHERE Id = @Id";
            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new MenuModel
                {
                    Id = reader.GetInt32(0),
                    BlobUrl = reader.GetString(1),
                    Count = reader.GetString(2),
                    Title = reader.GetString(3)
                };
            }

            return null;
        }
        public void UpdateProduct(MenuModel menu)
        {
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            string query = @"
            UPDATE Menu SET
                BlobUrl = @BlobUrl,
                Count = @Count,
                Title = @Title
            WHERE Id = @Id";

            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@BlobUrl", menu.BlobUrl);
            cmd.Parameters.AddWithValue("@Count", menu.Count);
            cmd.Parameters.AddWithValue("@Title", menu.Title);
            cmd.Parameters.AddWithValue("@Id", menu.Id);

            cmd.ExecuteNonQuery();
        }
        public void DeleteProduct(int id)
        {
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            string query = "DELETE FROM Menu WHERE Id = @Id";
            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }

    }
}
