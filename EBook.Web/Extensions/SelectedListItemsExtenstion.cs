namespace EBook.Web.Extensions;
public static class SelectedListItemsExtenstion
{
    public static IEnumerable<SelectListItem> CategoryList(this IUnitOfWork unitOfWork)
        => unitOfWork.CategoryRepository.GetAll().Select(c => new SelectListItem
        {
            Text = c.Name,
            Value = c.Id.ToString()
        });

    public static IEnumerable<SelectListItem> CoverTypeList(this IUnitOfWork unitOfWork)
        => unitOfWork.CoverTypeRepository.GetAll().Select(c => new SelectListItem
        {
            Text = c.Name,
            Value = c.Id.ToString()
        });

}
