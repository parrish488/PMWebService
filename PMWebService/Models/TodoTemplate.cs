using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMWebService.Models
{
    public class TodoTemplate
    {
        public int TodoTemplateID { get; set; }

        [Display(Name = "Description")]
        public string TemplateDescription { get; set; }

        [Display(Name = "Range Units")]
        public int RangeUnits { get; set; }

        [Display(Name = "Range Value")]
        public string RangeValue { get; set; }

        public string Username { get; set; }
    }
}