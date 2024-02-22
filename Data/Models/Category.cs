using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static SeminarHub.Data.DataConstants;
namespace SeminarHub.Data.Models
{
    [Comment("Seminar Category Class")]
    public class Category
    {
        [Key]
        [Comment("Category Identificator")]
        public int Id { get; set; }

        [Required]
        [MaxLength(CategoryNameMaxLength)]
        [Comment("Category Name")]
        public string Name { get; set; } = string.Empty;
        [Comment("Collection of seminars which belong to this category")]
        public IList<Seminar> Seminars { get; set; } = new List<Seminar>();
    }
}
