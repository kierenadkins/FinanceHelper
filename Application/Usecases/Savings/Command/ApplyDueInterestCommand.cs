using FinanceHelper.Application.Services.Savings;
using MediatR;

namespace FinanceHelper.Application.Usecases.Savings.Command;

/// <summary>
/// Command to apply due interest to all saving accounts
/// Useful for scheduled jobs or manual triggers
/// </summary>
public class ApplyDueInterestCommand : IRequest<ApplyDueInterestResult>
{
}

public class ApplyDueInterestResult
{
    public bool Success { get; set; }
    public int AccountsProcessed { get; set; }
    public decimal TotalInterestApplied { get; set; }
    public string Message { get; set; }
}

public class ApplyDueInterestHandler : IRequestHandler<ApplyDueInterestCommand, ApplyDueInterestResult>
{
    private readonly IInterestApplicationService _interestApplicationService;

    public ApplyDueInterestHandler(IInterestApplicationService interestApplicationService)
    {
        _interestApplicationService = interestApplicationService;
    }

    public async Task<ApplyDueInterestResult> Handle(ApplyDueInterestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _interestApplicationService.ApplyDueInterestAsync();

            return new ApplyDueInterestResult
            {
                Success = true,
                Message = "Interest applied successfully"
            };
        }
        catch (Exception ex)
        {
            return new ApplyDueInterestResult
            {
                Success = false,
                Message = $"Error applying interest: {ex.Message}"
            };
        }
    }
}
