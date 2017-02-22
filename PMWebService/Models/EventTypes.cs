using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMWebService.Models
{
    public class EventTypes
    {
        public int ID { get; set; }

        [Display(Name = "Event Type Name")]
        public string EventTypeName { get; set; }

        public string Username { get; set; }
    }
}