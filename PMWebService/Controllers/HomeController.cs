using PMWebService.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMWebService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public static void VerifyActiveStatus(PhotographerContext db, string user)
        {
            HashSet<int> activeClients = new HashSet<int>();

            var eventList = db.Events.Where(e => e.Username == user);

            foreach (Event clientEvent in eventList)
            {
                if (clientEvent.EventDate > DateTime.Now.AddMonths(-1))
                {
                    activeClients.Add(clientEvent.ClientID);
                }
            }

            var clientList = db.Clients.Where(c => c.Username == user);

            foreach (Client client in clientList)
            {
                if (activeClients.Contains(client.ClientID))
                {
                    client.Status = "Active";
                }
                else
                {
                    client.Status = "Inactive";
                }

                db.Entry(client).State = EntityState.Modified;
            }

            db.SaveChanges();
        }
    }
}
