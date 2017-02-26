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
    public class EventTypesController : ApiController
    {
        private PhotographerContext db = new PhotographerContext();

        // GET: api/EventTypes
        public IQueryable<EventTypes> GetEventTypes()
        {
            IEnumerable<string> headerValues;
            string nameFilter = string.Empty;
            if (Request.Headers.TryGetValues("username", out headerValues))
            {
                nameFilter = headerValues.FirstOrDefault();
            }

            var eventTypes = from e in db.EventTypes
                        where e.Username == nameFilter
                        orderby e.EventTypeName ascending
                        select e;

            return eventTypes;
        }

        // GET: api/EventTypes/5
        [ResponseType(typeof(EventTypes))]
        public async Task<IHttpActionResult> GetEventTypes(int id)
        {
            EventTypes eventTypes = await db.EventTypes.FindAsync(id);
            if (eventTypes == null)
            {
                return NotFound();
            }

            return Ok(eventTypes);
        }

        // PUT: api/EventTypes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEventTypes(int id, EventTypes eventTypes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventTypes.ID)
            {
                return BadRequest();
            }

            db.Entry(eventTypes).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventTypesExists(id))
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

        // POST: api/EventTypes
        [ResponseType(typeof(EventTypes))]
        public async Task<IHttpActionResult> PostEventTypes(EventTypes eventTypes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EventTypes.Add(eventTypes);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = eventTypes.ID }, eventTypes);
        }

        // DELETE: api/EventTypes/5
        [ResponseType(typeof(EventTypes))]
        public async Task<IHttpActionResult> DeleteEventTypes(int id)
        {
            EventTypes eventTypes = await db.EventTypes.FindAsync(id);
            if (eventTypes == null)
            {
                return NotFound();
            }

            db.EventTypes.Remove(eventTypes);
            await db.SaveChangesAsync();

            return Ok(eventTypes);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventTypesExists(int id)
        {
            return db.EventTypes.Count(e => e.ID == id) > 0;
        }
    }
}