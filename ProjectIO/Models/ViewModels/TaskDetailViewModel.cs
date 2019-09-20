using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectIO.Models.ViewModels
{
    public class TaskDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Total Duration")]
        public TimeSpan? TotalDuration { get; set; }
        public IList<Timer> Timers { get; set; }
    }
}
