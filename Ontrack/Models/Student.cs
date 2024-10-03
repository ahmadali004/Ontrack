using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace Ontrack.Models
{
    public class Student
    {
		[Key]
		public int StudentID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string ? Gender { get; set; }
        public string ? Address { get; set; }
        public string ? PhoneNumber { get; set; }

        public int ClassID { get; set; }
        public Class ? Class { get; set; }

        public int ParentID { get; set; }
        public Parent ? Parent { get; set; }
      
        public ICollection<Subject>? Subjects { get; set; }
        public ICollection<Examination>? Examinations { get; set; }
        public ICollection<Payment>? Payments { get; set; }

        public ICollection<StudentExamResult> StudentExamResults { get; set; }
        public string FullName => $"{FirstName} {LastName}";


        [NotMapped]
        public decimal TuitionAmount { get; set; }

        [NotMapped]
        public bool IsPaid { get; set; }


    }


}
