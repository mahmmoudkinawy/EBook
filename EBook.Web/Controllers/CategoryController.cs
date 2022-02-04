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
    [ValidateAntiForgeryToken]
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

    //GET edit view
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0) return NotFound();

        var category = _context.Categories.Find(id);

        if (category == null) return NotFound();

        return View(category);
    }

    //POST data
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category category)
    {
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name.");
        }
        if (ModelState.IsValid)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(category);
    }

    //GET delete VIEW
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0) return NotFound();

        var category = _context.Categories.Find(id);

        if (category == null) return NotFound();

        return View(category);
    }

    //POST deleted data
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        if (id == null || id == 0) return NotFound();

        var category = _context.Categories.Find(id);

        if (category == null) return NotFound();

        _context.Categories.Remove(category);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }


}

