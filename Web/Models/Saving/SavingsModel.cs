using FinanceHelper.Domain.Objects.Accounts;

namespace FinanceHelper.Web.Models.Saving
{
    public class SavingsModel
    {
        public List<SavingAccountModel> Savings { get; set; } = new();
        public decimal TotalGross { get; set; }
        public decimal TotalNet { get; set; }

        public void CalculateTotals()
        {
            TotalGross = Savings.Sum(s => s.GrossBalance);
            TotalNet = Savings.Sum(s => s.NetBalance);
        }
    }
}
