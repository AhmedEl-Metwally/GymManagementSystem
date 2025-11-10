using GymManagementDAL.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class CreateMemberViewModel
    {
        //[StringLength(maximumLength:50, MinimumLength =3)]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        //[StringLength(maximumLength:100,MinimumLength =5)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Phone Number")]
        [Phone]
        [Required]
        [RegularExpression(@"^(010|011|015|012)\d{8}$")]
        public string Phone { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }
        public Gender Gender  { get; set; }
        //[StringLength(maximumLength: 200, MinimumLength = 1)]
        public int BuildingNumber  { get; set; }
        //[StringLength(maximumLength: 30, MinimumLength = 1)]
        public string Street { get; set; } = string.Empty;
        //[StringLength(maximumLength: 30, MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string City { get; set; } = string.Empty;
        [Required]
        public HealthRecordViewModel HealthRecordViewModel { get; set; } = default!;

    }
}
