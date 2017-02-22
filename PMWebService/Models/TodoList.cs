using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PMWebService.Models
{
    public class TodoList
    {
        public int TodoListID { get; set; }

        [Display(Name = "Description")]
        public string ItemDescription { get; set; }

        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        [Display(Name = "Is Completed?")]
        public bool IsCompleted { get; set; }

        public string Username { get; set; }
    }
}