using Microsoft.AspNetCore.Mvc.Rendering;
using Ontrack.Models;
using Ontrack.ViewModels;
namespace Ontrack.ViewModels
{
    public class ExpenseViewModel
    {
        public decimal TotalExpenses { get; set; }
        public decimal TotalPayments { get; set; }
        public decimal Balance { get; set; }
        public List<Expense> Expenses { get; set; }
        public List<Payment> Payments { get; set; }
        public int SelectedMonth { get; set; }
        public int SelectedYear { get; set; }

        // Populate a list of months and years for selection
        public IEnumerable<SelectListItem> Months { get; set; }
        public IEnumerable<SelectListItem> Years { get; set; }
    }


}
