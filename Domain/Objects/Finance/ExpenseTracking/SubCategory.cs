using FinanceHelper.Domain.Objects.Base;

namespace FinanceHelper.Domain.Objects.Finance.ExpenseTracking;

public enum SubCategoryType
{
    Expense = 1,
    Income = 2
}

public enum PaymentFrequency
{
    Other = 0,
    Daily = 1,
    Weekly = 2,
    EveryTwoWeeks = 3,
    Monthly = 4,
    Quarterly = 5,
    Quadrimester = 6,
    SemiAnnual = 7,
    Yearly = 8
}

public class SubCategory : BaseEntity
{
    public SubCategoryType SubCategoryType { get; set; }
    public PaymentFrequency PayFrequency { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public decimal YearlyCost { get; set; }
    public decimal MonthlyCost { get; set; }
    public decimal WeeklyCost { get; set; }
}