using EManager.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace EManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmanDbContext _context;
        private readonly IWebHostEnvironment _env;

            public EmployeeController(EmanDbContext context, IWebHostEnvironment env)
            {
                _context= context;
                _env = env;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Employee>>> Get()
            {
                try
                {
                    var employees = await _context.Employees.ToListAsync();
                    return Ok(employees);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpPost]
            public async Task<ActionResult<Employee>> Post([FromBody] Employee employee)
            {
                 try
                 {
                    _context.Employees.Add(employee);
                    await _context.SaveChangesAsync();
                    return Ok();
                 }
                 catch (Exception ex)
                 {

                    return BadRequest(ex.Message);
                 }
               
            }

            [HttpPatch("{id:int}")]
            public async Task<ActionResult<Employee>> Edit(int id, [FromBody] Employee employee)
            {
                if(id != employee.EmployeeId)
                {
                    return BadRequest("Id not found");
                }

                _context.Entry(employee).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                    return Ok("updated sucessfully");
                }
                catch (Exception ex)
                {

                    return BadRequest(ex.Message);
                }

            }

            [HttpDelete("{id:int}")]
            public async Task<ActionResult<Employee>> Delete(int id)
            {
                var employee = await _context.Employees.FindAsync(id);
                
                if(employee == null)
                {
                    return NotFound();
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                
                return NoContent();               
            }

            [Route("SaveFile")]
            [HttpPost]
            public JsonResult SaveFile()
            {
                try
                {
                    var httpRequest = Request.Form;
                    var postedFile = httpRequest.Files[0];
                    string filename = postedFile.FileName;
                    var phisicalPath = $"{_env.ContentRootPath}/Photos/{filename}";
                    
                    using(var stream = new FileStream(phisicalPath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }

                    return new JsonResult(filename);
                }
                catch (Exception)
                {
                    return new JsonResult("anonymous.png");
                }

            }
    }
}
