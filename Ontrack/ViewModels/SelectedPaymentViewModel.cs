using Microsoft.AspNetCore.Mvc.Rendering;
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

        public int SelectedYear { get; set; }

        // A list of students with payment information
        public List<StudentPaymentViewModel> Students { get; set; }

        // List of months for dropdown selection
       


    }

    // ViewModel for representing month data in dropdown
    public class MonthItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
