using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectIO.Models.ViewModels
{
    public class ProjectsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Customer Customer { get; set; }
        public List<SelectListItem> Customers { get; set; }
        public string SelectedCustomerId { get; set; }
    }
}
