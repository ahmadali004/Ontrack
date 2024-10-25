using Microsoft.AspNetCore.Mvc.Rendering;
using Ontrack.Models;
using System.Collections.Generic;

namespace Ontrack.ViewModels
{
    public class StudentPaymentViewModel
    {
        public int StudentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClassName { get; set; }
        public decimal PaymentAmount { get; set; }
        public int ParentID { get; set; }
        public DateTime DOB { get; set; }
        public bool HasPaid { get; set; }

    }

    public class SelectedParentViewModel
    {
        public IEnumerable<Parent> Parents { get; set; }
        public Parent Parent { get; set; }
        public List<StudentPaymentViewModel> Students { get; set; }  // Changed to List
        public SelectList Months { get; set; }
        public SelectList Years { get; set; }
        public int SelectedMonth { get; set; }

        public int SelectedYear { get; set; }
    }
}
