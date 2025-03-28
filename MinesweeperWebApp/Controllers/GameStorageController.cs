using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MinesweeperWebApp.Controllers
{
    public class GameStorageController : Controller
    {
        [Route("api")]
        public JsonResult showSavedGames(int? id)
        {
            return Json("showed");
        }

        [HttpPost]
        [Route("api")]
        public JsonResult deleteOneGame(int id)
        {
            return Json("deleted");
        }

        public IActionResult showSavedGamesView()
        {
            return View();
        }
    }
}
