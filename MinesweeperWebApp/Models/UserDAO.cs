using MySql.Data.MySqlClient;

namespace MinesweeperWebApp.Models
{
    public class UsersDAO : IUserManager
    {
        //string connectionString = @"Data Source=172.24.94.226,8889;Initial Catalog=Test;User ID=tyler;Password=tyler;Encrypt=True;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        string connectionString = "datasource=172.24.94.226;port=8889;username=tyler;password=tyler;database=Test;";

        // from databases class
        //string connectionString = "datasource=172.24.94.226;port=8889;username=tyler;password=tyler;database=sys;";
        public int AddUser(UserModel user)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO users (Username, PasswordHash, Salt, `Groups`)
                VALUES (@Username, @PasswordHash, @Salt, @Groups);
                SELECT LAST_INSERT_ID();";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@Salt", user.Salt);
                    command.Parameters.AddWithValue("@Groups", user.Groups);

                    object result = command.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
        }

        public int CheckCredentials(string username, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Username = @Username";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        //Salt = reader.GetString(reader.GetOrdinal("Salt")),
                        Salt = reader["Salt"] as byte[],
                        Groups = reader.GetString(reader.GetOrdinal("Groups"))
                    };
                    bool valid = user.VerifyPassword(password);
                    if (valid)
                    {
                        return user.Id;
                    }
                    else
                    {
                        return 0;
                    }
                }
                return 0;
            }
        }

        public UserModel GetUserByUsername(string username)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Username = @Username";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = reader["Salt"] as byte[],
                        Groups = reader.GetString(reader.GetOrdinal("Groups"))
                    };
                    return user;
                }
                return null;
            }
        }

        public void DeleteUser(UserModel user)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Users WHERE Id = @Id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.ExecuteNonQuery();
            }
        }

        public List<UserModel> GetAllUsers()
        {
            List<UserModel> users = new List<UserModel>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        //Salt = reader.GetString(reader.GetOrdinal("Salt")),
                        Salt = reader["Salt"] as byte[],
                        Groups = reader.GetString(reader.GetOrdinal("Groups"))
                    };
                    users.Add(user);
                }
            }
            return users;
        }

        public UserModel GetUserById(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Id = @Id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserModel user = new UserModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        //Salt = reader.GetString(reader.GetOrdinal("Salt")),
                        Salt = reader["Salt"] as byte[],
                        Groups = reader.GetString(reader.GetOrdinal("Groups"))
                    };
                    return user;
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
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE Users SET Username = @Username, PasswordHash = @PasswordHash, Salt = @Salt, Groups = @Groups WHERE Id = @Id";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@Salt", user.Salt);
                    command.Parameters.AddWithValue("@Groups", user.Groups);
                }
            }
        }
    }
}
