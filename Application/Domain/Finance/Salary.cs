using Application.Enums.Finance;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Domain.Finance
{
    public class Salary
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal NetSalary { get; set; }
        public int PensionPercentage { get; set; }
        public decimal PensionContribution { get; set; }
        public decimal Tax { get; set; }
        public TaxBand TaxBand { get; set; }
        public decimal NationalInsurance { get; set; }
        public decimal TaxableBenefits { get; set; }
        public bool PayNationalInsurance { get; set; } = true;
        public bool HasStudentLoan { get; set; }
        public StudentPlanType? StudentPlanType { get; set; } = null;
        public decimal StudentLoan { get; set; }
    }
}
