namespace backend.DTO
{
    public class PlanDto
    {
        public string Name { get; set; } = string.Empty;
        public int DurationWeeks { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
