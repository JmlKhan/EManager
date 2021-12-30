using EManager.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace EManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
            private readonly IConfiguration _configuration;

            public EmployeeController(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            [HttpGet]
            public JsonResult Get()
            {
                string query = @"
                            select EmployeeId, EmployeeName,Department,
                            convert(varchar(10), DateOfJoining,120) as DateofJoining, PhotoFileName
                            from
                            dbo.Employee 
                            ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EManagerCon");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult(table);
            }

            [HttpPost]
            public JsonResult Post(Employee emp)
            {
                string query = @"
                            insert into dbo.Employee
                            (EmployeeName,Department, DateOfJoining,PhotoFileName)
                            values (@EmployeeName, @Department, @DateOfJoining, @PhotoFileName) 
                            ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EManagerCon");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                        myCommand.Parameters.AddWithValue("@Department", emp.Department);
                        myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                        myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult("added successfully");
            }

            [HttpPatch("{id:int}")]
            public JsonResult Patch(int id, Employee emp)
            {

                string query = @"
                            update dbo.Employee 
                            set EmployeeName = @EmployeeName,
                            Department = @Department, 
                            DateOfJoining = @DateOfJoining,
                            PhotoFileName = @PhotoFileName
                            where EmployeeId = @EmployeeId
                            ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EManagerCon");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@EmployeeId", id);
                        myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                        myCommand.Parameters.AddWithValue("@Department", emp.Department);
                        myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                        myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult("updated successfully");
            }

            [HttpDelete("{id:int}")]
            public JsonResult Delete(int id)
            {
                string query = @"
                            delete from dbo.Employee
                            where EmployeeId = @EmployeeId
                            ";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EManagerCon");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@EmployeeId", id);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }

                return new JsonResult("deleted successfully");
            }
        }
}
