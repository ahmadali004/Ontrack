using Ontrack.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [RegularExpression(@"^0\d{10}$", ErrorMessage = "Phone number must be exactly 11 digits and start with 0.")]
        public string PhoneNumber { get; set; }
		[Required]
		public string Email { get; set; }
        public decimal ? Salary { get; set; }
        public ICollection<ClassTeacher> ? ClassTeachers { get; set; }
		
		public ICollection<Subject>? Subjects { get; set; }

                    public ICollection<Class>? Classes { get; set; }
        public string FullName => $"{FirstName} {LastName}";

        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual OntrackUser User { get; set; }

    }


}
