namespace FinanceHelper.Web.Models.Finance;

public class SalaryInputModel
{
    public int? SalaryId { get; set; }
    public decimal GrossSalary { get; set; }

    public bool IsEdit => SalaryId.HasValue;
}
