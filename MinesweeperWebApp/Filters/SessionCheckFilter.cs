﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MinesweeperWebApp.Filters
{
    public class SessionCheckFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string GameStarter = context.HttpContext.Session.GetString("GameStarter");
            string User = context.HttpContext.Session.GetString("User");
            
            if (context.HttpContext.Session.GetString("User") == null)
            {
                context.Result = new RedirectResult("/User/Index");
            }
            else if (User != GameStarter)
            {
                context.Result = new RedirectResult("/User/Index"); // Redirect unauthorized users
                return;
            }
        }
    }
}
