using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Ontrack.Models
{
    public class Teacher
    {
		[Key]
		public int TeacherID { get; set; }
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string PhoneNumber { get; set; }
		[Required]
		public string Email { get; set; }
	
        public ICollection<ClassTeacher> ? ClassTeachers { get; set; }
		
		public ICollection<Subject>? Subjects { get; set; }

                    public ICollection<Class>? Classes { get; set; }
        public string FullName => $"{FirstName} {LastName}";

    }


}
