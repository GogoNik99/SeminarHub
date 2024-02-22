namespace SeminarHub.Models
{
    public class SeminarJoinedViewModel
    {
        public int Id { get; set; }

        public string Topic { get; set; } = string.Empty;

        public string Lecturer { get; set; } = string.Empty;

        public string DateAndTime { get; set; } = string.Empty;

        public string Organizer { get; set; } = string.Empty;
    }
}
