using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementAPI.Models;

namespace SchoolManagementAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly SchoolContext _context;

        public TeacherController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet("get-all-teachers")]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            return await _context.Teachers.ToListAsync();
        }

        [HttpGet("get-teacher-by-id/{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
            {
                return NotFound();
            }

            return teacher;
        }

        [HttpPost("create-teacher")]
        public async Task<ActionResult<Teacher>> PostTeacher(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeacher", new { id = teacher.TeacherId }, teacher);
        }

        [HttpPut("update-teacher/{id}")]
        public async Task<IActionResult> PutTeacher(int id, Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                return BadRequest();
            }

            _context.Entry(teacher).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Teachers.Any(e => e.TeacherId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(teacher);
        }

        [HttpDelete("delete-teacher/{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return Ok(teacher);
        }
    }
}
