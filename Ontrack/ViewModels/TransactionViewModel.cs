namespace Ontrack.ViewModels
{
    public class TransactionViewModel
    {
        public int PaymentID { get; set; }
        public string ParentName { get; set; }
        public string StudentName { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
    }
}
