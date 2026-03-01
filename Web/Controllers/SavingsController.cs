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
            var model = new FinanceHelper.Web.Models.Saving.SavingsModel();

            var list = await _mediator.Send(new GetSavingAccountDashboard());

            if (list == null || list.Count == 0)
            {
                // Debug: Return empty model if no accounts
                return View(model);
            }

            model.Savings = list.Select(x => new FinanceHelper.Web.Models.Saving.SavingAccountModel(x)).ToList();
            model.CalculateTotals();

            return View(model);
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
                ModelState.AddModelError(string.Empty, result.Errors != null && result.Errors.Any() ? string.Join(';', result.Errors) : "Unable to save saving account.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: SavingsController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var account = await _mediator.Send(new GetSavingAccount { Id = id });
            if (account == null)
                return NotFound();

            var model = new FinanceHelper.Web.Models.Saving.SavingAccountModel(account);

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
                TempData["Error"] = result.Errors != null && result.Errors.Any() ? string.Join(';', result.Errors) : "Unable to update saving account.";
                return RedirectToAction(nameof(Edit), new { id = model.Id });
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: SavingsController/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: SavingsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<JsonResult> AddTransaction(AddSavingTransaction model)
        {
            var result = await _mediator.Send(model);
            if (result != null && !result.Success)
                return Json(new { success = false, message = result.Errors != null && result.Errors.Any() ? string.Join(';', result.Errors) : "Failed" });

            return Json(new { success = true });
        }
    }
}
