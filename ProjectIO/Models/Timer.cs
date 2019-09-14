using System;
using Microsoft.AspNetCore.Identity;

namespace ProjectIO.Models
{
    public class Timer
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }

        // Associations
        public IdentityUser User { get; set; }
        public Task Task { get; set; }

//        public void StartTimer(int userId, int TaskId)
//        {
//            var taskTimer = new Timer();
//        }
    }
}
