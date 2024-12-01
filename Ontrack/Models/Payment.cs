using System.ComponentModel.DataAnnotations;

namespace Ontrack.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        public decimal Amount { get; set; } // Amount paid by the parent
        public decimal TuitionAmount { get; set; } // Set the tuition amount based on child order

        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }

        [Required]
        public int StudentID { get; set; }
        public Student? Student { get; set; }

        [Required]
        public int ParentID { get; set; }
        public Parent Parent { get; set; }

        public bool IsPaid => Amount >= TuitionAmount; 

        public string StudentFullName => Student != null ? $"{Student.FirstName} {Student.LastName}" : "N/A";
        public string ParentFullName => Parent != null ? $"{Parent.FirstName} {Parent.LastName}" : "N/A";

        
        public static decimal CalculateTuition(int childOrder)
        {
            return childOrder switch
            {
                1 => 1700m,
                2 => 1500m,
                3 => 1500m,
                4 => 1300m,
                5=>1300m,
                6=>1300m,
                _ => throw new NotImplementedException(),
            };
        }

    }

}
