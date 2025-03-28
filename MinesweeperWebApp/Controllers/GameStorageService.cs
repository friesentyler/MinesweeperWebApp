using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MinesweeperWebApp.Controllers
{
    [Route("api")]
    public class GameStorageService : Controller
    {
        public JsonResult showSavedGames(int? id)
        {
            return Json("showed");
        }

        [HttpPost]
        public JsonResult deleteOneGame(int id)
        {
            return Json("deleted");
        }
    }
}
