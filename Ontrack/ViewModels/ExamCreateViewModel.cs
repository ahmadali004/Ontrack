using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ontrack.ViewModels
{
    public class ExamCreateViewModel
    {
        public int SubjectID { get; set; }
        public SelectList Subjects { get; set; }
    }

}
