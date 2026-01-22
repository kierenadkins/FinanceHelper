using FinanceHelper.Application.Services.Finance.ExpenseTracking;
using FinanceHelper.Application.Services.User;
using FinanceHelper.Application.Usecases.Category.Command;
using FinanceHelper.Domain.Objects.Finance.ExpenseTracking;
using FinanceHelper.Web.Attributes;
using FinanceHelper.Web.Models.Finance;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHelper.Web.Controllers;

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
        var model = new AddSubCategoryModel()
        {
            CategoryId = categoryId
        };

        return View("AddSubCategory", model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSubCategoryPostAsync(AddSubCategoryModel model)
    {
        var result = await _mediator.Send(new AddSubCategoryCommand {
            Name = model.Name,
            SubCategoryType = model.SubCategoryType,
            PayFrequency = model.PaymentFrequency,
            CategoryId = model.CategoryId,
            YearlyCost = model.YearlyCost,
            MonthlyCost = model.MonthlyCost,
            WeeklyCost = model.WeeklyCost
        });

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
        var result = await _mediator.Send(new UpdateSubCategoryCommand
        {
            Id = model.SubCategoryId,
            Name = model.Name,
            SubCategoryType = model.SubCategoryType,
            PayFrequency = model.PaymentFrequency,
            CategoryId = model.CategoryId,
            YearlyCost = model.YearlyCost,
            MonthlyCost = model.MonthlyCost,
            WeeklyCost = model.WeeklyCost
        });

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
}