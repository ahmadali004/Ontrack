
using Ontrack.Models;

namespace Ontrack.ViewModels
{
    public class SelectedParentViewModel
    {
        public Parent Parent { get; set; }
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Payment> Payments { get; set; }
    }
}
