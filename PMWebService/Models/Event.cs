using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PMWebService.Models
{
    public class Event
    {
        public int EventID { get; set; }
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; }
        [Display(Name = "Event Type")]
        public string EventType { get; set; }
        [Display(Name = "Contract Status")]
        public string ContractCompleted { get; set; }
        public string Username { get; set; }

        [Display(Name = "Event")]
        public string EventLabel
        {
            get
            {
                return string.Format("{0}, {1} - {2}", Client.FullName, EventType, EventDate.ToShortDateString());
            }
        }

        [Display(Name = "Event Type")]
        [ForeignKey("EventTypes")]
        public int EventTypeID { get; set; }
        public virtual EventTypes EventTypes { get; set; }

        [ForeignKey("Client")]
        public int ClientID { get; set; }
        public virtual Client Client { get; set; }

        public List<Client> ClientIDs { get; set; }
        public List<EventTypes> EventTypeIDs { get; set; }
        public List<TodoTemplate> TodoTemplates { get; set; }
    }
}