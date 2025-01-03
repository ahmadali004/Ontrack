﻿using System.ComponentModel.DataAnnotations;
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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }
        public string ? Gender { get; set; }
        public string ? Address { get; set; }
        [Required]
        [RegularExpression(@"^0\d{10}$", ErrorMessage = "Phone number must be exactly 11 digits and start with 0.")]
        public string  PhoneNumber { get; set; }
        public virtual ICollection<Attendance>? Attendances { get; set; }


        public int ClassID { get; set; }
        public Class ? Class { get; set; }

        public int ParentID { get; set; }
        public Parent ? Parent { get; set; }
      
        public ICollection<Subject>? Subjects { get; set; }
        public ICollection<Examination>? Examinations { get; set; }
        public ICollection<Payment>? Payments { get; set; }

		public virtual ICollection<StudentExamsResult>? StudentExamsResult { get; set; }
		public string FullName => $"{FirstName} {LastName}";


        [NotMapped]
        public decimal TuitionAmount { get; set; }

        [NotMapped]
        public bool IsPaid { get; set; }


    }


}
