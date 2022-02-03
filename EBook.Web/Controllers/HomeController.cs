using Microsoft.AspNetCore.Mvc;

namespace EBook.Web.Controllers;
public class HomeController : BaseController
{
    public IActionResult Index()
    {
        return View();
    }
}

