using Application.Domain.Finance.ExpenseTracking;
using Application.Services.Finance.ExpenseTracking;
using Application.Services.User;
using Application.Usecases.Category.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Attribute;
using Web.Models.Finance;

namespace Web.Controllers;

[AuthorizeSession]
public class CategoryController : Controller
{
    private readonly IMediator _mediator;
    private readonly ICategoryService _categoryService;
    private readonly ISubCategoryService _subCategoryService;
    private readonly IUserAccountService _userAccountService;
    public CategoryController(IMediator mediator, ICategoryService categoryService, ISubCategoryService subCategoryService, IUserAccountService userAccountService)
    {
        _mediator = mediator;
        _categoryService = categoryService;
        _subCategoryService = subCategoryService;
        _userAccountService = userAccountService;
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCategory(CategoryType categoryType)
    {
        var result = await _mediator.Send(new SaveCategoryCommand() {CategoryType = categoryType });

        if (!result.Success)
        {

        }

        return RedirectToAction("Index", "Finance");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var userId = _userAccountService.GetCurrent();
        await _categoryService.DeleteAsync(id, userId);
        return RedirectToAction("Index", "Finance");
    }


    public IActionResult AddSubCategory(int categoryId)
    {
        var paymentFrequency = Enum.GetValues(typeof(PaymentFrequency)).Cast<PaymentFrequency>().Select(ct => new SelectListItem
        {
            Value = ct.ToString(),
            Text = ct.ToString()
        });

        var model = new AddSubCategoryModel()
        {
            PaymentFrequencyTypeOptions = paymentFrequency,
            CategoryId = categoryId
        };

        return View("AddSubCategory", model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSubCategoryPostAsync(AddSubCategoryModel model)
    {
        var subCategory = PopulateSubCategory(model);

        var result = await _mediator.Send(new AddSubCategoryCommand { SubCategory = subCategory });

        if (!result.Success)
        {

        }

        return RedirectToAction("Index", "Finance");
    }

    public async Task<IActionResult> EditSubCategory(int subCategoryId)
    {
        var subCat = await _subCategoryService.GetByIdAsync(subCategoryId);

        if (subCat == null)
        {
            return View("Error");
        }

        var model = new AddSubCategoryModel()
        {
            SubCategoryId = subCategoryId,
            CategoryId = subCat.CategoryId,
            SubCategoryType = subCat.SubCategoryType,
            MonthlyCost = subCat.MonthlyCost,
            Name = subCat.Name,
            WeeklyCost = subCat.WeeklyCost,
            YearlyCost = subCat.YearlyCost,
        };

        return View("EditSubCategory", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditSubCategoryPost(AddSubCategoryModel model)
    {
        var subCat = PopulateSubCategory(model);

        var result = await _mediator.Send(new UpdateSubCategoryCommand() { SubCategory = subCat });

        if (!result.Success)
        {

        }

        return RedirectToAction("Index", "Finance");
    }

    // POST: Delete Subcategory
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSubCategory(int id)
    {
        var userId = _userAccountService.GetCurrent();
        await _subCategoryService.DeleteAsync(id, userId);

        return RedirectToAction("Index", "Finance");
    }

    private static SubCategory PopulateSubCategory(AddSubCategoryModel model)
    {
        var subCategory = new SubCategory
        {
            Id = model.SubCategoryId,
            Name = model.Name,
            SubCategoryType = model.SubCategoryType,
            PayFrequency = model.PaymentFrequency,
            CategoryId = model.CategoryId,
            YearlyCost = model.YearlyCost,
            MonthlyCost = model.MonthlyCost,
            WeeklyCost = model.WeeklyCost
        };
        return subCategory;
    }
}