using System.Collections.Generic;

namespace ProjectIO.Models.ViewModels
{
    public class TaskDetailViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Timer> Timers { get; set; }
    }
}
