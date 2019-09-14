using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectIO.Models.ViewModels
{
    public class TimersViewModel
    {
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public IdentityUser User { get; set; }
        public List<SelectListItem> Users { get; set; }
        public string SelectedUserId { get; set; }
        public Task Task { get; set; }
        public List<SelectListItem> Tasks { get; set; }
        public string SelectedTaskId { get; set; }

    }
}
