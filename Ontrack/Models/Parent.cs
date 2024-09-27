using System.ComponentModel.DataAnnotations;

namespace Ontrack.Models
{
    public class Parent
    {
		[Key]
		public int ParentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public ICollection<Student> ? Students { get; set; }
        public ICollection<Payment> ? Payments { get; set; }

    }

}
