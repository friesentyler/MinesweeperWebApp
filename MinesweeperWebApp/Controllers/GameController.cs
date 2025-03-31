using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures; // Make sure you have this for logging
using MinesweeperWebApp.Data;
using MinesweeperWebApp.Filters;
using MinesweeperWebApp.Models;
using ServiceStack;


public class GameController : Controller
{
    private readonly ICompositeViewEngine _viewEngine;
    private GameStorageServiceDAO _gameStorageService;

    public GameController(ICompositeViewEngine viewEngine)
    {
        _viewEngine = viewEngine;
        _gameStorageService = new GameStorageServiceDAO();
    }


    public ActionResult Index()
    {
        var board = HttpContext.Session.GetObjectFromJson<Board>("Board");
        if (board == null)
        {
            if (board == null)
            {
                board = new Board(1);
            }
        }
        return View(board);
    }

    [HttpPost]
    [SessionCheckFilter]
    public IActionResult StartGame(int difficulty)
    {
        var board = new Board(difficulty);
        HttpContext.Session.SetObjectAsJson("Board", board);
        HttpContext.Session.Remove("GameId");
        return RedirectToAction("Index");
    }

    public IActionResult ChooseDifficulty()
    {
        string? currentUser = HttpContext.Session.GetString("User");
        if (currentUser != null)
        {
            HttpContext.Session.SetString("GameStarter", currentUser);
        }
        return View();
    }

    [HttpPost]
    public ActionResult ClickCell(int x, int y)
    {
        var board = HttpContext.Session.GetObjectFromJson<Board>("Board") ?? new Board(1);
        if (!board.Cells[x, y].IsFlagged)
        {
            board.FloodFill(x, y);
            board.UpdateScore(x, y);
            HttpContext.Session.SetObjectAsJson("Board", board);
            int state = board.DetermineGameState();
            if (state == -1)
            {
                return View("Lose", board);
            }
            else if (state == 1)
            {
                return View("Win", board);
            }
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult FlagCell(int x, int y)
    {
        var board = HttpContext.Session.GetObjectFromJson<Board>("Board") ?? new Board(1);
        var cell = board.Cells[y, x];

        if (!cell.IsVisited)
        {
            cell.IsFlagged = !cell.IsFlagged;
        }
        HttpContext.Session.SetObjectAsJson("Board", board);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public ActionResult GetTimestamp()
    {
        var currentTimestamp = DateTime.Now.ToString("HH:mm:ss");
        var board = HttpContext.Session.GetObjectFromJson<Board>("Board") ?? new Board(1);
        return Content(currentTimestamp);
    }

    public ActionResult RestartGame()
    {
        HttpContext.Session.SetObjectAsJson("Board", new Board(1)); // Default difficulty 1
        return RedirectToAction("Index");
    }

    public ActionResult SaveGame()
    {
        GameStateModel model = new GameStateModel();

        model.GameData = HttpContext.Session.GetSerializedObject<Board>("Board");

        int? gameId = HttpContext.Session.GetString("GameId").ToInt(); 
        
        Console.WriteLine(gameId.ToString());

        if (gameId == 0)
        {
            string userJson = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(userJson))
            {
                var user = ServiceStack.Text.JsonSerializer.DeserializeFromString<UserModel>(userJson);
                model.UserId = user.Id;
                model.DateSaved = DateTime.Now;
            }

            _gameStorageService.saveGame(model);
        }
        else
        {
            string userJson = HttpContext.Session.GetString("User");
            if (!string.IsNullOrEmpty(userJson))
            {
                var user = ServiceStack.Text.JsonSerializer.DeserializeFromString<UserModel>(userJson);
                model.UserId = user.Id;
                model.DateSaved = DateTime.Now;
                model.Id = (int)gameId;
            }
            _gameStorageService.updateGame(model);
        }

        return RedirectToAction("Index");
    }


    [HttpPost]
    public IActionResult PartialPageUpdate(string id)
    {
        try
        {
            // Parse the ID "x-y" into two integers
            var parts = id.Split('-');
            if (parts.Length != 2 || !int.TryParse(parts[0], out int x) || !int.TryParse(parts[1], out int y))
            {
                return BadRequest("Invalid ID format. Expected format: x-y");
            }

            // Retrieve the previous board from session
            var oldBoard = HttpContext.Session.GetObjectFromJson<Board>("Board") ?? new Board(1);

            // Make a deep copy of the board before modifications (so we can compare later)
            var oldCells = oldBoard.Cells.Cast<Cell>().ToDictionary(c => $"{c.Column}-{c.Row}", c => c.Clone());

            // Apply game logic
            var board = oldBoard;
            if (!board.Cells[x, y].IsFlagged)
            {
                board.FloodFill(x, y);
                board.UpdateScore(x, y);
                HttpContext.Session.SetObjectAsJson("Board", board);
            }

            // **Find only the updated cells**
            var updatedCells = new Dictionary<string, string>();

            foreach (var cell in board.Cells)
            {
                string cellId = $"{cell.Column}-{cell.Row}";

                // **Compare new cell state with old cell state**
                if (!oldCells[cellId].Equals(cell))
                {
                    string renderedHtml = RenderPartialViewToString("_Cell", cell);
                    updatedCells[cellId] = renderedHtml;
                }
            }

            // **Check for win/loss conditions**
            int state = board.DetermineGameState();
            if (state == -1)
            {
                return Json(new { gameOver = true, redirectUrl = Url.Action("Lose", "Game") });
            }
            else if (state == 1)
            {
                return Json(new { gameOver = true, redirectUrl = Url.Action("Win", "Game") });
            }

            return Json(updatedCells);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in PartialPageUpdate: " + ex.Message);
            return StatusCode(500, "An error occurred while updating the cell.");
        }
    }


    private string RenderPartialViewToString(string viewName, object model)
    {
        if (string.IsNullOrEmpty(viewName))
            viewName = ControllerContext.ActionDescriptor.ActionName;

        ViewData.Model = model;

        using (var writer = new StringWriter())
        {
            var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);

            if (viewResult.View == null)
                throw new ArgumentException($"The view '{viewName}' was not found.");

            var viewContext = new ViewContext(
                ControllerContext,
                viewResult.View,
                ViewData,
                TempData,
                writer,
                new HtmlHelperOptions()
            );

            viewResult.View.RenderAsync(viewContext).Wait();
            return writer.GetStringBuilder().ToString();
        }
    }

    public IActionResult Win()
    {
        var board = HttpContext.Session.GetObjectFromJson<Board>("Board"); // Retrieve the board
        return View(board); // Pass the board to the view
    }

    public IActionResult Lose()
    {
        var board = HttpContext.Session.GetObjectFromJson<Board>("Board"); // Retrieve the board
        return View(board); // Pass the board to the view
    }

}

