using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAPI.DataSimulation;
using StudentAPI.Model;
using StudentDataAccessLayer;
using System.Data;

namespace StudentAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/Students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet ("All",Name ="GetAllStudents")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents()
        {
            List<StudentDTO> students = StudentAPIBusinessLayer.Student.GetAllStudent();
            if (students.Count == 0)
                return NotFound("No Students Found!");

            return Ok(students);
            /*if (StudentDataSumulation.StudentsList.Count == 0)
                return NotFound("NO Student To Show");

            return Ok(StudentDataSumulation.StudentsList);*/
        }


        [HttpGet("Passed",Name ="GetPassedStudents")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetPassedStudent()
        {
            List<StudentDTO>passedStudent = StudentAPIBusinessLayer.Student.GetPassedStudent();
            
            if (passedStudent.Count == 0)
                return NotFound("No Passed Students!");

            return Ok(passedStudent);

            /*if (StudentDataSumulation.StudentsList.Count == 0)
                return NotFound("NO Student To Show");

            var passedstudent = StudentDataSumulation.StudentsList.Where(s =>s.Grade >=50).ToList();
            return Ok(passedstudent);*/
        }


        [HttpGet("AverageGrade",Name ="GetAverageGrade")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<double> GetAverageStudent()
        {
            var AvgStudent = StudentAPIBusinessLayer.Student.GetAverageStudent();

            if (AvgStudent == 0)
                return NotFound("No Student to show Average");

            return Ok(AvgStudent);

            /*//StudentDataSumulation.StudentsList.Clear();

            if (StudentDataSumulation.StudentsList.Count is 0)
                return NotFound("No Students Found.");

            var AverageStudent = StudentDataSumulation.StudentsList.Average(s => s.Grade);
            return Ok(AverageStudent);*/
        }


        [HttpGet("{ID}",Name ="GetStudentByID")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDTO> GetStudentByID(int ID)
        {
            if(ID <= 0)
                return BadRequest($"Not Accepted ID {ID}");

            var student = StudentAPIBusinessLayer.Student.Find(ID);
            if (student is null)
                return NotFound($"Student with ID {ID} not found.");

            StudentDTO DTO = student.SDTO;
            return Ok(DTO);
            
        }

        [HttpPost("MyAddded",Name ="AddStudent") ]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<StudentDTO> AddStudent(StudentDTO Newstudent)
        {
            if(Newstudent is null || string.IsNullOrEmpty(Newstudent.Name) || Newstudent.age < 0 || Newstudent.Grade < 0)
                return BadRequest("Invalid Student Data.");

            var student = new StudentAPIBusinessLayer.Student(new StudentDTO(Newstudent.Id, Newstudent.Name, Newstudent.age, Newstudent.Grade));
            student.Save();

            Newstudent.Id = student.Id;

            return CreatedAtRoute("GetStudentByID",new { Id = Newstudent.Id }, Newstudent);
            /*if (Newstudent is null || string.IsNullOrEmpty(Newstudent.Name) || Newstudent.Age < 0 || Newstudent.Grade < 0)
                return BadRequest("Invalid Student Data.");

            Newstudent.Id = StudentDataSumulation.StudentsList.Count > 0 ?
                Newstudent.Id = StudentDataSumulation.StudentsList.Count + 1 : 1;

            StudentDataSumulation.StudentsList.Add(Newstudent);

            return CreatedAtRoute("GetStudentByID", new { ID = Newstudent.Id }, Newstudent);*/
        }

        [HttpDelete("{ID}",Name ="DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult DeleteStudent(int ID)
        {
            if (ID <= 0)
                return BadRequest($"Not Accepted ID {ID}");

            if (StudentAPIBusinessLayer.Student.DeleteStudent(ID))

                return Ok($"Student with ID {ID} has been deleted.");
            else
                return NotFound($"Student with ID {ID} not found. no rows deleted!");


            /*if (ID <= 0) 
                return BadRequest($"Not Accepted ID {ID}");

            var student = StudentDataSumulation.StudentsList.FirstOrDefault(s => s.Id == ID);
            if (student is null)
                return NotFound($"Student with ID {ID} is not found");
            StudentDataSumulation.StudentsList.Remove(student);

            return Ok($"Student with ID {ID} has been deleted.");*/
        }

        [HttpPut("{ID}",Name ="UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDTO> UpdateStudent(int ID,StudentDTO updatedStudent)
        {
            if (ID < 1 || updatedStudent == null || string.IsNullOrEmpty(updatedStudent.Name) || updatedStudent.age < 0 || updatedStudent.Grade < 0)
            {
                return BadRequest("Invalid student data.");
            }

            var student = StudentAPIBusinessLayer.Student.Find(ID);
            
            if (student is null)
                return BadRequest($"Student with ID {updatedStudent.Id} not found.");

            //student = new StudentAPIBusinessLayer.Student(new StudentDTO(id, Updatestudent.Name, Updatestudent.age, Updatestudent.Grade),StudentAPIBusinessLayer.Student.enMode.Update);
            
            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.age;
            student.Grade = updatedStudent.Grade;
            student.Save();

            return Ok(student.SDTO);



            /*if (ID <= 0 || string.IsNullOrEmpty(Updatestudent.Name) || Updatestudent.Age < 1 || Updatestudent.Grade < 1)
                return BadRequest("Invalid student Data.");

            var Student = StudentDataSumulation.StudentsList.FirstOrDefault(s => s.Id ==ID);
            if (Student is null)
                return BadRequest($"Student with ID {ID} not found.");

            Student.Name = Updatestudent.Name;
            Student.Age = Updatestudent.Age;
            Student.Grade = Updatestudent.Grade;

            return Ok(Student);*/

        }
                                              

    }
}
