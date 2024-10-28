using Ontrack.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace Ontrack.Models
{
    public class Parent
    {
		[Key]
		public int ParentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^0\d{10}$", ErrorMessage = "Phone number must be exactly 11 digits and start with 0.")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public ICollection<Student> ? Students { get; set; }
        public ICollection<Payment> ? Payments { get; set; }
		public string UserId { get; set; }
		public virtual OntrackUser User { get; set; }

	}

}
