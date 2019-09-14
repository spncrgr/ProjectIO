using System.Collections.Generic;

namespace ProjectIO.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Associations
        public Project Project { get; set; }
        public IList<Timer> Timers { get; set; }
    }
}
