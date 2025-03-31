using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinesweeperWebApp.Data;
using MinesweeperWebApp.Filters;
using MinesweeperWebApp.Models;
using Newtonsoft.Json;

namespace MinesweeperWebApp.Controllers
{
    public class GameStorageController : Controller
    {
        private GameStorageServiceDAO _gameStorageServiceDAO = new GameStorageServiceDAO();

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _gameStorageServiceDAO.deleteGameById(id);

            return RedirectToAction("ShowSavedGamesView");
        }

        [SessionCheckFilter]
        public IActionResult ShowSavedGamesView()
        {
            List<GameStateModel> list = new List<GameStateModel>();
            list = _gameStorageServiceDAO.GetAllGames();
            return View(list);
        }

        [HttpPost]
        public IActionResult ResumeGame(int id)
        {
            // Step 1: Get the saved game from DB
            GameStateModel game = _gameStorageServiceDAO.GetGameById(id);

            if (game == null)
            {
                return NotFound();
            }

            // Step 2: Get the current user's ID from session or auth
            UserModel user = HttpContext.Session.GetObjectFromJson<UserModel>("User"); // adjust as needed

            if (game.UserId != user.Id)
            {
                return View("MismatchedUser");
            }

            // Step 3: Deserialize and store board in session
            var board = JsonConvert.DeserializeObject<Board>(game.GameData);
            HttpContext.Session.SetObjectAsJson("Board", board);

            HttpContext.Session.SetString("GameId", game.Id.ToString());

            // Step 4: Redirect to game
            return RedirectToAction("Index", "Game");
        }


    }
}
