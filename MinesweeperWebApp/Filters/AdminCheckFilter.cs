using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MinesweeperWebApp.Models;

namespace MinesweeperWebApp.Filters
{
    public class AdminCheckFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string userInfo = context.HttpContext.Session.GetString("User");

            if (userInfo == null)
            {
                // No user info in session. Redirect to login page
                context.Result = new RedirectResult("/User/Index");
                return;
            }

            // Part 2 - check to see if the user is an admin
            try
            {
                UserModel user = ServiceStack.Text.JsonSerializer.DeserializeFromString<UserModel>(userInfo);
                if (user.Groups.Contains("Admin") == false)
                {
                    // User is not an admin. Redirect to login page
                    context.Result = new RedirectResult("/User/Index");
                    return;
                }
            }
            catch
            {
                // Error in deserialization of the session information. Redirect to login page
                context.Result = new RedirectResult("/User/Index");
                return;
            }
        }
    }

}
