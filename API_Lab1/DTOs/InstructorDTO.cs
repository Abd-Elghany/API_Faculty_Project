using API_Lab1.Models;

namespace API_Lab1.DTOs
{
    public class InstructorDTO
    {
        public int Id { get; set; }                    // Needed for client-side Edit/Delete
        public string Name { get; set; }
        public List<string> Courses { get; set; } = new();
    }

    public class InstructorCreateDTO
    {
        public string Name { get; set; }
        public List<int>? CourseIds { get; set; } = new();  // Optional: assign courses
    }

}
