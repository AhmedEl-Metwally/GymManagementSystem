using GymManagementDAL.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class CreateMemberViewModel
    {
        [Range(2, 50)]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Range(5,100)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        [Phone]
        [Required]
        [RegularExpression(@"^(010|011|015|012)\d{8}$")]
        public string Phone { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }
        public Gender Gender  { get; set; }
        [Range(1, 200)]
        public int BuildingNumber  { get; set; }
        [Range(1, 30)]
        public string Street { get; set; } = string.Empty;
        [Range(1, 30)]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string City { get; set; } = string.Empty;
        [Required]
        public HealthRecordViewModel HealthRecordViewModel { get; set; } = default!;

    }
}
