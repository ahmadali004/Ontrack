namespace Ontrack.ViewModels
{
    public class TeacherLandingPageViewModel
    {
        public string TeacherFullName { get; set; }
        public List<ClassViewModel> Classes { get; set; }
    }

    public class ClassViewModel
    {
        public string ClassName { get; set; }
        //public List<StudentViewModel> Students { get; set; }
        public List<StudentDetailViewModel> Students { get; set; }
    }


}
