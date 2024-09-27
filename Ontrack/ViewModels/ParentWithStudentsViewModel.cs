using Ontrack.Models;
using System.Collections.Generic;

namespace Ontrack.ViewModels
{
    public class ParentWithStudentsViewModel
    {
        public Parent Parent { get; set; }
        public List<Student> Students { get; set; } = new List<Student>();

        public decimal GetTuitionAmount()
        {
            decimal totalTuition = 0;

            // Define tuition amounts based on the child index
            for (int i = 0; i < Students.Count; i++)
            {
                if (i == 0)
                {
                    totalTuition += 1700; // First child
                }
                else if (i == 1 || i == 2)
                {
                    totalTuition += 1500; // Second and third child
                }
                else
                {
                    totalTuition += 1300; // Fourth child and beyond
                }
            }

            return totalTuition;
        }

        public bool AllChildrenPaid()
        {
            return Students.All(student => student.Payments.Any(payment => payment.IsPaid));
        }
    }
}
