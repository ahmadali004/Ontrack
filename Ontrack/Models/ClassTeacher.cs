using System.ComponentModel.DataAnnotations;

namespace Ontrack.Models
{
   
        public class ClassTeacher
        {
		[Key]
		public int ClassTeacherID { get; set; }

            public int ClassID { get; set; }
            public Class ? Class { get; set; }

            public int ? TeacherID { get; set; }
            public Teacher ? Teacher { get; set; }
        }
    


}
