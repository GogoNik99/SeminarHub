using System.ComponentModel.DataAnnotations;
using static SeminarHub.Data.DataConstants;
namespace SeminarHub.Models
{
    public class SeminarFormViewModel
    {
        [Required(ErrorMessage = RequireErrorMessage)]
        [StringLength(SeminarTopicMaxLength,
            MinimumLength = SeminarTopicMinLength,
            ErrorMessage = StringLengthErrorMessage)]
        public string Topic { get; set; } = string.Empty;

        [Required(ErrorMessage = RequireErrorMessage)]
        [StringLength(SeminarLecturerMaxLength,
            MinimumLength = SeminarLecturerMinLength,
            ErrorMessage = StringLengthErrorMessage)]
        public string Lecturer { get; set; } = string.Empty;

        [Required(ErrorMessage = RequireErrorMessage)]
        [StringLength(SeminarDetailsMaxLength,
            MinimumLength = SeminarDetailsMinLength,
            ErrorMessage = StringLengthErrorMessage)]
        public string Details { get; set; } = string.Empty;

        [Required(ErrorMessage = RequireErrorMessage)]
        public string DateAndTime { get; set; } = string.Empty;

        [Required(ErrorMessage = RequireErrorMessage)]
        [Range(SeminarDurationMinLength,
           SeminarDurationMaxLength,
           ErrorMessage = IntRangeErrorMessage)]
        public int Duration { get; set; }

        [Range(CategoryMinRange, int.MaxValue)]
        public int CategoryId { get; set; }
        public IEnumerable<CategoriesViewModel> Categories { get; set; } = new List<CategoriesViewModel>();
    }
}
