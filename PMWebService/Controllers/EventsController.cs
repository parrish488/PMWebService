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
    public class EventsController : ApiController
    {
        private PhotographerContext db = new PhotographerContext();

        // GET: api/Events
        public IQueryable<Event> GetEvents()
        {
            IEnumerable<string> headerValues;
            string nameFilter = string.Empty;
            if (Request.Headers.TryGetValues("username", out headerValues))
            {
                nameFilter = headerValues.FirstOrDefault();
            }

            var events = from e in db.Events
                        where e.Username == nameFilter
                        select e;

            return events;
        }

        // GET: api/Billings/Years
        [Route("api/Events/Years")]
        public List<int> GetEventYears()
        {
            IEnumerable<string> headerValues;
            string nameFilter = string.Empty;
            if (Request.Headers.TryGetValues("username", out headerValues))
            {
                nameFilter = headerValues.FirstOrDefault();
            }

            var eventYears = (from e in db.Events
                                where e.Username == nameFilter
                                select e.EventDate.Year).Distinct();

            return eventYears.OrderByDescending(e => e).ToList();
        }

        // GET: api/Events/5
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> GetEvent(int id)
        {
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            return Ok(@event);
        }

        // PUT: api/Events/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEvent(int id, Event @event)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @event.EventID)
            {
                return BadRequest();
            }

            IEnumerable<string> headerValues;
            string nameFilter = string.Empty;
            if (Request.Headers.TryGetValues("username", out headerValues))
            {
                nameFilter = headerValues.FirstOrDefault();
            }

            @event.EventType = db.EventTypes.Find(@event.EventTypeID).EventTypeName;
            db.Entry(@event).State = EntityState.Modified;

            HomeController.VerifyActiveStatus(db, nameFilter);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // POST: api/Events
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> PostEvent(Event @event)
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

            @event.EventType = db.EventTypes.Find(@event.EventTypeID).EventTypeName;
            db.Events.Add(@event);

            HomeController.VerifyActiveStatus(db, nameFilter);

            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = @event.EventID }, @event);
        }

        // DELETE: api/Events/5
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> DeleteEvent(int id)
        {
            Event @event = await db.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            IEnumerable<string> headerValues;
            string nameFilter = string.Empty;
            if (Request.Headers.TryGetValues("username", out headerValues))
            {
                nameFilter = headerValues.FirstOrDefault();
            }

            db.Events.Remove(@event);
            HomeController.VerifyActiveStatus(db, nameFilter);

            await db.SaveChangesAsync();

            return Ok(@event);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(int id)
        {
            return db.Events.Count(e => e.EventID == id) > 0;
        }
    }
}