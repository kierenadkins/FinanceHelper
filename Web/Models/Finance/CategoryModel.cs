using Application.Domain.Finance.ExpenseTracking;

namespace Web.Models.Finance;

public class CategoryModel
{
    public CategoryModel(Category category)
    {
        Id = category.Id;
        UserId = category.UserId;
        Name = category.Type.ToString();
        SubCategories = category.SubCategories.Select(x => new SubCategoryModel(x)).ToList();
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public List<SubCategoryModel> SubCategories { get; set; }
}