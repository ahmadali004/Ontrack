using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ontrack.Models
{
    public class StudentExamsResult
    {
        [Key]
        public int StudentExamResultID { get; set; }
        public decimal Score { get; set; }

        // Foreign Key for Student
        public int ?StudentID { get; set; }
        [ForeignKey("StudentID")]
        public virtual Student? Student { get; set; }

        // Foreign Key for Examination
        public int? ExaminationID { get; set; }
        [ForeignKey("ExaminationID")]
        public virtual Examination? Examination { get; set; }


        // Foreign Key for Class
        public int? ClassID { get; set; }
        [ForeignKey("ClassID")]
        public virtual Class? Class { get; set; }

    }
}
