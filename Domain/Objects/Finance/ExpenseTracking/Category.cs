using FinanceHelper.Domain.Objects.Base;

namespace FinanceHelper.Domain.Objects.Finance.ExpenseTracking;

public enum CategoryType
{
    HouseholdBills,
    Subscriptions,
    Car,
    Living,
    Insurance,    
    Savings,        
    Healthcare,       
    Childcare,       
    Education,       
    Travel,          
    Gifts,           
    Clothing,    
    HomeMaintenance,  
    Entertainment,    
    Miscellaneous
}


public class Category : BaseEntity
{
    public int UserId { get; set; }
    public CategoryType Type { get; set; }
    public List<SubCategory> SubCategories { get; set; } = [];
}