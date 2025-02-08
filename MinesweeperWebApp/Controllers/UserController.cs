using Microsoft.AspNetCore.Mvc;
using MinesweeperWebApp.Models;
using MinesweeperWebApp.Filters;
using System.Net.Security;
using MinesweeperWebApp.Data;
using MinesweeperWebApp.Controllers;

namespace MinesweeperWebApp.Controllers
{
    public class UserController : Controller
    {
        private ValidationService _validationService = new ValidationService();
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
            return RedirectToAction("ChooseDifficulty", "Game");
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
            user.Email = registerViewModel.Email;
            user.Age = registerViewModel.Age;
            user.State = registerViewModel.State;
            user.FirstName = registerViewModel.Firstname;
            user.LastName = registerViewModel.Lastname;
            user.Sex = registerViewModel.Sex;   

            foreach (var group in registerViewModel.Groups)
            {
                if (group.IsSelected)
                {
                    user.Groups += group.GroupName + ",";
                }
            }

            user.Groups = user.Groups.TrimEnd(','); // Remove trailing comma
            string errors = _validationService.ValidateUser(user);

            if (errors == "")
            {
                users.AddUser(user);

                return View("RegisterSuccess", user);
            }
            else
            {
                RegistrationErrorViewModel errorViewModel = new RegistrationErrorViewModel(errors);
                return View("ProcessRegister", errorViewModel); 
            }
        }
    }
}
