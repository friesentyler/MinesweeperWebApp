using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using MinesweeperWebApp.Models;

namespace MinesweeperWebApp.Controllers
{
    public class ButtonController : Controller
    {
        static List<ButtonModel> buttons = new List<ButtonModel>();

        string[] buttonImages = ["bluebtn.png", "redbtn.png", "orangebtn.png", "smileybtn.png"];

        static bool allSameColor = false;

        public ButtonController()
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
            var viewModel = new ButtonViewModel
            {
                Buttons = buttons,
                AllSameColor = allSameColor,
            };
            return View(viewModel);
        }
    }
}
