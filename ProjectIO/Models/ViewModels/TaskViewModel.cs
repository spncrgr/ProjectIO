using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectIO.Models.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Total Duration")]
        public TimeSpan? TotalDuration { get; set; }
        public IList<Timer> Timers { get; set; }
        public Project Project { get; set; }
        public List<SelectListItem> Projects { get; set; }
        public string SelectedProjectId { get; set; }
    }
}
