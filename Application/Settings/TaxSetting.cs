using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceHelper.Application.Settings
{
    public class TaxSettings
    {
        public decimal PersonalAllowance { get; set; }
        public decimal BasicRateLimit { get; set; }
        public decimal HigherRateLimit { get; set; }
        public decimal BasicRate { get; set; }
        public decimal HigherRate { get; set; }
        public decimal AdditionalRate { get; set; }
        public decimal NIPrimaryThreshold { get; set; }
        public decimal NIUpperEarningsLimit { get; set; }
        public decimal NIMainRate { get; set; }
        public decimal NIUpperRate { get; set; }
        public decimal StudentLoan1Threshold { get; set; }
        public decimal StudentLoan2Threshold { get; set; }
        public decimal StudentLoan4Threshold { get; set; }
        public decimal StudentLoan5Threshold { get; set; }
        public decimal StudentLoanRate { get; set; }
        public decimal PensionThreshold { get; set; }
    }
}
