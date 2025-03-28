using Microsoft.Data.SqlClient;
using MinesweeperWebApp.Models;

namespace MinesweeperWebApp.Data
{
    public class GameStorageServiceDAO : IGameStorageServiceManager
    {
        private string _connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MineSweeperWebDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public void deleteGameById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Users WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public List<GameStateModel> GetAllGames()
        {
            List<GameStateModel> gameStates = new List<GameStateModel>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM GameState";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    GameStateModel gameState = new GameStateModel
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                        DateSaved = reader.GetDateTime(reader.GetOrdinal("DateSaved")),
                        GameData = reader.GetString(reader.GetOrdinal("GameData")),
                    };
                    gameStates.Add(gameState);
                }
            }
            return gameStates;

        }

        public GameStateModel GetGameById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM GameState WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new GameStateModel 
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                        DateSaved = reader.GetDateTime(reader.GetOrdinal("DateSaved")),
                        GameData = reader.GetString(reader.GetOrdinal("GameData")),
                    };
                }
            }
            return null;
        }
    }
}
