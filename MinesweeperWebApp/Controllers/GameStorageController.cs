using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MinesweeperWebApp.Controllers
{
    public class GameStorageController : Controller
    {
        [Route("api/showSavedGames")]
        public IActionResult showSavedGames(int? id)
        {
            return Json("showed");
        }

        [HttpPost]
        [Route("api/deleteOneGame")]
        public IActionResult deleteOneGame(int id)
        {
            return Json("deleted");
        }

        public IActionResult ShowSavedGamesView()
        {
            return View();
        }
    }
}
