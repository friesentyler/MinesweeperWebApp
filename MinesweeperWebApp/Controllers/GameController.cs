using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using MinesweeperWebApp.Models;
/*
namespace MinesweeperWebApp.Controllers
{
    public class GameController : Controller
    {
        static List<ButtonModel> buttons = new List<ButtonModel>();

        string[] buttonImages = ["bluebtn.png", "redbtn.png", "orangebtn.png", "smileybtn.png"];

        static bool allSameColor = false;

        public GameController()
        {
            // Create a set of buttons with random images
            if (buttons.Count == 0)
            {
                for (int i = 0; i < 25; i++)
                {
                    int number = RandomNumberGenerator.GetInt32(0, 4);
                    buttons.Add(new ButtonModel(i, number, buttonImages[number]));
                }
            }
        }

        public IActionResult ButtonClick(int id)
        {
            ButtonModel button = buttons.FirstOrDefault(b => b.Id == id);
            if (button != null)
            {
                button.ButtonState = (button.ButtonState + 1) % buttonImages.Length;
                button.ButtonImage = buttonImages[button.ButtonState];
            }

            // Check if all buttons have the same color
            allSameColor = buttons.Select(b => b.ButtonImage).Distinct().Count() == 1;

            return RedirectToAction("Index");
        }


        public IActionResult Index()
        {
            var board = new Board(1);
            return View(board);
        }
    }
}*/

public class GameController : Controller
{
    public Board CurrentBoard
    {
        get
        {
            var board = HttpContext.Session.GetObjectFromJson<Board>("Board");
            if (board == null)
            {
                board = new Board(1); // Default difficulty 1
            }
            board.UnflattenCells(); // Convert the list back to the 2D array
            return board;
        }
        set
        {
            value.FlattenCells(); // Convert the 2D array to a list before saving
            HttpContext.Session.SetObjectAsJson("Board", value);
        }
    }

    public ActionResult Index()
    {
        var board = CurrentBoard;
        return View(board);
    }

    [HttpPost]
    public ActionResult ClickCell(int x, int y)
    {
        var board = CurrentBoard;
        board.FloodFill(x, y);

        var gameState = board.DetermineGameState();
        return View("Index", board);
    }

    public ActionResult RestartGame()
    {
        HttpContext.Session.SetObjectAsJson("Board", new Board(1)); // Default difficulty 1
        return RedirectToAction("Index");
    }
}

