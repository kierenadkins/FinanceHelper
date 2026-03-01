namespace FinanceHelper.Web.Models.Saving;

public class SavingAccountProgressionViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Provider { get; set; }
    public decimal CurrentBalance { get; set; }
    public string InterestType { get; set; }
    public decimal InterestRate { get; set; }
    public List<TransactionProgressionItem> Transactions { get; set; } = new();
    public decimal TotalInterestEarned => Transactions
        .Where(t => t.Type == "Interest")
        .Sum(t => t.Amount);
    public decimal TotalDeposits => Transactions
        .Where(t => t.Type == "Deposit")
        .Sum(t => t.Amount);
}

public class TransactionProgressionItem
{
    public DateTime TransactionDate { get; set; }
    public string Type { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public decimal RunningBalance { get; set; }
}
