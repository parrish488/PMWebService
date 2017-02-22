using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PMWebService.Models
{
    public class Billing
    {
        public int BillingID { get; set; }
        [Display(Name = "Billing Date")]
        public DateTime BillingDate { get; set; }
        [Display(Name = "Billing Type")]
        public string BillingType { get; set; }
        public decimal Subtotal { get; set; }
        [Display(Name = "Sales Tax")]
        public decimal SalesTax { get; set; }
        public decimal Total { get; set; }
        public string Username { get; set; }

        [ForeignKey("Client")]
        public int? ClientID { get; set; }
        public virtual Client Client { get; set; }

        [ForeignKey("TaxYear")]
        public int TaxYearID { get; set; }
        public virtual TaxYear TaxYear { get; set; }

        public decimal GetSalesTax(Billing billing, TaxYear taxYear)
        {
            decimal subtotal = decimal.Round(decimal.Divide(decimal.Multiply(billing.Total, (taxYear.TaxablePercent / 100)), (decimal)(1 + (taxYear.TaxRate / 100))), 2);
            decimal salesTax = decimal.Round(decimal.Subtract(decimal.Multiply(billing.Total, (taxYear.TaxablePercent / 100)), subtotal), 2);

            return salesTax;
        }

        public List<TaxYear> TaxYearIDs { get; set; }
        public List<Client> ClientIDs { get; set; }
    }
}