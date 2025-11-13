namespace backend.Models
{
    public class Week
    {
        public int Id { get; set; }
        public int PlanId { get; set; }
        public int WeekNumber { get; set; }
        public string Summary { get; set; } = string.Empty;
        public double Progress { get; set; }
        public string Rating { get; set; } = string.Empty;
        public virtual Plan? Plan { get; set; }
        public virtual List<TaskItem>? Tasks { get; set; }
    }
}
