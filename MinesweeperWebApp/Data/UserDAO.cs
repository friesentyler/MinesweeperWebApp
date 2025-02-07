
using Microsoft.Data.SqlClient;
using MinesweeperWebApp.Models;

namespace MinesweeperWebApp.Data
{
    public class UserDAO : IUserManager
    {
        private string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MineSweeperWebDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public int AddUser(UserModel user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                INSERT INTO Users (Username, PasswordHash, Salt, Groups)
                VALUES (@Username, @PasswordHash, @Salt, @Groups);
                SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@Salt", user.Salt);
                    command.Parameters.AddWithValue("@Groups", user.Groups);

                    // Execute the query and get the newly inserted ID
                    object result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }


        public int CheckCredentials(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Find a user with the given username
                string query = "SELECT * FROM Users WHERE Username = @Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                SqlDataReader reader = command.ExecuteReader();

                // If the user is found, verify the password hash and return the user ID
                if (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = (byte[])reader["Salt"],
                        Groups = reader.GetString(reader.GetOrdinal("Groups"))
                    };

                    bool valid = user.VerifyPassword(password);
                    if (valid == true)
                    {
                        return user.Id; // User found and password is correct
                    }
                    else
                    {
                        return 0; // User found but password is incorrect
                    }
                }
                return 0; // User not found
            }
        }


        public void DeleteUser(UserModel user)
        {
            // Delete the matching user record
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Users WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.ExecuteNonQuery();
            }
        }


        public List<UserModel> GetAllUsers()
        {
            // Search for all users
            List<UserModel> users = new List<UserModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = (byte[])reader["Salt"],
                        Groups = reader.GetString(reader.GetOrdinal("Groups"))
                    };
                    users.Add(user);
                }
            }
            return users;
        }


        public UserModel GetUserById(int id)
        {
            // Find the matching ID number
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = (byte[])reader["Salt"],
                        Groups = reader.GetString(reader.GetOrdinal("Groups"))
                    };
                    return user; // Return the matching user
                }
            }
            return null; // No matching user found
        }


        public void UpdateUser(UserModel user)
        {
            // Find the matching user. If found, update the record using the new data
            // ID numbers do not change; all other fields can be updated.
            int id = user.Id;
            UserModel found = GetUserById(id);
            if (found != null)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = @"
            UPDATE Users 
            SET Username = @Username, 
                PasswordHash = @PasswordHash, 
                Salt = @Salt, 
                Groups = @Groups 
            WHERE Id = @Id";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@Salt", user.Salt);
                    command.Parameters.AddWithValue("@Groups", user.Groups);
                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public UserModel GetUserByUsername(string userName)
        {
            UserModel user = null;
            return user;
        }

    }
}
