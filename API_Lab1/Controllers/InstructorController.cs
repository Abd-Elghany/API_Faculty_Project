using API_Lab1.DTOs;
using API_Lab1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : ControllerBase
    {
        private readonly Context _context;

        public InstructorController(Context context)
        {
            _context = context;
        }

        // GET: api/instructor
        [HttpGet]
        public IActionResult GetInstructors()
        {
            var instructors = _context.Instructors
                .Include(i => i.Courses)
                .Select(i => new InstructorDTO
                {
                    Id = i.Id,
                    Name = i.Name,
                    Courses = i.Courses.Select(c => c.Name).ToList()
                })
                .ToList();

            return Ok(instructors);
        }

        // GET: api/instructor/{id}
        [HttpGet("{id}")]
        public IActionResult GetInstructorById(int id)
        {
            var instructor = _context.Instructors
                .Include(i => i.Courses)
                .FirstOrDefault(i => i.Id == id);

            if (instructor == null)
                return NotFound("Instructor not found.");

            var dto = new InstructorDTO
            {
                Id = instructor.Id,
                Name = instructor.Name,
                Courses = instructor.Courses.Select(c => c.Name).ToList()
            };

            return Ok(dto);
        }

        // POST: api/instructor
        [HttpPost]
        public IActionResult AddInstructor(InstructorCreateDTO dto)
        {
            var instructor = new Instructor
            {
                Name = dto.Name,
                Courses = dto.CourseIds != null && dto.CourseIds.Count > 0
                    ? _context.Courses.Where(c => dto.CourseIds.Contains(c.Id)).ToList()
                    : new List<Course>()
            };

            _context.Instructors.Add(instructor);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetInstructorById), new { id = instructor.Id }, new { instructor.Id });
        }

        // PUT: api/instructor/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateInstructor(int id, InstructorCreateDTO dto)
        {
            var instructor = _context.Instructors
                .Include(i => i.Courses)
                .FirstOrDefault(i => i.Id == id);

            if (instructor == null)
                return NotFound("Instructor not found.");

            instructor.Name = dto.Name;
            instructor.Courses = dto.CourseIds != null && dto.CourseIds.Count > 0
                ? _context.Courses.Where(c => dto.CourseIds.Contains(c.Id)).ToList()
                : new List<Course>();

            _context.SaveChanges();
            return Ok("Updated!");
        }

        // DELETE: api/instructor/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteInstructor(int id)
        {
            var instructor = _context.Instructors
                .Include(i => i.Courses)
                .FirstOrDefault(i => i.Id == id);

            if (instructor == null)
                return NotFound("Instructor not found.");

            _context.Instructors.Remove(instructor);
            _context.SaveChanges();
            return Ok("Deleted!");
        }
    }
}
