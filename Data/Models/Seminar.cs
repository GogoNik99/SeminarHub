using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static SeminarHub.Data.DataConstants;

namespace SeminarHub.Data.Models
{
    [Comment("Seminar Class")]
    public class Seminar
    {
        [Key]
        [Comment("Seminar Identificator")]
        public int Id { get; set; }
        [Required]
        [Comment("Seminar Topic")]
        [MaxLength(SeminarTopicMaxLength)]
        public string Topic { get; set; } = string.Empty;

        [Required]
        [Comment("Seminar Lecturer")]
        [MaxLength(SeminarLecturerMaxLength)]
        public string Lecturer { get; set; } = string.Empty;

        [Required]
        [Comment("Seminar Details")]
        [MaxLength(SeminarDetailsMaxLength)]
        public string Details { get; set; } = string.Empty;

        [Required]
        [Comment("Seminar Organizer Identificator")]
        public string OrganizerId { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(OrganizerId))]
        [Comment("Seminar Organizer")]
        public IdentityUser Organizer { get; set; } = null!;

        [Required]
        [Comment("Seminar Date and Time")]
        public DateTime DateAndTime { get; set; }

        [Required]
        [Comment("Seminar duration")]
        [Range(SeminarDurationMinLength, SeminarDetailsMaxLength)]
        public int Duration { get; set; }

        [Required]
        [Comment("Seminar Category Identificator")]
        public int CategoryId { get; set; }

        [Required]
        [ForeignKey(nameof(CategoryId))]
        [Comment("Seminar Category")]
        public Category Category { get; set; } = null!;
        [Comment("Seminar collection of participants")]
        public IList<SeminarParticipant> SeminarsParticipants { get; set; } = new List<SeminarParticipant>();


    }
}
