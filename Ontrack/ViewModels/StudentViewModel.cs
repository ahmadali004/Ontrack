using Ontrack.Models;

namespace Ontrack.ViewModels
{
    public class StudentViewModel
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClassName { get; set; }
        public decimal PaymentAmount { get; set; }
        public List<Attendance> AttendanceRecords { get; set; }
        public List<Payment> Payments { get; set; }
        public List<StudentExamsResult> ExamResults { get; set; }

        public string FullName => $"{FirstName} {LastName}";

    }
}
