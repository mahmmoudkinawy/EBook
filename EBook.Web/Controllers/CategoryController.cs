using EBook.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace EBook.Web.Controllers;
public class CategoryController : BaseController
{
    private readonly DataContext _context;
    public CategoryController(DataContext context) => _context = context;

    //GET all data
    public IActionResult Index() => View(_context.Categories.ToList());

}

