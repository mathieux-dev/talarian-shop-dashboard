using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TolarianShop.Models;

namespace TolarianShop.Controllers;

public class HomePageController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        ViewData["ActivePage"] = "Dashboard";
        return View();
    }

    [HttpGet("notas-emitidas")]
    public IActionResult NotasEmitidas()
    {
        ViewData["ActivePage"] = "NotasEmitidas";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [HttpGet("error")]
    public IActionResult Error()
    {
        var errorViewModel = new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        };
        return View(errorViewModel);
    }
}
