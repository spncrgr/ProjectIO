using System.Collections.Generic;

namespace ProjectIO.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Associations
        public Customer Customer { get; set; }
        public IList<Task> Tasks { get; set; }
    }
}
