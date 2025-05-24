//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using _26._02_sushi_market_back.Models;
//using System.Collections.Generic;
//using System.Data;
//using Microsoft.Data.SqlClient;
//using Microsoft.AspNetCore.Mvc;


//namespace _26._02_sushi_market_back.Services
//{
//    public class RegisterService
//    {
//        private readonly string _connectionString;
//        private readonly ILogger<RegisterService> _logger;

//        public RegisterService(string connectionString, ILogger<RegisterService> logger)
//        {
//            _connectionString = connectionString; 
//            _logger = logger;
//        }

//        private void EnsureTableExists()
//        {
//            using var connection = new SqlConnection(_connectionString);
//            connection.Open();

//            _logger.LogInformation(" Connected to database: " + connection.Database);

//            var command = connection.CreateCommand();
//            command.CommandText = @"
//        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'sushiUsers')
//        BEGIN
//            CREATE TABLE sushiUsers (
//                Id INT IDENTITY(1,1) PRIMARY KEY,
//                UserName NVARCHAR(50) NOT NULL,
//                Password NVARCHAR(50) NOT NULL,
//                Email NVARCHAR(50) NOT NULL
//            )
//        END";

//            _logger.LogInformation("Executing SQL to ensure 'sushiUsers' table exists...");
//            command.ExecuteNonQuery();
//            _logger.LogInformation("Table 'sushiUsers' checked/created if not exists.");
//        }


//        public List<User> GetAllUsers()
//        {
//            EnsureTableExists();
//            var tests = new List<User>();

//            try
//            {
//                _logger.LogInformation("Attempting to connect to the database with connection string: " + _connectionString);

//                using (SqlConnection connection = new SqlConnection(_connectionString))
//                {
//                    connection.Open();
//                    _logger.LogInformation("Successfully connected to database: " + connection.Database);

//                    string query = "SELECT Id, UserName, Password, Email FROM sushiUsers";

//                    using (SqlCommand command = new SqlCommand(query, connection))
//                    using (SqlDataReader reader = command.ExecuteReader())
//                    {
//                        _logger.LogInformation("Executing SQL query: " + query);

//                        if (reader.HasRows)
//                        {
//                            while (reader.Read())
//                            {
//                                tests.Add(new User
//                                {
//                                    Id = reader.GetInt32(0),
//                                    UserName = reader.GetString(1),
//                                    Password = reader.GetString(2),
//                                    Email = reader.GetString(3)
//                                });
//                            }
//                            _logger.LogInformation("Successfully retrieved " + tests.Count + " test records.");
//                        }
//                        else
//                        {
//                            _logger.LogWarning("No records found in the 'TestForWork' table.");
//                        }
//                    }
//                }
//                return tests;
//            }
//            catch (SqlException sqlEx)
//            {
//                _logger.LogError("SQL Exception occurred: " + sqlEx.Message + "\n" + sqlEx.StackTrace);
//                throw new Exception("A SQL error occurred while retrieving tests.", sqlEx);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError("An error occurred while retrieving tests: " + ex.Message + "\n" + ex.StackTrace);
//                throw new Exception("An error occurred while retrieving tests.", ex);
//            }
//        }



//        public List<User> Register(UserRequestReg request)
//        {
//            EnsureTableExists();
//            var users = new List<User>();


//            using (SqlConnection connection = new SqlConnection(_connectionString))
//            {
//                connection.Open();

//                string query = @"
//                                INSERT INTO sushiUsers (UserName, Password, Email) 
//                                VALUES (@UserName, @Password, @Email); 
//                                SELECT CAST(SCOPE_IDENTITY() AS INT);";


//                using (SqlCommand command = new SqlCommand(query, connection))
//                {
//                    command.Parameters.Add("@UserName", SqlDbType.NVarChar, 500).Value = request.UserName;
//                    command.Parameters.Add("@Password", SqlDbType.NVarChar, -1).Value = request.Password;
//                    command.Parameters.Add("@Email", SqlDbType.NVarChar, 10000).Value = request.Email;

//                    var result = command.ExecuteScalar();


//                    if (result != null && result != DBNull.Value)
//                    {
//                        int newId = Convert.ToInt32(result);

//                        users.Add(new User
//                        {
//                            Id = newId,
//                            UserName = request.UserName,
//                            Password = request.Password,
//                            Email = request.Email
//                        });
//                    }
//                }
//            }

//            return users;
//        }




//        public User Login([FromBody] UserRequestLog request)
//        {

//            User foundUser = null;

//            using (SqlConnection connection = new SqlConnection(_connectionString))
//            {
//                connection.Open();

//                string query = "SELECT Username, Password FROM sushiUsers WHERE Username = @Username AND Password = @password";

//                using (SqlCommand command = new SqlCommand(query, connection))
//                {

//                    command.Parameters.AddWithValue("@Username", request.UserName);
//                    command.Parameters.AddWithValue("@password", request.Password);

//                    using (SqlDataReader reader = command.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            foundUser = new User
//                            {

//                                UserName = reader.GetString(reader.GetOrdinal("Username")),
//                                Password = reader.GetString(reader.GetOrdinal("Password")),
//                            };
//                        }
//                    }
//                }
//            }

//            return foundUser;
//        }

//    }
//}



using _26._02_sushi_market_back.Models;
using Microsoft.EntityFrameworkCore;

namespace _26._02_sushi_market_back.Services
{
    public class RegisterService
    {
        private readonly ApplicationContext _context;  
        private readonly ILogger<RegisterService> _logger;

      
        public RegisterService(ApplicationContext context, ILogger<RegisterService> logger)
        {
            _context = context;  
            _logger = logger;
        }


        public User Register(UserRequestReg request)
        {
            if (_context.Users.Any(u => u.UserName == request.UserName))
            {
                throw new Exception("A user with this name already exists.");
            }

            if (_context.Users.Any(u => u.Email == request.Email))
            {
                throw new Exception("A user with this email already exists.");
            }


            var user = new User
            {
                UserName = request.UserName,
                Password = request.Password, 
                Email = request.Email
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        
        public User? Login(UserRequestLog request)
        {
           
            var user = _context.Users
                .FirstOrDefault(u => u.UserName == request.UserName && u.Password == request.Password);

            return user;
        }

        
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }
    }
}

