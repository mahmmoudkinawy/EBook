using EBook.Web.Data;
using EBook.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EBook.Web.Controllers;
public class CategoryController : BaseController
{
    private readonly DataContext _context;
    public CategoryController(DataContext context) => _context = context;

    //GET all data
    public IActionResult Index() => View(_context.Categories.ToList());

    //GET the View
    public IActionResult Create() => View();

    //POST the model
    [HttpPost]
    public IActionResult Create(Category category)
    {
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "Name cannot be same as Display Order");
        }
        if (ModelState.IsValid)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(category);
    }

}

