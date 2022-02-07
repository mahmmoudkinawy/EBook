﻿namespace EBook.Models.ViewModels;
public class ProductViewModel
{
    public Product Product { get; set; }
    public IEnumerable<SelectListItem> CategoryList { get; set; }
    public IEnumerable<SelectListItem> CoverTypeList { get; set; }
}

