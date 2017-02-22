using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMWebService.Models
{
    public class Client
    {
        public Client()
        {
            Events = new HashSet<Event>();
            Billing = new HashSet<Billing>();
            Mileage = new HashSet<Mileage>();
        }

        [Display(Name = "Client")]
        public int ClientID { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        [Display(Name = "Primary Phone")]
        public string PrimaryPhone { get; set; }
        [Display(Name = "Secondary Phone")]
        public string SecondaryPhone { get; set; }
        public string Email { get; set; }
        [Display(Name = "Client Notes")]
        [DataType(DataType.MultilineText)]
        public string ClientNotes { get; set; }
        [Display(Name = "Client Status")]
        public string Status { get; set; }
        public string Username { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Billing> Billing { get; set; }
        public virtual ICollection<Mileage> Mileage { get; set; }

        public List<TodoTemplate> TodoTemplates { get; set; }
    }
}