using Microsoft.AspNetCore.Mvc;
using RegisterAndLoginApp.Models;

namespace MinesweeperWebApp.Controllers
{
    public class UserController : Controller
    {
        private IUserManager users;

        public IActionResult Index()
        {
            return View();
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
            user.Groups = user.Groups.TrimEnd(',');
            users.AddUser(user);

            return View("Index");
        }
    }
}
