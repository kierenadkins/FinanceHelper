using FinanceHelper.Domain.Objects.Accounts;

namespace FinanceHelper.Application.Services.Savings;

public interface IInterestCalculationService
{
    /// <summary>
    /// Calculates the next interest due date based on interest type and last calculation date
    /// </summary>
    DateTime GetNextInterestDueDate(DateTime lastCalculationDate, InterestType interestType);

    /// <summary>
    /// Calculates the interest amount based on current balance and annual interest rate
    /// </summary>
    decimal CalculateInterest(decimal currentBalance, decimal annualInterestRate, InterestType interestType);

    /// <summary>
    /// Checks if interest is due for application and applies it if needed
    /// </summary>
    void ApplyInterestIfDue(SavingAccount account);
}

public class InterestCalculationService : IInterestCalculationService
{
    public DateTime GetNextInterestDueDate(DateTime lastCalculationDate, InterestType interestType)
    {
        return interestType switch
        {
            InterestType.Daily => lastCalculationDate.AddDays(1),
            InterestType.Weekly => lastCalculationDate.AddDays(7),
            InterestType.Monthly => lastCalculationDate.AddMonths(1),
            InterestType.Yearly => lastCalculationDate.AddYears(1),
            _ => lastCalculationDate.AddYears(1)
        };
    }

    public decimal CalculateInterest(decimal currentBalance, decimal annualInterestRate, InterestType interestType)
    {
        if (currentBalance <= 0 || annualInterestRate <= 0)
            return 0;

        var divisor = interestType switch
        {
            InterestType.Daily => 365,
            InterestType.Weekly => 52,
            InterestType.Monthly => 12,
            InterestType.Yearly => 1,
            _ => 1
        };

        var periodicRate = annualInterestRate / 100 / divisor;
        var interest = currentBalance * periodicRate;

        return Math.Round(interest, 2);
    }

    public void ApplyInterestIfDue(SavingAccount account)
    {
        if (account.InterestType == InterestType.None)
            return;

        var nextDueDate = GetNextInterestDueDate(account.LastInterestCalculationDate, account.InterestType);

        if (DateTime.UtcNow < nextDueDate)
            return;

        var interestAmount = CalculateInterest(account.Balance, account.InterestRate, account.InterestType);
        
        if (interestAmount > 0)
        {
            account.ApplyInterest(interestAmount);
        }
    }
}
