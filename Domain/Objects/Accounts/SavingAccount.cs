using FinanceHelper.Domain.Objects.Base;

namespace FinanceHelper.Domain.Objects.Accounts
{
    public class SavingAccount : BaseEntity
    {
        public SavingAccount(int userId, string name, string provider, AccountType accountType, decimal interestRate = 0, decimal initialDeposit = 0, InterestType interestType)
        {
            UserId = userId;
            Name = name;
            Provider = provider;
            AccountType = accountType;
            InterestRate = interestRate;
            InterestType = interestType;

            if (initialDeposit > 0)
            {
                AddTransaction(initialDeposit, "Initial Deposit");
            }
        }
        public int UserId { get; private set; }
        protected string Name { get; private set; }
        protected string Provider { get; private set; }
        protected decimal GrossBalance => Transactions.Sum(t => t.EffectiveAmount);
        protected decimal Earnings { get; private set; }
        protected decimal NetBalance => GrossBalance + Earnings;
        protected AccountType AccountType { get; private set; }
        protected decimal InterestRate { get; private set; }
        public List<SavingTransaction> Transactions { get; private set; } = new();
        protected InterestType InterestType { get; private set; } = InterestType.None;

        public void AddTransaction(decimal amount, string description = "Transaction")
        {
            if (amount == 0)
                throw new ArgumentException("Transaction amount cannot be zero.", nameof(amount));

            var type = amount > 0 ? TransactionType.Deposit : TransactionType.Withdrawal;

            var absoluteAmount = Math.Abs(amount);

            if (type == TransactionType.Withdrawal && GrossBalance < absoluteAmount)
                throw new InvalidOperationException("Insufficient balance for this withdrawal.");

            var transaction = new SavingTransaction
            {
                Amount = absoluteAmount,
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

    public enum InterestType
    {
        None,
        Daily,
        Weekly,
        Monthly,
        Yearly,
    }
}
