using FinanceHelper.Application.Usecases.Finance;
using FinanceHelper.Application.Usecases.Finance.Salarys.Command;
using FinanceHelper.Domain.Enums.Finance;
using FinanceHelper.Domain.Objects.Finance;
using FinanceHelper.Web.Attributes;
using FinanceHelper.Web.Models.Finance;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinanceHelper.Web.Controllers
{
    [AuthorizeSession]
    public class FinanceController : Controller
    {
        private readonly IMediator _mediator;
        public FinanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var model = new FinanceModel();

            var finance = await _mediator.Send(new GetFinanceDashboardQuery { });

            if (finance == null)
                return View(model);

            model.Salary = new SalaryModel(finance.Salary);
            model.Category = finance.CategoriesWithSubCats.Select(x => new CategoryModel(x)).ToList();
            model.MonthlyLeftOver = finance.MonthlyLeftOver;
            model.YearlyLeftOver = finance.YearlyLeftOver;
            model.MonthlyOutGoings = finance.MonthlyOutGoings;
            model.YearlyTotalOutGoings = finance.YearlyTotalOutGoings;

            return View(model);
        }

        [HttpGet]
        public IActionResult SalaryForm(decimal salary)
        {
            var model = new AddSalaryFormModel();
            var studentPlanTypeOptions = Enum.GetValues(typeof(StudentPlanType)).Cast<StudentPlanType>()
                .Select(sp => new SelectListItem
                {
                    Value = sp.ToString(),
                    Text = sp.ToString()
                });

            if (salary < 0)
            {
                model.Errors.Add("Please enter a positive number");
                return View(model);
            }

            model.GrossSalary = salary;
            model.StudentPlanTypeOptions = studentPlanTypeOptions;

            return View(model);
        }


        public async Task<IActionResult> CalculateSalaryDeductions(AddSalaryFormModel model)
        {
            var result = await _mediator.Send(new CalculateSalaryTaxablesCommand { GrossSalary = model.GrossSalary, PensionPercentage = model.PensionPercentage, PayNationalInsurance = model.PayNationalInsurance, HasStudentLoan = model.HasStudentLoan, StudentPlanType = model.StudentPlanType });
            
            if(result.Errors.Count > 0)
            {

            }

            var salaryResponse = result.Value;

            var addSalaryFormModel = new AddSalaryFormModel { GrossSalary = salaryResponse!.GrossSalary, HasStudentLoan = salaryResponse!.HasStudentLoan, NationalInsurance = salaryResponse!.NationalInsurance, NetSalary = salaryResponse!.NetSalary, PayNationalInsurance = salaryResponse!.PayNationalInsurance, PensionContribution = salaryResponse!.PensionContribution, PensionPercentage = salaryResponse!.PensionPercentage, StudentLoan = salaryResponse!.StudentLoan, StudentPlanType = salaryResponse!.StudentPlanType, Tax = salaryResponse!.Tax, TaxableBenefits = salaryResponse!.TaxableBenefits, TaxBand = salaryResponse!.TaxBand, StudentPlanTypeOptions = model.StudentPlanTypeOptions };
            addSalaryFormModel.UpdateMonthlyTotals();

            addSalaryFormModel.IsReview = true;

            return View("SalaryForm", addSalaryFormModel);
        }

        public async Task<IActionResult> SaveSalary(AddSalaryFormModel model)
        {
            await _mediator.Send(new SaveSalaryCommand { GrossSalary = model.GrossSalary, HasStudentLoan = model.HasStudentLoan, NationalInsurance = model.NationalInsurance, NetSalary = model.NetSalary, PayNationalInsurance = model.PayNationalInsurance, PensionContribution = model.PensionContribution, PensionPercentage = model.PensionPercentage, StudentLoan = model.StudentLoan, StudentPlanType = model.StudentPlanType, Tax = model.Tax, TaxBand = model.TaxBand });
            return RedirectToAction(nameof(Index));
        }
    }
}
