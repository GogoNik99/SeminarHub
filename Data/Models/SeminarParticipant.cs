using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeminarHub.Data.Models
{
    [Comment("Mapping Table for IdentityUser(Participants) & Seminar")]
    public class SeminarParticipant
    {
        [Required]
        [Comment("Seminar Identificator")]
        public int SeminarId { get; set; }

        [Required]
        [ForeignKey(nameof(SeminarId))]
        [Comment("Seminar")]
        public Seminar Seminar { get; set; } = null!;

        [Required]
        [Comment("Participant Identificator")]
        public string ParticipantId { get; set; } = string.Empty;

        [Required]
        [ForeignKey(nameof(ParticipantId))]
        [Comment("IdentityUser(Participant)")]
        public IdentityUser Participant { get; set; } = null!;
    }
}
