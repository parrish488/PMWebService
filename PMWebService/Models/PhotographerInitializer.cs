using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PMWebService.Models
{
    public class PhotographerInitializer : DropCreateDatabaseIfModelChanges<PhotographerContext>
    {
        protected override void Seed(PhotographerContext context)
        {
            //var clients = new List<Client>
            //{
            //    new Client {FirstName="Shanise", LastName="Keith", Street="", City="", State="", Zip="", PrimaryPhone="(801) 822-7220", SecondaryPhone="", Email="shanisekeith90@gmail.com", ContractCompleted="Yes" },
            //    new Client {FirstName="Catherine", LastName="McMaster", Street="", City="", State="", Zip="", PrimaryPhone="", SecondaryPhone="", Email="cmcmaster05@gmail.com", ContractCompleted="Yes" }
            //};

            //foreach (var client in clients)
            //{
            //    context.Clients.Add(client);
            //}

            //var events = new List<Event>
            //{
            //    new Event {EventDate=DateTime.Now, EventType="Wedding", ClientID=1 }
            //};

            //context.SaveChanges();
        }
    }
}