namespace Ontrack.Models
{
    public class Attendance
    {
        public int AttendanceID { get; set; }
        public int StudentID { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }

        public Student Student { get; set; }  
    }

}
