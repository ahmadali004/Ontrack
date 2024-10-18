using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ontrack.ViewModels
{
    public class StudentExamViewModel
    {
        public int SelectedExamID { get; set; }
        public IEnumerable<SelectListItem> Exams { get; set; }
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public double Score { get; set; } // Adjust based on your requirements

        public StudentExamViewModel()
        {
            Exams = new List<SelectListItem>();
        }
    }
}
