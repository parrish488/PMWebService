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

            db.Billing.Add(billing);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = billing.BillingID }, billing);
        }

        // DELETE: api/Billings/5
        [ResponseType(typeof(Billing))]
        public async Task<IHttpActionResult> DeleteBilling(int id)
        {
            Billing billing = await db.Billing.FindAsync(id);
            if (billing == null)
            {
                return NotFound();
            }

            db.Billing.Remove(billing);
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