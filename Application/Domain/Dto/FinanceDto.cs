using FinanceHelper.Application.Extensions.Numerics;
using FinanceHelper.Domain.Objects.Finance;
using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;

namespace FinanceHelper.Application.Domain.Dto;

public class FinanceDto
{
    public FinanceDto(Salary salary, List<Category> categoriesWithSubCats)
    {
        Salary = salary;
        CategoriesWithSubCats = categoriesWithSubCats;

        //i think this will be fine for now but if we need more calculations maybe seperate into a finance calculator class
        YearlyTotalOutGoings = CategoriesWithSubCats.SelectMany(cat => cat.SubCategories).Where(sub => sub.SubCategoryType == SubCategoryType.Expense).Sum(sub => sub.YearlyCost);
        MonthlyOutGoings = YearlyTotalOutGoings.YearlyToMonthly();
        YearlyLeftOver = Salary.NetSalary - YearlyTotalOutGoings;
        MonthlyLeftOver = (Salary.NetSalary.YearlyToMonthly() - MonthlyOutGoings).To2DP();

    }

    public Salary Salary { get; set; }
    public List<Category> CategoriesWithSubCats { get; set; } = new();

    public decimal YearlyTotalOutGoings { get; set; } 
    public decimal MonthlyOutGoings { get; set; }

    public decimal YearlyLeftOver { get; set; }
    public decimal MonthlyLeftOver { get; set; }
}