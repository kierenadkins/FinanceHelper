using Application.Domain.Base;

namespace Application.Domain.Finance.ExpenseTracking;

public enum SubCategoryType
{
    Expense = 1,
    Income = 2
}

public class SubCategory : BaseEntity
{
    public SubCategoryType SubCategoryType { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public decimal YearlyCost { get; set; }
    public decimal MonthlyCost { get; set; }
    public decimal WeeklyCost { get; set; }
}