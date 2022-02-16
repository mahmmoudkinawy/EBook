namespace EBook.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = Constants.RoleAdmin + "," + Constants.RoleEmployee)]
public abstract class BaseAdminController : Controller
{
}

