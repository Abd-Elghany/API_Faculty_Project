using API_Lab1.DTOs;
using API_Lab1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly Context context;

        public CourseController(Context _context)
        {
            context = _context;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetCourses()
        {
            var courses = context.Courses.Include(c => c.Instructor)
                .Select(c => new CourseDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Instructor = c.Instructor != null ? c.Instructor.Name : "No Instructor"
                }).ToList();
            return Ok(courses);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddCourse(CourseCreateDTO dto)
        {
            Instructor instructor = context.Instructors.FirstOrDefault(i => i.Id == dto.InstructorId);
            if (instructor == null)
                return BadRequest("Instructor not found.");

            var course = new Course
            {
                Name = dto.Name,
                InstructorId = dto.InstructorId
            };

            context.Courses.Add(course);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetCourseById), new { id = course.Id }, new CourseDTO
            {
                Id = course.Id,
                Name = course.Name,
                Instructor = context.Instructors.Find(course.InstructorId)?.Name ?? "Unknown"
            });
        }


        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetCourseById(int id)
        {
            var course = context.Courses.Include(c => c.Instructor).FirstOrDefault(c => c.Id == id);
            if (course == null)
                return NotFound("Course not found.");

            return Ok(new CourseDTO
            {
                Id = course.Id,
                Name = course.Name,
                Instructor = course.Instructor?.Name ?? "Unknown"
            });
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateCourse(int id, CourseCreateDTO dto)
        {
            var existing = context.Courses.FirstOrDefault(c => c.Id == id);
            if (existing == null)
                return NotFound("Course not found.");

            existing.Name = dto.Name;
            existing.InstructorId = dto.InstructorId;
            context.SaveChanges();
            return Ok("Updated!");
        }


        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var course = context.Courses.FirstOrDefault(c => c.Id == id);
            if (course == null) return NotFound("Course not found.");

            context.Courses.Remove(course);
            context.SaveChanges();
            return Ok("Deleted!");
        }
    }
}
