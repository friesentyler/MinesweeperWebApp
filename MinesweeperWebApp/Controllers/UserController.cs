using Microsoft.AspNetCore.Mvc;
using MinesweeperWebApp.Models;
using MinesweeperWebApp.Filters;
using System.Net.Security;

namespace RegisterAndLoginApp.Controllers
{
    public class UserController : Controller
    {
        private IUserManager users;
        public UserController(IUserManager userManager)
        {
            users = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessLogin(LoginViewModel loginViewModel)
        {
            Console.WriteLine("Hit");
            var result = users.CheckCredentials(loginViewModel.Username, loginViewModel.Password);

            if (result > 0)
            {
                var user = users.GetUserById(result);
                string userJson = ServiceStack.Text.JsonSerializer.SerializeToString(user);
                HttpContext.Session.SetString("User", userJson);
                return View("LoginSuccess", user);
            }
            else
            {
                return View("LoginFailure");
            }
        }

        [SessionCheckFilter]
        public IActionResult Game()
        {
            return View();
        }

        [AdminCheckFilter]
        public IActionResult AdminOnly()
        {
            List<UserModel> allUsers = users.GetAllUsers();
            return View(allUsers);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "User");
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        public IActionResult ProcessRegister(RegisterViewModel registerViewModel)
        {
            UserModel user = new UserModel();
            user.Username = registerViewModel.Username;
            user.SetPassword(registerViewModel.Password);
            user.Groups = "";

            foreach (var group in registerViewModel.Groups)
            {
                if (group.IsSelected)
                {
                    user.Groups += group.GroupName + ",";
                }
            }

            user.Groups = user.Groups.TrimEnd(','); // Remove trailing comma
            users.AddUser(user);

            return View("Index");
        }
    }
}
