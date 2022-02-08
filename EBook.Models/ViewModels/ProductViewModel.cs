namespace EBook.Models.ViewModels;
public class ProductViewModel
{
    public Product Product { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem> CategoryList { get; set; }

    [ValidateNever]
    public IEnumerable<SelectListItem> CoverTypeList { get; set; }
}

