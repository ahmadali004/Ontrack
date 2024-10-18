using Ontrack.Models;
using System;
using System.Collections.Generic;

namespace Ontrack.ViewModels
{
    public class StudentAttendanceViewModel
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClassName { get; set; }
        public bool IsPresent { get; set; }  // Indicates if the student is present
        public DateTime Date { get; set; }  // Date of the attendance


    }

  
}
