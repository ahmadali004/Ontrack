using System.ComponentModel.DataAnnotations;

namespace Ontrack.Models
{
    public class Class
    {
		[Key]
		public int ClassID { get; set; }
        public string ClassName { get; set; }

        public Teacher ? teacher { get; set; }

        public ICollection<Student> ? Students { get; set; }
        public ICollection<ClassTeacher> ? ClassTeachers { get; set; }
        public ICollection<Examination> ? Examinations { get; set; }
    }

}
