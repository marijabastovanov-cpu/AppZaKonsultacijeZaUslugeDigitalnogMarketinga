using Microsoft.AspNetCore.Mvc;

namespace PrezentacioniSloj.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
}
