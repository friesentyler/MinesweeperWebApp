namespace MinesweeperWebApp.Models
{
    public interface IGameStorageServiceManager
    {
        public List<GameStateModel> GetAllGames();
        public GameStateModel GetGameById(int id);
        public void deleteGameById(int id);
    }
}
