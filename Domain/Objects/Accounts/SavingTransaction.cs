using FinanceHelper.Domain.Objects.Base;

namespace FinanceHelper.Domain.Objects.Accounts;

public class SavingTransaction : BaseEntity
{
    public int SavingAccountId { get; set; } // Foreign key to SavingAccount
    public decimal Amount { get; set; } 
    public TransactionType Type { get; set; } 
    public string Description { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal EffectiveAmount => Type == TransactionType.Deposit ? Amount : -Amount;
}

public enum TransactionType
{
    Deposit,
    Withdrawal,
}