using Microsoft.AspNetCore.Mvc.Rendering;
using Ontrack.Models;

namespace Ontrack.ViewModels
{
    public class StudentDetailsViewModel
    {
        public string ParentFullName { get; set; }
        public List<StudentDetailViewModel> Students { get; set; }
        public int? SelectedStudentID { get; set; }
        public Student SelectedStudent { get; set; }
        public List<StudentDetailViewModel> ExamResults { get; set; }
        public string SelectedMonth { get; set; }
        public string SelectedWeek { get; set; }
        public int SelectedYear { get; set; }
        public List<SelectListItem> MonthOptions { get; set; }
        public List<StudentDetailViewModel> StudentDetails { get; internal set; }
    }

}
