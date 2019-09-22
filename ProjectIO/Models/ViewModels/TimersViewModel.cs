using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectIO.Areas.Identity;

namespace ProjectIO.Models.ViewModels
{
    public class TimersViewModel : ITimer
    {
        public int Id { get; set; }
        [Display(Name = "Start Time")]
        [DisplayFormat(DataFormatString = "{0:%h}h {0:%m}m {0:%s}s")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Stop Time")]
        public DateTime? StopTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:%h}h {0:%m}m {0:%s}s")]
        public TimeSpan? Duration { get; set; }
        public ApplicationUser User { get; set; }
        public List<SelectListItem> Users { get; set; }
        public string SelectedUserId { get; set; }
        public Task Task { get; set; }
        public List<SelectListItem> Tasks { get; set; }
        public string SelectedTaskId { get; set; }

    }
}
