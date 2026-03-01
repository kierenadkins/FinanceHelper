using FinanceHelper.Web.Attributes;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using FinanceHelper.Application.Usecases.Finance;
using FinanceHelper.Application.Usecases.Category.Command;
using FinanceHelper.Application.Usecases.Savings.Command;
using FinanceHelper.Web.Models.Saving;
using FinanceHelper.Domain.Objects.Accounts;


namespace FinanceHelper.Web.Controllers
{
    [AuthorizeSession]
    public class SavingsController : Controller
    {
        private readonly IMediator _mediator;

        public SavingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: /Savings
        public async Task<IActionResult> Index()
        {
            var model = new SavingsModel();

            var list = await _mediator.Send(new GetSavingAccountDashboard());

            if (list == null || list.Count == 0)
            {
                return View(model);
            }

            model.Savings = list.Select(x => MapToProgressionModel(x)).ToList();
            model.CalculateTotals();

            return View(model);
        }

        private SavingAccountProgressionModel MapToProgressionModel(SavingAccount account)
        {
            var transactions = account.Transactions
                .OrderBy(t => t.TransactionDate)
                .ToList();

            var progressionHistory = new List<ProgressionPoint>();
            decimal runningBalance = 0;

            foreach (var transaction in transactions)
            {
                runningBalance += transaction.EffectiveAmount;
                progressionHistory.Add(new ProgressionPoint
                {
                    Date = transaction.TransactionDate,
                    Type = transaction.Type.ToString(),
                    Amount = transaction.Amount,
                    Description = transaction.Description,
                    RunningBalance = runningBalance
                });
            }

            var lastInterestTransaction = transactions
                .Where(t => t.Type == TransactionType.Interest)
                .OrderByDescending(t => t.TransactionDate)
                .FirstOrDefault();

            return new SavingAccountProgressionModel
            {
                Id = account.Id,
                Name = account.Name,
                Provider = account.Provider,
                AccountType = account.AccountType,
                InterestRate = account.InterestRate,
                InterestType = account.InterestType.ToString(),
                Balance = account.Balance,
                TotalInterestEarned = account.Transactions
                    .Where(t => t.Type == TransactionType.Interest)
                    .Sum(t => t.Amount),
                ProgressionHistory = progressionHistory,
                LastInterestDate = lastInterestTransaction?.TransactionDate ?? (account.DateCreated ?? DateTime.UtcNow),
                CreatedDate = account.DateCreated ?? DateTime.UtcNow
            };
        }

        // GET: SavingsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SavingsController/Create
        public IActionResult Create()
        {
            var model = new AddSavingAccountModel();
            return View(model);
        }

        // POST: SavingsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddSavingAccountModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.InitialBalance < 0)
            {
                ModelState.AddModelError(nameof(model.InitialBalance), "Initial balance must be greater than or equal to zero");
                return View(model);
            }

            var cmd = new SaveSavingsAccount
            {
                Name = model.Name,
                Provider = model.Provider,
                InitialBalance = model.InitialBalance,
                AccountType = model.AccountType,
                InterestRate = model.InterestRate,
                InterestType = model.InterestType
            };

            var result = await _mediator.Send(cmd);
            if (result != null && !result.Success)
            {
                var errorMessage = result.Errors?.Any() == true
                    ? string.Join("; ", result.Errors)
                    : "Unable to save saving account.";
                ModelState.AddModelError(string.Empty, errorMessage);
                return View(model);
            }

            TempData["Success"] = "Saving account created successfully";
            return RedirectToAction(nameof(Index));
        }

        // GET: SavingsController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var account = await _mediator.Send(new GetSavingAccount { Id = id });
            if (account == null)
                return NotFound();

            var model = new SavingAccountModel(account);
            return View(model);
        }

        // POST: SavingsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateSavingAccount model)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Edit), new { id = model.Id });

            var result = await _mediator.Send(model);
            if (result != null && !result.Success)
            {
                var errorMessage = result.Errors?.Any() == true
                    ? string.Join("; ", result.Errors)
                    : "Unable to update saving account.";
                TempData["Error"] = errorMessage;
                return RedirectToAction(nameof(Edit), new { id = model.Id });
            }

            TempData["Success"] = "Saving account updated successfully";
            return RedirectToAction(nameof(Index));
        }

        // GET: SavingsController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var account = await _mediator.Send(new GetSavingAccount { Id = id });
            if (account == null)
                return NotFound();

            var model = new SavingAccountModel(account);
            return View(model);
        }

        // POST: SavingsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var result = await _mediator.Send(new DeleteSavingAccount { Id = id });
                
                if (result != null && !result.Success)
                {
                    var errorMessage = result.Errors?.Any() == true
                        ? string.Join("; ", result.Errors)
                        : "Unable to delete saving account.";
                    TempData["Error"] = errorMessage;
                    return RedirectToAction(nameof(Delete), new { id });
                }

                TempData["Success"] = "Saving account deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting account: {ex.Message}";
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddTransaction(AddSavingTransaction model)
        {
            // Allow negative amounts (withdrawals) - just check it's not zero
            if (model.Amount == 0)
            {
                return Json(new { success = false, message = "Transaction amount cannot be zero" });
            }

            var result = await _mediator.Send(model);
            if (result != null && !result.Success)
            {
                var message = result.Errors?.Any() == true
                    ? string.Join("; ", result.Errors)
                    : "Failed to add transaction";
                return Json(new { success = false, message });
            }

            return Json(new { success = true, message = "Transaction added successfully" });
        }
    }
}
