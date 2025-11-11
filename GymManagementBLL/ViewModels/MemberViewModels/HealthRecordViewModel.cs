using System.ComponentModel.DataAnnotations;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class HealthRecordViewModel
    {
        [Range(0.1,300)]
        public decimal Height { get; set; }
        [Range(0.1, 500)]
        public decimal Weight { get; set; }
        public string BloodType { get; set; } = string.Empty;
        public string? Note { get; set; }

    }
}
