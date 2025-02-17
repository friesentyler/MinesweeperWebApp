﻿using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using MinesweeperWebApp.Models;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.CSharp.Syntax;


public class GameController : Controller
{

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
    public IActionResult StartGame(int difficulty)
    {
        var board = new Board(difficulty);
        HttpContext.Session.SetObjectAsJson("Board", board);
        return RedirectToAction("Index", board);
    }

    public IActionResult ChooseDifficulty()
    {
        return View();
    }

    [HttpPost]
    public ActionResult ClickCell(int x, int y)
    {
        var board = HttpContext.Session.GetObjectFromJson<Board>("Board") ?? new Board(1);
        board.FloodFill(x, y);
        board.UpdateScore(x, y);
        HttpContext.Session.SetObjectAsJson("Board", board);
        int state = board.DetermineGameState();
        Console.WriteLine(state);
        if (state == -1)
        {
            return View("Lose", board);
        }
        else if (state == 1)
        {
            return View("Win", board);
        }
        
        return RedirectToAction("Index");
    }

    public ActionResult RestartGame()
    {
        HttpContext.Session.SetObjectAsJson("Board", new Board(1)); // Default difficulty 1
        return RedirectToAction("Index");
    }
}

