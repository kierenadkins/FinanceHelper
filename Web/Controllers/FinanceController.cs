using Application.Domain.Finance;
using Application.Enums.Finance;
using Application.Extentions.Numerics;
using Application.Services.User;
using Application.Usecases.Finance.Salarys.Command;
using Application.Usecases.Finance.Salarys.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Models.Finance;

namespace Web.Controllers
{
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

            var salary = await _mediator.Send(new GetSalaryQuery { });

            model.Salary = new SalaryModel(salary);
            model.YearlyLeftOver = model.Salary.NetSalary - model.YearlyTotalOutGoings;
            model.MonthlyLeftOver = (model.Salary.NetSalary.YearlyToMonthly() - model.MonthlyOutGoings).To2DP();
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
            var salary = new Salary { GrossSalary = model.GrossSalary, PensionPercentage = model.PensionPercentage, PayNationalInsurance = model.PayNationalInsurance, HasStudentLoan = model.HasStudentLoan, StudentPlanType = model.StudentPlanType };

            var result = await _mediator.Send(new CalculateSalaryTaxablesCommand { Salary = salary });
            
            if(result.Errors.Count > 0)
            {

            }

            var salaryResponse = result.Value;

            var addSalaryFormModel = new AddSalaryFormModel { GrossSalary = salaryResponse!.GrossSalary, HasStudentLoan = salaryResponse!.HasStudentLoan, NationalInsurance = salaryResponse!.NationalInsurance, NetSalary = salaryResponse!.NetSalary, PayNationalInsurance = salaryResponse!.PayNationalInsurance, PensionContribution = salaryResponse!.PensionContribution, PensionPercentage = salaryResponse!.PensionPercentage, StudentLoan = salaryResponse!.StudentLoan, StudentPlanType = salaryResponse!.StudentPlanType, Tax = salaryResponse!.Tax, TaxableBenefits = salaryResponse!.TaxableBenefits, TaxBand = salaryResponse!.TaxBand, StudentPlanTypeOptions = model.StudentPlanTypeOptions };

            var monthlyTotals = new MonthlyTotals(salary);
            addSalaryFormModel.UpdateMonthlyTotals(monthlyTotals);

            addSalaryFormModel.IsReview = true;

            return View("SalaryForm", addSalaryFormModel);
        }

        public async Task<IActionResult> SaveSalary(AddSalaryFormModel model)
        {
            var salary = new Salary { GrossSalary = model.GrossSalary, HasStudentLoan = model.HasStudentLoan, NationalInsurance = model.NationalInsurance, NetSalary = model.NetSalary, PayNationalInsurance = model.PayNationalInsurance, PensionContribution = model.PensionContribution, PensionPercentage = model.PensionPercentage, StudentLoan = model.StudentLoan, StudentPlanType = model.StudentPlanType, Tax = model.Tax, TaxBand = model.TaxBand };

            await _mediator.Send(new SaveSalaryCommand { Salary = salary });
            return RedirectToAction(nameof(Index));
        }
    }
}
