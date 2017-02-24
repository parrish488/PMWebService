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
    public class TaxYearsController : ApiController
    {
        private PhotographerContext db = new PhotographerContext();

        // GET: api/TaxYears
        public IQueryable<TaxYear> GetTaxYears()
        {
            IEnumerable<string> headerValues;
            string nameFilter = string.Empty;
            if (Request.Headers.TryGetValues("username", out headerValues))
            {
                nameFilter = headerValues.FirstOrDefault();
            }

            var taxYears = from t in db.TaxYears
                          where t.Username == nameFilter
                          select t;

            return taxYears;
        }

        // GET: api/TaxYears/5
        [ResponseType(typeof(TaxYear))]
        public async Task<IHttpActionResult> GetTaxYear(int id)
        {
            TaxYear taxYear = await db.TaxYears.FindAsync(id);
            if (taxYear == null)
            {
                return NotFound();
            }

            return Ok(taxYear);
        }

        // PUT: api/TaxYears/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTaxYear(int id, TaxYear taxYear)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taxYear.TaxYearID)
            {
                return BadRequest();
            }

            db.Entry(taxYear).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaxYearExists(id))
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

        // POST: api/TaxYears
        [ResponseType(typeof(TaxYear))]
        public async Task<IHttpActionResult> PostTaxYear(TaxYear taxYear)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TaxYears.Add(taxYear);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = taxYear.TaxYearID }, taxYear);
        }

        // DELETE: api/TaxYears/5
        [ResponseType(typeof(TaxYear))]
        public async Task<IHttpActionResult> DeleteTaxYear(int id)
        {
            TaxYear taxYear = await db.TaxYears.FindAsync(id);
            if (taxYear == null)
            {
                return NotFound();
            }

            db.TaxYears.Remove(taxYear);
            await db.SaveChangesAsync();

            return Ok(taxYear);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaxYearExists(int id)
        {
            return db.TaxYears.Count(e => e.TaxYearID == id) > 0;
        }
    }
}