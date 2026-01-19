using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinanceHelper.Web.Models.Finance;

public class CategoryModel
{
    public CategoryModel(Category category)
    {
        Id = category.Id;
        UserId = category.UserId;
        Name = category.Type.ToString();
        SubCategories = category.SubCategories.Select(x => new SubCategoryModel(x)).ToList();
        YearlyCost = SubCategories?.Sum(x => x.YearlyCost) ?? 0;
        MonthlyCost = SubCategories?.Sum(x => x.MonthlyCost) ?? 0;
        WeeklyCost = SubCategories?.Sum(x => x.WeeklyCost) ?? 0;
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public List<SubCategoryModel> SubCategories { get; set; }

    public decimal YearlyCost { get; set; }
    public decimal MonthlyCost { get; set; }
    public decimal WeeklyCost { get; set; }

    public IEnumerable<SelectListItem> CategoryTypeOptions { get; set; } = Enum.GetValues(typeof(PaymentFrequency)).Cast<CategoryType>().Select(ct => new SelectListItem
    {
        Value = ct.ToString(),
        Text = ct.ToString()
    });
}