using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace UnitTestProject1
{
    public struct Teacher
    {
        public string Faculty;
        public string LastName;
        public string Position;
        public double Salary;
    }

    [TestClass]
    public class ProgramTests
    {
        [TestMethod]
        public void Test_AddDataStudent()
        {
            var fileName = "test.bin";
            var students = new Test_AddData_Student[0]; 
            var teachers = new Teacher[0]; 

            AddData(fileName, ref students, ref teachers);

            Assert.AreEqual(1, students.Length);
        }

        public struct Test_AddData_Student
        {
            public string Faculty;
            public string LastName;
            public double Scholarship;
            public double AverageGrade;
        }


        private static void AddData(string fileName, ref Test_AddData_Student[] students, ref Teacher[] teachers)
        {
            Test_AddData_Student student = new Test_AddData_Student
            {
                Faculty = "Engineering",
                LastName = "Smith",
                Scholarship = 1000,
                AverageGrade = 85
            };
            Array.Resize(ref students, students.Length + 1);
            students[students.Length - 1] = student;

            Teacher teacher = new Teacher
            {
                Faculty = "Engineering",
                LastName = "Johnson",
                Position = "Assistant Professor",
                Salary = 60000
            };
            Array.Resize(ref teachers, teachers.Length + 1);
            teachers[teachers.Length - 1] = teacher;
        }

    }
}
