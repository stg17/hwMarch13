using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hwMarch13.Data
{
    public class AdRepository
    {
        private readonly string _connectionString;
        public AdRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Ad> GetAllAds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Ads";
            connection.Open();
            List<Ad> ads = new();
            var reader = command.ExecuteReader();
            while(reader.Read())
            {
                var ad = new Ad()
                {
                    Id = (int)reader["id"],
                    date = (DateTime)reader["date"],
                    Description = (string)reader["description"],
                    PhoneNumber = (int)reader["Phonenumber"],
                    UserId = (int)reader["userid"]
                };
                ads.Add(ad);
            }
            return ads;
        }

        public void AddAd(Ad ad)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Ads VALUES (@date, @phone, @desc, @userId) SELECT SCOPE_IDENTITY()";
            command.Parameters.AddWithValue("@date", DateTime.Today);
            command.Parameters.AddWithValue("@phone", ad.PhoneNumber);
            command.Parameters.AddWithValue("@desc", ad.Description);
            command.Parameters.AddWithValue("userId", ad.UserId);
            connection.Open();
            ad.Id = (int)(decimal)command.ExecuteScalar();
        }

        public void Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Ads WHERE ID = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
