
using System.ComponentModel.DataAnnotations;

namespace GymManagementBLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {
        [StringLength(50)]
        public string PlanName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is Required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Description Between 5 and 50")]
        public string Description { get; set; } = string.Empty;
        [Range(1,365)]
        public int DurationDays { get; set; }
        [Range(0.1, 10000)]
        public decimal Price { get; set; }
    }
}

