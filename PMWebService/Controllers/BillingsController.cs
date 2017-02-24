using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PMWebService.Models;

namespace PMWebService.Controllers
{
    public class BillingsController : ApiController
    {
        private PhotographerContext db = new PhotographerContext();

        // GET: api/Billings
        public IQueryable<Billing> GetBilling()
        {
            IEnumerable<string> headerValues;
            string nameFilter = string.Empty;
            if (Request.Headers.TryGetValues("username", out headerValues))
            {
                nameFilter = headerValues.FirstOrDefault();
            }

            var bills = from b in db.Billing
                        where b.Username == nameFilter
                        select b;

            return bills;
        }

        // GET: api/Billings/Years
        [Route("api/Billings/Years")]
        public List<int> GetBillingYears()
        {
            IEnumerable<string> headerValues;
            string nameFilter = string.Empty;
            if (Request.Headers.TryGetValues("username", out headerValues))
            {
                nameFilter = headerValues.FirstOrDefault();
            }

            var billingYears = (from b in db.Billing
                        where b.Username == nameFilter
                        select b.BillingDate.Year).Distinct();

            return billingYears.OrderByDescending(b => b).ToList();
        }

        // GET: api/Billings/5
        [ResponseType(typeof(Billing))]
        public async Task<IHttpActionResult> GetBilling(int id)
        {
            Billing billing = await db.Billing.FindAsync(id);
            if (billing == null)
            {
                return NotFound();
            }

            return Ok(billing);
        }

        // PUT: api/Billings/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBilling(int id, Billing billing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != billing.BillingID)
            {
                return BadRequest();
            }

            db.Entry(billing).State = EntityState.Modified;

            IEnumerable<string> headerValues;
            string nameFilter = string.Empty;
            if (Request.Headers.TryGetValues("username", out headerValues))
            {
                nameFilter = headerValues.FirstOrDefault();
            }

            TaxYear taxYear = db.TaxYears.Find(billing.TaxYearID);

            // Calculate sales tax for payment
            if (billing.BillingType == "Payment")
            {
                decimal salesTax = billing.GetSalesTax(billing, taxYear);
                billing.Subtotal = billing.Total - salesTax;
                billing.SalesTax = salesTax;
            }

            taxYear.TotalTax = 0;
            taxYear.TotalExpenses = 0;
            taxYear.TotalGrossIncome = 0;

            var bills = from b in db.Billing
                        where b.Username == nameFilter
                        select b;

            foreach (Billing bill in bills)
            {
                if (bill.TaxYearID == billing.TaxYearID)
                {
                    if (bill.BillingType == "Payment")
                    {
                        taxYear.TotalTax += bill.SalesTax;
                        taxYear.TotalGrossIncome += bill.Subtotal;
                    }
                    else if (bill.BillingType == "Expense")
                    {
                        taxYear.TotalExpenses += bill.Total;
                    }
                }
            }

            taxYear.TotalNetIncome = taxYear.TotalGrossIncome - taxYear.TotalExpenses;

            db.Entry(taxYear).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Billings
        [ResponseType(typeof(Billing))]
        public async Task<IHttpActionResult> PostBilling(Billing billing)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<string> headerValues;
            string nameFilter = string.Empty;
            if (Request.Headers.TryGetValues("username", out headerValues))
            {
                nameFilter = headerValues.FirstOrDefault();
            }

            TaxYear taxYear = db.TaxYears.Find(billing.TaxYearID);

            // Calculate sales tax for payment
            if (billing.BillingType == "Payment")
            {
                decimal salesTax = billing.GetSalesTax(billing, taxYear);
                billing.Subtotal = billing.Total - salesTax;
                billing.SalesTax = salesTax;
            }

            if (billing.BillingType == "Payment")
            {
                taxYear.TotalTax += billing.SalesTax;
                taxYear.TotalGrossIncome += billing.Subtotal;
            }
            else if (billing.BillingType == "Expense")
            {
                taxYear.TotalExpenses += billing.Total;
            }

            taxYear.TotalNetIncome = taxYear.TotalGrossIncome - taxYear.TotalExpenses;

            db.Entry(taxYear).State = EntityState.Modified;

            db.Billing.Add(billing);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = billing.BillingID }, billing);
        }

        // DELETE: api/Billings/5
        [ResponseType(typeof(Billing))]
        public async Task<IHttpActionResult> DeleteBilling(int id)
        {
            IEnumerable<string> headerValues;
            string nameFilter = string.Empty;
            if (Request.Headers.TryGetValues("username", out headerValues))
            {
                nameFilter = headerValues.FirstOrDefault();
            }

            Billing billing = await db.Billing.FindAsync(id);
            if (billing == null)
            {
                return NotFound();
            }

            db.Billing.Remove(billing);

            TaxYear taxYear = db.TaxYears.Find(billing.TaxYearID);
            taxYear.TotalTax = 0;
            taxYear.TotalExpenses = 0;
            taxYear.TotalGrossIncome = 0;

            var bills = from b in db.Billing
                        where b.Username == nameFilter
                        select b;

            foreach (Billing bill in bills)
            {
                if (bill.TaxYearID == billing.TaxYearID)
                {
                    if (bill.BillingType == "Payment")
                    {
                        taxYear.TotalTax += bill.SalesTax;
                        taxYear.TotalGrossIncome += bill.Subtotal;
                    }
                    else if (bill.BillingType == "Expense")
                    {
                        taxYear.TotalExpenses += bill.Total;
                    }
                }
            }

            taxYear.TotalNetIncome = taxYear.TotalGrossIncome - taxYear.TotalExpenses;

            db.Entry(taxYear).State = EntityState.Modified;

            await db.SaveChangesAsync();

            return Ok(billing);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BillingExists(int id)
        {
            return db.Billing.Count(e => e.BillingID == id) > 0;
        }
    }
}