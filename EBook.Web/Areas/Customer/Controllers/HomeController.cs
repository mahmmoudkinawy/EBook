﻿using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EBook.Web.Areas.Customer.Controllers;
public class HomeController : BaseCustomerController
{
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public IActionResult Index()
        => View(_unitOfWork.ProductRepository.GetAll(includeProperties: "Category,CoverType"));

    public IActionResult Details(int productId)
    {
        var shoppingCart = new ShoppingCart
        {
            Product = _unitOfWork.ProductRepository.GetFirstOrDefault(p => p.Id == productId,
            includeProperties: "Category,CoverType"),
            Count = 1
        };

        return View(shoppingCart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        shoppingCart.AppUserId = claim.Value;

        var cart = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(
                u => u.AppUserId == claim.Value && u.ProductId == shoppingCart.ProductId
            );

        if (cart == null)
            _unitOfWork.ShoppingCartRepository.Add(shoppingCart);
        else
            _unitOfWork.ShoppingCartRepository.IncrementCount(cart, shoppingCart.Count);

        _unitOfWork.Save();

        return RedirectToAction(nameof(Index));
    }
}

