using FinanceHelper.Domain.Objects.Accounts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinanceHelper.Web.Models.Saving;

public class AddSavingAccountModel
{
    public string Name { get; set; }
    public string Provider { get; set; }
    public decimal InitialBalance { get; set; }
    public AccountType AccountType { get; set; }
    public decimal InterestRate { get; set; }
    public InterestType InterestType { get; set; }

    public IEnumerable<SelectListItem> InterestTypeOptions { get; set; } = Enum.GetValues(typeof(InterestType)).Cast<InterestType>().Select(at => new SelectListItem { Value = at.ToString(), Text = at.ToString() });
    public IEnumerable<SelectListItem> AccountTypeOptions { get; set; } = Enum.GetValues(typeof(AccountType)).Cast<AccountType>().Select(at => new SelectListItem { Value = at.ToString(), Text = at.ToString() });
}