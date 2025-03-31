using Microsoft.AspNetCore.Mvc;
using MinesweeperWebApp.Data;
using MinesweeperWebApp.Models;
using System.Net.WebSockets;

namespace MinesweeperWebApp.Controllers
{
    [ApiController]
    [Route("api/v1/games")]
    public class GameStorageRestController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<HomeController> _logger;
        private GameStorageServiceDAO _gameStorageServiceDAO = new GameStorageServiceDAO();
        private readonly IConfiguration _configuration;

        public GameStorageRestController(ILogger<HomeController> logger, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public async Task<ActionResult<IEnumerable<GameStateModel>>> ShowAllGames()
        {
            IEnumerable<GameStateModel> games = _gameStorageServiceDAO.GetAllGames();
            return Ok(games);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<GameStateModel>> GetGameById(int id)
        {
            var game = _gameStorageServiceDAO.GetGameById(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = _gameStorageServiceDAO.GetGameById(id);
            if (game == null)
            {
                return NotFound(); // 404 if the product doesn't exist
            }

            _gameStorageServiceDAO.deleteGameById(id);

            return NoContent(); // 204 No Content, standard for successful DELETE
        }
    }
}
