namespace backend.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public int WeekId { get; set; }
        public string Day { get; set; } = "";
        public string Title { get; set; } = "";
        public string ActivityType { get; set; } = "Reading"; // Reading, Practice, Project...
        public string Book { get; set; } = "";
        public string Pages { get; set; } = "";
        public string Notes { get; set; } = "";
        public string Status { get; set; } = "Pending"; // Done / InProgress / Pending

        public  Week? Week { get; set; }
    }
}
