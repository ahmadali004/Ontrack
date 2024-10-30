using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ontrack.ViewModels
{
    public class StudentDetailsViewModel
    {
        public string ParentFullName { get; set; }
        public List<StudentDetailViewModel> Students { get; set; }
        public string SelectedMonth { get; set; }
        public string SelectedWeek { get; set; }
        public List<SelectListItem> MonthOptions { get; set; }
    }
}
