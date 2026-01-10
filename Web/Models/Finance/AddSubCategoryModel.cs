using Application.Domain.Finance.ExpenseTracking;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Models.Finance;

public class AddSubCategoryModel
{
    public int SubCategoryId { get; set; }
    public IEnumerable<SelectListItem> SubCategoryTypeOptions { get; set; } = Enumerable.Empty<SelectListItem>();
    public SubCategoryType SubCategoryType { get; set; }

    public IEnumerable<SelectListItem> PaymentFrequencyTypeOptions { get; set; } = Enumerable.Empty<SelectListItem>();
    public PaymentFrequency PaymentFrequency { get; set; }

    public int CategoryId { get; set; }
    public string Name { get; set; }
    public decimal YearlyCost { get; set; }
    public decimal MonthlyCost { get; set; }
    public decimal WeeklyCost { get; set; }
}