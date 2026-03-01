using FinanceHelper.Domain.Objects.Accounts;

namespace FinanceHelper.Web.Models.Saving
{
    public class SavingsModel
    {
        public List<SavingAccountProgressionModel> Savings { get; set; } = new();
        public decimal TotalBalance { get; set; }
        public decimal TotalWithInterest { get; set; }
        public decimal TotalInterestEarned { get; set; }

        public void CalculateTotals()
        {
            TotalBalance = Savings.Sum(s => s.Balance);
            TotalWithInterest = Savings.Sum(s => s.Balance + s.TotalInterestEarned);
            TotalInterestEarned = Savings.Sum(s => s.TotalInterestEarned);
        }
    }

    public class SavingAccountProgressionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public AccountType AccountType { get; set; }
        public decimal InterestRate { get; set; }
        public string InterestType { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalInterestEarned { get; set; }
        public List<ProgressionPoint> ProgressionHistory { get; set; } = new();
        public DateTime LastInterestDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public decimal GetProjectedInterest(int monthsAhead) 
        {
            if (string.IsNullOrEmpty(InterestType) || InterestType == "None" || InterestRate <= 0)
                return 0;

            var divisor = InterestType switch
            {
                "Daily" => 365,
                "Weekly" => 52,
                "Monthly" => 12,
                "Yearly" => 1,
                _ => 1
            };

            var periodicRate = InterestRate / 100 / divisor;
            var monthlyInterest = (Balance * periodicRate) * (divisor / 12m);
            return monthlyInterest * monthsAhead;
        }
    }

    public class ProgressionPoint
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public decimal RunningBalance { get; set; }
    }
}
