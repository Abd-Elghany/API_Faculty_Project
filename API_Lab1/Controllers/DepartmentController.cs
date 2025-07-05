using API_Lab1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace API_Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private Context context ;
        public DepartmentController(Context _context)
        {
            context = _context;
        }

        [HttpGet]
        public IActionResult All()
        {
            List<Department> departments = context.Departments.ToList();
            return Ok(departments);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Department dept = context.Departments.FirstOrDefault(d => d.Id == id);
            if (dept == null)
            {
                return NotFound("Department is not found.");
            }
            return Ok(dept);
        }

        [HttpPost]
        public IActionResult AddDepartment(Department department)
        {
            context.Departments.Add(department);
            context.SaveChanges();
            return CreatedAtAction("GetById", new { id = department.Id }, department);

            //return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateDepartment(int id, Department department)
        {
            Department dept = context.Departments.FirstOrDefault(d => d.Id == id);
            if (dept == null)
            {
                context.Departments.Add(department);
                context.SaveChanges();
                return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
            }
            else
            {
                dept.Name = department.Name;
                dept.Location = department.Location;
                context.SaveChanges();
                return Ok("Updated ! 👍");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            Department dept = context.Departments.FirstOrDefault(d => d.Id == id);
            if (dept == null)
            {
                return NotFound("Department is not found.");
            }
            context.Departments.Remove(dept);
            context.SaveChanges();
            return Ok("Deleted successfully. 👍");
        }
    }
}
