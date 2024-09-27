using System.ComponentModel.DataAnnotations;

namespace Ontrack.Models
{
    public class Examination
    {
        [Key]
        public int ExaminationID { get; set; }

        public string ExamName { get; set; }
        public DateTime Date { get; set; }
        public decimal Score { get; set; }

        public int ClassID { get; set; }
        public Class? Class { get; set; }

        public int SubjectID { get; set; }
        public Subject? Subject { get; set; }

        //
        //
        public ICollection<StudentExamResult> ? StudentExamResults { get; set; }
    }
}