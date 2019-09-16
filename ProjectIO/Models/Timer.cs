using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ProjectIO.Models
{
    public class Timer : ITimer
    {
        public int Id { get; set; }
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [Display(Name = "Stop Time")]
        public DateTime? StopTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:%h}h {0:%m}m {0:%s}s")]
        public TimeSpan? Duration { get; set; }

        // Associations
        public IdentityUser User { get; set; }
        public Task Task { get; set; }

//        public void StartTimer(int userId, int TaskId)
//        {
//            var taskTimer = new Timer();
//        }
    }

    public interface ITimer
    {
        DateTime StartTime { get; set; }
        DateTime? StopTime { get; set; }
        TimeSpan? Duration { get; set; }
        IdentityUser User { get; set; }
        Task Task { get; set; }
    }
}
