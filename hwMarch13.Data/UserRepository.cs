using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hwMarch13.Data
{
    public class UserRepository
    {
        private readonly string _connectionString;
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user, string password)
        {
            user.HashPassword = BCrypt.Net.BCrypt.HashPassword(password);
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Users VALUES (@name, @email, @pswd) SELECT SCOPE_IDENTITY()";
            command.Parameters.AddWithValue("@name", user.Name);
            command.Parameters.AddWithValue("@email", user.Email);
            command.Parameters.AddWithValue("@pswd", user.HashPassword);
            connection.Open();
            user.Id = (int)(decimal)command.ExecuteScalar();
        }

        public User GetUserById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Users WHERE ID = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            return new User()
            {
                Email = (string)reader["email"],
                HashPassword = (string)reader["hashpassword"],
                Id = (int)reader["id"],
                Name = (string)reader["Name"]
            };
        }

        public User GetByEmail(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Users WHERE Email = @email";
            command.Parameters.AddWithValue("@email", email);
            connection.Open();
            var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            return new User()
            {
                Email = (string)reader["email"],
                HashPassword = (string)reader["hashpassword"],
                Id = (int)reader["id"],
                Name = (string)reader["Name"]
            };
        }

        public User Login(string email, string password)
        {
            var user = GetByEmail(email);
            if (user == null)
            {
                return null;
            }

            var isMatch = BCrypt.Net.BCrypt.Verify(password, user.HashPassword);
            if (!isMatch)
            {
                return null;
            }

            return user;
        }
    }
}
