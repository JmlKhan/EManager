using EManager.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace EManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly EmanDbContext _context;

        public DepartmentController(EmanDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> Get()
        {
            try
            {
                var departments = await _context.Departments.ToListAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            
        }
        
        [HttpPost]
        public async Task<ActionResult<Department>> Post([FromBody] Department dep)
        {
            try
            {
                _context.Departments.Add(dep);
                await _context.SaveChangesAsync();
                return Created("created successfully", new
                {
                    DepartmentId = dep.DepartmentId,    
                });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Department>> Edit(int id, [FromBody]Department dep)
        {
            if(id != dep.DepartmentId)
            {
                return BadRequest("Id not found!!!");
            }            

            _context.Entry(dep).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok("Updated successfully");
            }
            catch (Exception ex)
            {

                return BadRequest($"something went wrong: {ex}");
            }

        
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Department>> Delete(int id)
        {
            var department = await _context.Departments.FindAsync(id);

            if(department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
