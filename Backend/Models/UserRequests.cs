namespace Backend.Models
{
    public class UserRequests
    {
        public int Id { get; set; }
        public List<string>? TechStack { get; set; }
        public int? Experience { get; set; }
        public string? RoleOrCompany { get; set; }
        public int? DifficultyLevel { get; set; } // 1: Easy, 2: Medium, 3: Hard, 4: All
        public string? QuestionType { get; set; } // Short, Long, MCQ, Code, Others Optinal
        public int? NoOfQuestion { get; set; }
        public string? AdditionalRequirements { get; set; }
    }
}
