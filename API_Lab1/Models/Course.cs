using System.ComponentModel.DataAnnotations.Schema;

namespace API_Lab1.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }
        public Instructor? Instructor { get; set; }
    }
}
