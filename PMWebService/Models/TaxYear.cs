using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMWebService.Models
{
    public class TaxYear
    {
        public TaxYear()
        {
            TaxRate = 0;
            TotalTax = 0.0m;
            TotalMiles = 0;
            TaxablePercent = 0.0m;
            TotalExpenses = 0.0m;
            TotalGrossIncome = 0.0m;
            TotalNetIncome = 0.0m;
        }

        public int TaxYearID { get; set; }

        public int Year { get; set; }

        [Display(Name = "Tax Rate")]
        public float TaxRate { get; set; }

        [Display(Name = "Total Tax")]
        public decimal TotalTax { get; set; }

        [Display(Name = "Total Miles")]
        public double TotalMiles { get; set; }

        [Display(Name = "Taxable Percent")]
        public decimal TaxablePercent { get; set; }

        [Display(Name = "Total Expenses")]
        public decimal TotalExpenses { get; set; }

        [Display(Name = "Total Gross Income")]
        public decimal TotalGrossIncome { get; set; }

        [Display(Name = "Total Net Income")]
        public decimal TotalNetIncome { get; set; }

        public string Username { get; set; }
    }
}