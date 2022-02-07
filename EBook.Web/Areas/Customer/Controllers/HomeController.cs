namespace EBook.Web.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

