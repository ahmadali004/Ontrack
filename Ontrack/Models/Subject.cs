using System.ComponentModel.DataAnnotations;

namespace Ontrack.Models
{
    public class Subject
    {
		[Key]
		public int SubjectID { get; set; }
        public string Name { get; set; }
    }

}
