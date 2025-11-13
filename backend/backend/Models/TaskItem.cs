namespace backend.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public int WeekId { get; set; }
        public string Day { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public string ActivityType { get; set; } = "Reading"; // Reading, Practice, Project...
        public string ActivityName { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending"; // Done / InProgress / Pending
        public  Week? Week { get; set; }
    }
}
