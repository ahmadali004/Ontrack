using System.ComponentModel.DataAnnotations;
namespace Ontrack.Models
{
    public class StudentExamResult
    {
        [Key]
        public int StudentExamResultID { get; set; }

        public decimal Score { get; set; }

        public int StudentID { get; set; }
        public Student? Student { get; set; }

        public int ExaminationID { get; set; }
        public Examination? Examination { get; set; }

    }

}





