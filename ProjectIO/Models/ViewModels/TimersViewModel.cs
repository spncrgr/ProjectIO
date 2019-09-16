using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectIO.Models.ViewModels
{
    public class TimersViewModel : ITimer
    {
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Stop Time")]
        public DateTime? StopTime { get; set; }
        [DisplayFormat(DataFormatString = "h'h' m'm' s's'")]
        public TimeSpan? Duration { get; set; }
        public IdentityUser User { get; set; }
        public List<SelectListItem> Users { get; set; }
        public string SelectedUserId { get; set; }
        public Task Task { get; set; }
        public List<SelectListItem> Tasks { get; set; }
        public string SelectedTaskId { get; set; }

    }
}
