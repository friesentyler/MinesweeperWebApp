using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using MinesweeperWebApp.Models;
using Newtonsoft.Json.Linq;
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
    /*
    public Board CurrentBoard
    {

        get
        {
            var board = HttpContext.Session.GetObjectFromJson<Board>("Board");
            if (board == null)
            {
                board = new Board(1); // Default difficulty 1
            }
            return board;
        }
        set
        {
            HttpContext.Session.SetObjectAsJson("Board", value);
        }
    }
    */

    public ActionResult Index()
    {
        var board = HttpContext.Session.GetObjectFromJson<Board>("Board");
        if (board == null)
        {
            board = new Board(1);
            HttpContext.Session.SetObjectAsJson("Board", board);
        }
        return View(board);
    }

    [HttpPost]
    public ActionResult ClickCell(int x, int y)
    {
        var board = HttpContext.Session.GetObjectFromJson<Board>("Board") ?? new Board(1);
        board.FloodFill(x, y);
        HttpContext.Session.SetObjectAsJson("Board", board);
        return RedirectToAction("Index"); // ✅ Redirect instead of returning the View directly
    }

    public ActionResult RestartGame()
    {
        HttpContext.Session.SetObjectAsJson("Board", new Board(1)); // Default difficulty 1
        return RedirectToAction("Index");
    }
}

