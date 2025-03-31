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
                string query = "DELETE FROM GameState WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }

        public void updateGame(GameStateModel gameState)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "UPDATE GameState SET userId = @UserId, dateSaved = @DateSaved, gameState = @GameData WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", gameState.Id);
                    command.Parameters.AddWithValue("@UserId", gameState.UserId);
                    command.Parameters.AddWithValue("@DateSaved", gameState.DateSaved);
                    command.Parameters.AddWithValue("@GameData", gameState.GameData);

                    command.ExecuteNonQuery(); // No result expected
                }
            }
        }


        public void saveGame(GameStateModel gameState)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO GameState (userId, dateSaved, gameState) VALUES (@UserId, @DateSaved, @GameData); SELECT SCOPE_IDENTITY()";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", gameState.UserId);
                    command.Parameters.AddWithValue("@DateSaved", gameState.DateSaved);
                    command.Parameters.AddWithValue("@GameData", gameState.GameData);

                    // Execute the query and get the newly inserted ID
                    object result = command.ExecuteScalar();
                }
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
                        UserId = reader.GetInt32(reader.GetOrdinal("userId")),
                        DateSaved = reader.GetDateTime(reader.GetOrdinal("dateSaved")),
                        GameData = reader.GetString(reader.GetOrdinal("gameState")),
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
                        UserId = reader.GetInt32(reader.GetOrdinal("userId")),
                        DateSaved = reader.GetDateTime(reader.GetOrdinal("dateSaved")),
                        GameData = reader.GetString(reader.GetOrdinal("gameState")),
                    };
                }
            }
            return null;
        }
    }
}
