namespace API_Lab1.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Course>? Courses { get; set; } = new();
    }
}
