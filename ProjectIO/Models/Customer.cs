using System.Collections.Generic;

namespace ProjectIO.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Associations
        public IList<Project> Projects { get; set; }
    }
}
