using System.ComponentModel.DataAnnotations;

namespace Ontrack.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }

        [Required]
        public int StudentID { get; set; }
        public virtual Student? Student { get; set; } 

        [Required]
        public int ParentID { get; set; }
        public virtual Parent? Parent { get; set; } 

        public bool IsPaid => Amount >= 1700;

      
        public string StudentFullName => Student != null ? $"{Student.FirstName} {Student.LastName}" : "N/A";
        public string ParentFullName => Parent != null ? $"{Parent.FirstName} {Parent.LastName}" : "N/A";
    }
}
