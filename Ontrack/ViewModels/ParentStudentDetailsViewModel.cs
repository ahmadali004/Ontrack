
using Ontrack.Models;
using System;
using System.Collections.Generic;

namespace Ontrack.ViewModels
{
    public class StudentDetailViewModel
    {
        public int StudentID { get; set; }
        public string FullName { get; set; }
        public string ClassName { get; set; }
        public List<Attendance> AttendanceRecords { get; set; }
        public List<Payment> Payments { get; set; }
       
        public List<StudentExamResultViewModel> ExamResults { get; set; }
    }

    

    public class AttendanceRecord
    {
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
    public class StudentGrade
    {
        public string Subject { get; set; }
        public decimal Score { get; set; } // Make sure Score is present
    }
}


