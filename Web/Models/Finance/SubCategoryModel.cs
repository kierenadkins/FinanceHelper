using Application.Domain.Finance.ExpenseTracking;

namespace Web.Models.Finance;

public class SubCategoryModel(SubCategory subCategory)
{
    public int Id { get; set; } = subCategory.Id;
    public SubCategoryType SubCategoryType { get; set; } = subCategory.SubCategoryType;
    public string Name { get; set; } = subCategory.Name;
    public decimal YearlyCost { get; set; } = subCategory.YearlyCost;
    public decimal MonthlyCost { get; set; } = subCategory.MonthlyCost;
    public decimal WeeklyCost { get; set; } = subCategory.WeeklyCost;
}