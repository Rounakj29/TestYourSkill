namespace Backend.Models
{
    public class UserRequests
    {
        public int Id { get; set; }
        public List<string>? TechStack {  get; set; }
        public int ? Experience { get; set; }
        public string? RoleOrCompany { get; set; }
        public int? NoOfQuestion { get; set; }
        public string? AdditionalRequirements { get; set; }
    }
}
