using FinanceHelper.Domain.Objects.Base;

namespace FinanceHelper.Domain.Objects.Accounts
{
    public class SavingAccount : BaseEntity
    {
        public SavingAccount(int userId, string name, string provider, AccountType accountType, decimal interestRate = 0, decimal initialDeposit = 0)
        {
            UserId = userId;
            Name = name;
            Provider = provider;
            AccountType = accountType;
            InterestRate = interestRate;

            if (initialDeposit > 0)
            {
                AddTransaction(initialDeposit, TransactionType.Deposit, "Initial Deposit");
            }
        }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Provider { get; set; }
        public decimal GrossBalance => Transactions.Sum(t => t.EffectiveAmount);
        public decimal Earnings { get; private set; }
        public decimal NetBalance => GrossBalance + Earnings;
        public AccountType AccountType { get; set; }
        public decimal InterestRate { get; set; }
        public List<SavingTransaction> Transactions { get; private set; } = new();

        public void AddTransaction(decimal amount, TransactionType type, string description = "Transaction")
        {
            if (amount <= 0)
                throw new ArgumentException("Transaction amount must be positive.", nameof(amount));

            if (type == TransactionType.Withdrawal && GrossBalance < amount)
                throw new InvalidOperationException("Insufficient balance for this withdrawal.");

            var transaction = new SavingTransaction
            {
                Amount = amount,
                Type = type,
                Description = description,
                TransactionDate = DateTime.UtcNow
            };

            Transactions.Add(transaction);
        }

        public decimal GetGrowth(DateTime fromDate)
        {
            return Transactions
                .Where(t => t.TransactionDate >= fromDate)
                .Sum(t => t.EffectiveAmount);
        }

        public decimal GetGrowthAllTime() => GetGrowth(DateTime.MinValue);
        public decimal GetGrowthLastYear() => GetGrowth(DateTime.UtcNow.AddYears(-1));
        public decimal GetGrowthLast6Months() => GetGrowth(DateTime.UtcNow.AddMonths(-6));
        public decimal GetGrowthLast3Months() => GetGrowth(DateTime.UtcNow.AddMonths(-3));
    }

    public enum AccountType
    {
        Savings,
        FixedDeposit,
        Other
    }
}
