namespace backend.DTO
{
    public class TaskDto
    {
        public string? Day { get; set; } // e.g., "Monday"
        public string? Industry { get; set; }
        public string? ActivityType { get; set; } // Reading, Practice, Project...
        public string? ActivityName { get; set; }
        public string? Target { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; } = "Pending";
    }
}
