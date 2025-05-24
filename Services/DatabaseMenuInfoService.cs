using _26._02_sushi_market_back.Models;
using Microsoft.Data.Sqlite;

namespace _26._02_sushi_market_back.Services
{
    public class DatabaseMenuInfoService
    {
        private const string ConnStr = "Data Source=menuDB.db";

        public void SaveProduct(MenuInfoModel menu)
        {
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO MenuInfo 
                (Category, BlobUrl, Title, About_1, About_2, Price) 
                VALUES 
                (@Category, @BlobUrl, @Title, @About_1, @About_2, @Price)";

            cmd.Parameters.AddWithValue("@Category", menu.Category ?? string.Empty);
            cmd.Parameters.AddWithValue("@BlobUrl", menu.BlobUrl);
            cmd.Parameters.AddWithValue("@Title", menu.Title);
            cmd.Parameters.AddWithValue("@About_1", menu.About_1);
            cmd.Parameters.AddWithValue("@About_2", menu.About_2);
            cmd.Parameters.AddWithValue("@Price", menu.Price);

            cmd.ExecuteNonQuery();
        }

        public List<MenuInfoModel> GetProducts()
        {
            var list = new List<MenuInfoModel>();
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            string query = @"
                SELECT Id, Category, BlobUrl, Title, About_1, About_2, Price
                FROM MenuInfo";

            using var cmd = new SqliteCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new MenuInfoModel
                {
                    Id = reader.GetInt32(0),
                    Category = reader.IsDBNull(1) ? null : reader.GetString(1),
                    BlobUrl = reader.GetString(2),
                    Title = reader.GetString(3),
                    About_1 = reader.GetString(4),
                    About_2 = reader.GetString(5),
                    Price = reader.GetString(6)
                });
            }

            return list;
        }
        public MenuInfoModel? GetProductById(int id)
        {
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            string query = "SELECT * FROM MenuInfo WHERE Id = @Id";
            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new MenuInfoModel
                {
                    Id = reader.GetInt32(0),
                    Category = reader.GetString(1),
                    BlobUrl = reader.GetString(2),
                    Title = reader.GetString(3),
                    About_1 = reader.GetString(4),
                    About_2 = reader.GetString(5),
                    Price = reader.GetString(6)
                };
            }

            return null;
        }

        public List<MenuInfoModel> GetProductByCategory(string category)
        {
            var list = new List<MenuInfoModel>();
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            string query = "SELECT * FROM MenuInfo WHERE Category = @Category";
            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@Category", category);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new MenuInfoModel
                {
                    Id = reader.GetInt32(0),
                    Category = reader.GetString(1),
                    BlobUrl = reader.GetString(2),
                    Title = reader.GetString(3),
                    About_1 = reader.GetString(4),
                    About_2 = reader.GetString(5),
                    Price = reader.GetString(6)
                });
            }

            return list;
        }

        public void UpdateProduct(MenuInfoModel menu)
        {
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            string query = @"
            UPDATE MenuInfo SET
                Category = @Category,
                BlobUrl = @BlobUrl,
                Title = @Title,
                About_1 = @About_1,
                About_2 = @About_2,
                Price = @Price
            WHERE Id = @Id";

            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@Category", menu.Category ?? "");
            cmd.Parameters.AddWithValue("@BlobUrl", menu.BlobUrl);
            cmd.Parameters.AddWithValue("@Title", menu.Title);
            cmd.Parameters.AddWithValue("@About_1", menu.About_1);
            cmd.Parameters.AddWithValue("@About_2", menu.About_2);
            cmd.Parameters.AddWithValue("@Price", menu.Price);
            cmd.Parameters.AddWithValue("@Id", menu.Id);

            cmd.ExecuteNonQuery();
        }

        public void DeleteProduct(int id)
        {
            using var conn = new SqliteConnection(ConnStr);
            conn.Open();

            string query = "DELETE FROM MenuInfo WHERE Id = @Id";
            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }

    }
}
