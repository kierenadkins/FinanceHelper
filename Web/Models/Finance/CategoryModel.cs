using Application.Domain.Finance.ExpenseTracking;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Models.Finance;

public class CategoryModel
{
    public CategoryModel(Category category)
    {
        Id = category.Id;
        UserId = category.UserId;
        Name = category.Type.ToString();
        SubCategories = category.SubCategories.Select(x => new SubCategoryModel(x)).ToList();
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public List<SubCategoryModel> SubCategories { get; set; }

    public IEnumerable<SelectListItem> CategoryTypeOptions { get; set; } = Enum.GetValues(typeof(PaymentFrequency)).Cast<CategoryType>().Select(ct => new SelectListItem
    {
        Value = ct.ToString(),
        Text = ct.ToString()
    });
}