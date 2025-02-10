
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
                    INSERT INTO Users (Username, PasswordHash, Salt, Groups, Email, Sex, State,      FirstName,  LastName, Age)
                    VALUES (@Username, @PasswordHash, @Salt, @Groups, @Email, @Sex, @State,     @FirstName,      @LastName, @Age);
                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@Salt", user.Salt);
                    command.Parameters.AddWithValue("@Groups", user.Groups ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Sex", user.Sex ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@State", user.State ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", user.LastName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Age", user.Age);

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
                        Groups = reader.IsDBNull(reader.GetOrdinal("Groups")) ? null : reader.GetString(reader.GetOrdinal("Groups")),
                        Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                        Sex = reader.IsDBNull(reader.GetOrdinal("Sex")) ? null : reader.GetString(reader.GetOrdinal("Sex")),
                        State = reader.IsDBNull(reader.GetOrdinal("State")) ? null : reader.GetString(reader.GetOrdinal("State")),
                        FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                        Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? 0 : reader.GetInt32(reader.GetOrdinal("Age"))
                    };

                    bool valid = user.VerifyPassword(password);
                    return valid ? user.Id : 0;
                }
                return 0; // User not found
            }
        }


        public void DeleteUser(UserModel user)
        {
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
                        Groups = reader.IsDBNull(reader.GetOrdinal("Groups")) ? null : reader.GetString(reader.GetOrdinal("Groups")),
                        Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                        Sex = reader.IsDBNull(reader.GetOrdinal("Sex")) ? null : reader.GetString(reader.GetOrdinal("Sex")),
                        State = reader.IsDBNull(reader.GetOrdinal("State")) ? null : reader.GetString(reader.GetOrdinal("State")),
                        FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                        Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? 0 : reader.GetInt32(reader.GetOrdinal("Age"))
                    };
                    users.Add(user);
                }
            }
            return users;
        }


        public UserModel GetUserById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new UserModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = (byte[])reader["Salt"],
                        Groups = reader.IsDBNull(reader.GetOrdinal("Groups")) ? null : reader.GetString(reader.GetOrdinal("Groups")),
                        Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                        Sex = reader.IsDBNull(reader.GetOrdinal("Sex")) ? null : reader.GetString(reader.GetOrdinal("Sex")),
                        State = reader.IsDBNull(reader.GetOrdinal("State")) ? null : reader.GetString(reader.GetOrdinal("State")),
                        FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                        Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? 0 : reader.GetInt32(reader.GetOrdinal("Age"))
                    };
                }
            }
            return null;
        }


        public void UpdateUser(UserModel user)
        {
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
                            Groups = @Groups,
                            Email = @Email,
                            Sex = @Sex,
                            State = @State,
                            FirstName = @FirstName,
                            LastName = @LastName,
                            Age = @Age
                        WHERE Id = @Id";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@Salt", user.Salt);
                    command.Parameters.AddWithValue("@Groups", user.Groups ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Sex", user.Sex ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@State", user.State ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@FirstName", user.FirstName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", user.LastName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Age", user.Age);
                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public UserModel GetUserByUsername(string userName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Username = @Username";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", userName);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) // If a match is found
                {
                    return new UserModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = (byte[])reader["Salt"],
                        Groups = reader.IsDBNull(reader.GetOrdinal("Groups")) ? null : reader.GetString(reader.GetOrdinal("Groups")),
                        Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                        Sex = reader.IsDBNull(reader.GetOrdinal("Sex")) ? null : reader.GetString(reader.GetOrdinal("Sex")),
                        State = reader.IsDBNull(reader.GetOrdinal("State")) ? null : reader.GetString(reader.GetOrdinal("State")),
                        FirstName = reader.IsDBNull(reader.GetOrdinal("FirstName")) ? null : reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.IsDBNull(reader.GetOrdinal("LastName")) ? null : reader.GetString(reader.GetOrdinal("LastName")),
                        Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? 0 : reader.GetInt32(reader.GetOrdinal("Age"))
                    };
                }
            }
            return null; // No matching user found
        }

    }
}
