using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MinesweeperWebApp.Controllers
{
    public class GameStorageController : Controller
    {
        [Route("api")]
        public JsonResult ShowSavedGames(int? id)
        {
            return Json("showed");
        }

        [HttpPost]
        [Route("api")]
        public JsonResult DeleteOneGame(int id)
        {
            return Json("deleted");
        }

        public IActionResult ShowSavedGamesView()
        {
            return View();
        }
    }
}
