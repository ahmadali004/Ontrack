using Microsoft.AspNetCore.Mvc.Rendering;
using Ontrack.Models;
namespace Ontrack.ViewModels
{
    public class ClassExaminationViewModel
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; }
        public DateTime ExamDate { get; set; }
        public string ClassName { get; set; }
        public IEnumerable<Class> Classes { get; set; } // List of classes
        public int? SelectedExamID { get; set; }
        public List<SelectListItem> Exams { get; set; }
        public Dictionary<int, StudentExamResultViewModel> Results { get; set; }


        // Instead of SelectListItem, use StudentExamViewModel for detailed data
        public List<StudentExamViewModel> Students { get; set; }
        public StudentExamsResult ExamResult { get; internal set; }
        public List<SelectListItem> Subjects { get; internal set; }

        public ClassExaminationViewModel()
        {
            Exams = new List<SelectListItem>();
            Students = new List<StudentExamViewModel>();
        }

     
    }
    public class StudentExamResultViewModel
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }  // Assuming you want to show student names
        public int Score { get; set; }           // The score of the student
        public string ExamName { get; set; }
        public string ExamDate { get; set; }

        public Examination Examination { get; internal set; }
        public StudentExamsResult StudentExamsResult { get; internal set; }
        public Student Student { get; internal set; }


    }
}