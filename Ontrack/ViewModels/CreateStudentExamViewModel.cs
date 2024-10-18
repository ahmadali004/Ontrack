using Microsoft.AspNetCore.Mvc.Rendering;
using Ontrack.Models;

namespace Ontrack.ViewModels
{
    public class CreateStudentExamViewModel
    {
        public StudentExamsResult? ExamResult { get; set; }
        public List<SelectListItem>? Classes { get; set; }
        public List<SelectListItem>? Subjects { get; set; }
        public List<SelectListItem> ?Students { get; set; } 
        public List<SelectListItem> ?Examinations { get; set; }
    }

}
