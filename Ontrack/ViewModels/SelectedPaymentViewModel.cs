using Ontrack.Models;
using System;
using System.Collections.Generic;

namespace Ontrack.ViewModels
{
    // ViewModel for displaying student details with payment information
   
  

    // ViewModel for handling payment-related data
    public class SelectedPaymentViewModel
    {
        public int SelectedMonth { get; set; }

        // A list of students with payment information
        public List<StudentPaymentViewModel> Students { get; set; }

        // List of months for dropdown selection
        public List<MonthItem> Months { get; set; }

       
    }

    // ViewModel for representing month data in dropdown
    public class MonthItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
