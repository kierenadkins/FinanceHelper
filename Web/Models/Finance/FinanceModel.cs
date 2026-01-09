namespace Web.Models.Finance
{
    public class FinanceModel
    {
        public SalaryModel? Salary { get; set; }
        public List<CategoryModel> Category { get; set; }

        public decimal YearlyTotalOutGoings { get; set; }
        public decimal MonthlyOutGoings { get; set; }

        public decimal YearlyLeftOver { get; set; }
        public decimal MonthlyLeftOver { get; set; }
    }
}
