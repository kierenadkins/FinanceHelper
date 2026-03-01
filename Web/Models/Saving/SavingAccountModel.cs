using FinanceHelper.Domain.Objects.Accounts;

namespace FinanceHelper.Web.Models.Saving
{
    public class SavingAccountModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public AccountType AccountType { get; set; }
        public decimal InterestRate { get; set; }
        public InterestType InterestType { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalInterestEarned { get; set; }
        public List<SavingTransaction> Transactions { get; set; } = new();

        public SavingAccountModel() { }

        public SavingAccountModel(SavingAccount account)
        {
            Id = account.Id;
            Name = account.Name;
            Provider = account.Provider;
            AccountType = account.AccountType;
            InterestRate = account.InterestRate;
            InterestType = account.InterestType;
            Balance = account.Balance;
            TotalInterestEarned = account.Transactions
                .Where(t => t.Type == TransactionType.Interest)
                .Sum(t => t.Amount);
            Transactions = account.Transactions ?? new List<SavingTransaction>();
        }
    }
}
