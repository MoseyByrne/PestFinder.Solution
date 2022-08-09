using Microsoft.AspNetCore.Mvc;

namespace PestFinder.controller
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      ViewBag.Title = "Pest Finder";
      ViewBag.Subtitle = "Home";
      return View();
    }
  }
}