using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolManagementAPI.Models;

namespace SchoolManagementAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly SchoolContext _context;

        public ClassController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet("get-all-classes")]
        public async Task<ActionResult<IEnumerable<Class>>> GetClasses()
        {
            var classes = await _context.Classes.Include(c => c.Teacher).ToListAsync();
            return Ok(classes);
        }

        [HttpGet("get-class-by-id")]
        public async Task<ActionResult<Class>> GetClass([FromQuery]int id)
        {
            var classObj = await _context.Classes
                                         .Include(c => c.Teacher)
                                         .FirstOrDefaultAsync(c => c.ClassId == id);

            if (classObj == null)
            {
                return NotFound();
            }

            return classObj;
        }


        [HttpPost("create-class")]
        public async Task<ActionResult<Class>> PostClass([FromBody]Class classObj)
        {
            var teacher = await _context.Teachers.FindAsync(classObj.TeacherId);

            if (teacher == null)
            {
                return NotFound("Teacher not found");
            }

            classObj.Teacher = teacher;

            _context.Classes.Add(classObj);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClass", new { id = classObj.ClassId }, classObj);
        }



        [HttpPut("update-class/{id}")]
        public async Task<IActionResult> PutClass([FromRoute]int id, [FromBody]Class classObj)
        {
            if (id != classObj.ClassId)
            {
                return BadRequest();
            }

            var classInDb = await _context.Classes
                .Include(c => c.Teacher) 
                .FirstOrDefaultAsync(c => c.ClassId == id);

            if (classInDb == null)
            {
                return NotFound();
            }

            classInDb.ClassName = classObj.ClassName;
            classInDb.TeacherId = classObj.TeacherId;

            if (classObj.Teacher != null)
            {
                classInDb.Teacher = classObj.Teacher;
            }

            _context.Entry(classInDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Classes.Any(e => e.ClassId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(classObj);
        }



        [HttpDelete("delete-class/{id}")]
        public async Task<IActionResult> DeleteClass([FromRoute]int id)
        {
            var classObj = await _context.Classes
                .Include(c => c.Teacher) 
                .FirstOrDefaultAsync(c => c.ClassId == id);

            if (classObj == null)
            {
                return NotFound();
            }

            _context.Classes.Remove(classObj);
            await _context.SaveChangesAsync();

            return Ok(classObj); 
        }

    }
}
