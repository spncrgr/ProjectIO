using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectIO.Models.ViewModels
{
    public class CreateTaskViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Project Project { get; set; }
        public List<SelectListItem> Projects { get; set; }
        public string SelectedProjectId { get; set; }
    }
}
