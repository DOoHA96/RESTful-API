using System;
using System.Collections;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;




namespace new_ConsoleDotNet
{

    public partial class Program
    {
        static readonly HttpClient httpClient = new HttpClient();
        static async Task  Main(string[] args)
        {
            httpClient.BaseAddress = new Uri("http://localhost:5254/api/Students/");


            await GetAllStudetns();
            await GetPassedStudents(); 
            await GetAverageGrade();
            await GetStudentByID(1);
            await GetStudentByID(20);
            await GetStudentByID(-30);
            var newStudent = new clsStudent { Name = "Mazen Abdullah", Age = 20, Grade = 85 };
            await AddStudent(newStudent);
            await GetAllStudetns();
            await DeleteStudent(10);
            await GetAllStudetns();
            await UpdateStudent(1, new clsStudent { Name = "Abbas Kamel", Age = 99, Grade = 99 });
            await GetAllStudetns();


            Console.ReadKey();
        }

        static async Task GetAllStudetns()
        {
            try
            {
                Console.WriteLine("\n_____________");
                Console.WriteLine("\nFetch All Students...\n");
                var students = await httpClient.GetFromJsonAsync<List<clsStudent>>("All");
                if(students is not null)
                {
                    foreach(var student in students)
                    {
                        Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Age: {student.Grade}");
                    }
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task GetPassedStudents()
        {
            try
            {
                Console.WriteLine("\n_____________");
                Console.WriteLine("\nPassed All Students...\n");

                var PassedStudents = await httpClient.GetFromJsonAsync<List<clsStudent>>("Passed");
                if(PassedStudents is not null)
                {
                    foreach(var student in PassedStudents)
                    {
                        Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Age: {student.Grade}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An Error occurred {ex.Message}");
            }
        }

        static async Task GetAverageGrade()
        {
            try
            {
                Console.WriteLine("\n_____________");
                Console.WriteLine("\nAverage All Students...\n");

                var response = await httpClient.GetAsync("AverageGrade");
                if(response.IsSuccessStatusCode)
                {
                    var AverageGrades = await response.Content.ReadFromJsonAsync<double>();
                    Console.WriteLine($"Average Grade: {AverageGrades}");
                }

                if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine("This Average is not found!!!");
                }

            }   
            catch(Exception ex)
            {
                Console.WriteLine($"An Error occurred {ex.Message}");
            }
        }


        static async Task GetStudentByID(int id)
        {
            try
            {
                Console.WriteLine("\n_____________");
                Console.WriteLine("\nStudent Info\n");
                var response = await httpClient.GetAsync($"{id}");

                if (response.IsSuccessStatusCode)
                {
                    var student = await response.Content.ReadFromJsonAsync<clsStudent>();
                    Console.WriteLine($"ID: {student.Id}, Name: {student.Name}, Age: {student.Age}, Age: {student.Grade}");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    //Console.WriteLine($"This {id} is not found!!!");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    //Console.WriteLine($"This {id} is Bad Request");
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An Error Accour {ex.Message}");
            }
        }

        static async Task AddStudent(clsStudent newstudnet)
        {
            try
            {
                Console.WriteLine("\n_____________");
                Console.WriteLine("\nStudent Info\n");

                var respnose = await httpClient.PostAsJsonAsync("MyAddded", newstudnet);

                if (respnose.IsSuccessStatusCode)
                {
                    var addedstudent = await respnose.Content.ReadFromJsonAsync<clsStudent>();
                    Console.WriteLine($"Added Student - ID {addedstudent.Id} ,Name {addedstudent.Name} , Age {addedstudent.Age} , Grade {addedstudent.Grade}");
                }
                else if (respnose.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    Console.WriteLine(await respnose.Content.ReadAsStringAsync());
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An Error Occour {ex.Message}");
            }
        }

        static async Task DeleteStudent(int studentID)
        {
            try
            {
                Console.WriteLine("\n_____________");
                Console.WriteLine("\nDelete Student\n");
                var response = await httpClient.DeleteAsync($"{studentID}");
                if (response.IsSuccessStatusCode)
                {
                    var student = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(student);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                else if (response.StatusCode != System.Net.HttpStatusCode.BadRequest)
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"An Error Occour {ex.Message}"); 
            }
        
        }

        static async Task UpdateStudent(int ID,clsStudent student)
        {
            try
            {
                Console.WriteLine("\n_____________");
                Console.WriteLine("\nUpdate Student\n");

                var response = await httpClient.PutAsJsonAsync($"{ID}", student);

                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
            catch(Exception ex)
            {
                Console.WriteLine($"An Error Occour {ex.Message}");
            }
        }

        public class clsStudent
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }
            public int Grade { get; set; }
        }

    }


}
