using System;
using System.IO;
using System.Linq;

struct Test_AddData_Student
{
    public string Faculty;
    public string LastName;
    public double Scholarship;
    public double AverageGrade;
}

struct Teacher
{
    public string Faculty;
    public string LastName;
    public string Position;
    public double Salary;
}

class Program
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Enter file name:");
            string fileName = Console.ReadLine();

            Test_AddData_Student[] students = ReadStudentsFromFile(fileName);
            Teacher[] teachers = ReadTeachersFromFile(fileName);

            bool exitProgram = false;

            while (!exitProgram)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Add data");
                Console.WriteLine("2. Update data");
                Console.WriteLine("3. Delete data");
                Console.WriteLine("4. List students with lowest grades and scholarships by faculty");
                Console.WriteLine("5. Count associate professors by faculty");
                Console.WriteLine("6. Calculate total scholarship and salary fund by faculty");
                Console.WriteLine("7. Exit");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddData(fileName, ref students, ref teachers);
                        break;
                    case 2:
                        UpdateData(fileName, ref students, ref teachers);
                        break;
                    case 3:
                        DeleteData(fileName);
                        break;
                    case 4:
                        ListStudentsWithLowestGradesByFaculty(students);
                        break;
                    case 5:
                        CountAssociateProfessorsByFaculty(teachers);
                        break;
                    case 6:
                        CalculateTotalFundsByFaculty(students, teachers);
                        break;
                    case 7:
                        exitProgram = true; 
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose again.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    static Test_AddData_Student[] ReadStudentsFromFile(string fileName)
    {
        if (!File.Exists(fileName))
        {
            return new Test_AddData_Student[0];
        }

        using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
        {
            int numStudents = reader.ReadInt32();
            Test_AddData_Student[] students = new Test_AddData_Student[numStudents];
            for (int i = 0; i < numStudents; i++)
            {
                students[i].Faculty = reader.ReadString();
                students[i].LastName = reader.ReadString();
                students[i].Scholarship = reader.ReadDouble();
                students[i].AverageGrade = reader.ReadDouble();
            }
            return students;
        }
    }

    static Teacher[] ReadTeachersFromFile(string fileName)
    {
        if (!File.Exists(fileName))
        {
            return new Teacher[0];
        }

        using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
        {
            reader.BaseStream.Seek(sizeof(int), SeekOrigin.Begin); 
            int numTeachers = reader.ReadInt32();
            Teacher[] teachers = new Teacher[numTeachers];
            for (int i = 0; i < numTeachers; i++)
            {
                teachers[i].Faculty = reader.ReadString();
                teachers[i].LastName = reader.ReadString();
                teachers[i].Position = reader.ReadString();
                teachers[i].Salary = reader.ReadDouble();
            }
            return teachers;
        }
    }

    static void AddData(string fileName, ref Test_AddData_Student[] students, ref Teacher[] teachers)
    {
        Console.WriteLine("Enter the type of data to add (student/teacher):");
        string dataType = Console.ReadLine();

        if (dataType.ToLower() == "student")
        {
            Console.WriteLine("Enter student details:");
            Test_AddData_Student student = GetStudentDetails();
            Array.Resize(ref students, students.Length + 1);
            students[students.Length - 1] = student;
        }
        else if (dataType.ToLower() == "teacher")
        {
            Console.WriteLine("Enter teacher details:");
            Teacher teacher = GetTeacherDetails();
            Array.Resize(ref teachers, teachers.Length + 1);
            teachers[teachers.Length - 1] = teacher;
        }
        else
        {
            Console.WriteLine("Invalid data type.");
            return;
        }

        WriteDataToFile(fileName, students, teachers);
        Console.WriteLine("Data added successfully.");
    }

    static Test_AddData_Student GetStudentDetails()
    {
        Console.Write("Faculty: ");
        string faculty = Console.ReadLine();
        Console.Write("Last Name: ");
        string lastName = Console.ReadLine();
        Console.Write("Scholarship: ");
        double scholarship = double.Parse(Console.ReadLine());
        Console.Write("Average Grade: ");
        double averageGrade = double.Parse(Console.ReadLine());

        return new Test_AddData_Student { Faculty = faculty, LastName = lastName, Scholarship = scholarship, AverageGrade = averageGrade };
    }

    static Teacher GetTeacherDetails()
    {
        Console.Write("Faculty: ");
        string faculty = Console.ReadLine();
        Console.Write("Last Name: ");
        string lastName = Console.ReadLine();
        Console.Write("Position: ");
        string position = Console.ReadLine();
        Console.Write("Salary: ");
        double salary = double.Parse(Console.ReadLine());

        return new Teacher { Faculty = faculty, LastName = lastName, Position = position, Salary = salary };
    }

    static void WriteDataToFile(string fileName, Test_AddData_Student[] students, Teacher[] teachers)
    {
        using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
        {
            writer.Write(students.Length);
            foreach (var student in students)
            {
                writer.Write(student.Faculty);
                writer.Write(student.LastName);
                writer.Write(student.Scholarship);
                writer.Write(student.AverageGrade);
            }

            writer.Write(teachers.Length);
            foreach (var teacher in teachers)
            {
                writer.Write(teacher.Faculty);
                writer.Write(teacher.LastName);
                writer.Write(teacher.Position);
                writer.Write(teacher.Salary);
            }
        }
    }

    static void UpdateData(string fileName, ref Test_AddData_Student[] students, ref Teacher[] teachers)
    {
        Console.WriteLine("Enter the type of data to update (student/teacher):");
        string dataType = Console.ReadLine();

        if (dataType.ToLower() == "student")
        {
            Console.WriteLine("Enter the index of the student to update:");
            int index = int.Parse(Console.ReadLine());
            if (index >= 0 && index < students.Length)
            {
                students[index] = GetStudentDetails();
            }
            else
            {
                Console.WriteLine("Invalid student index.");
                return;
            }
        }
        else if (dataType.ToLower() == "teacher")
        {
            Console.WriteLine("Enter the index of the teacher to update:");
            int index = int.Parse(Console.ReadLine());
            if (index >= 0 && index < teachers.Length)
            {
                teachers[index] = GetTeacherDetails();
            }
            else
            {
                Console.WriteLine("Invalid teacher index.");
                return;
            }
        }
        else
        {
            Console.WriteLine("Invalid data type.");
            return;
        }

        WriteDataToFile(fileName, students, teachers);
        Console.WriteLine("Data updated successfully.");
    }

    static void DeleteData(string fileName)
    {
        Console.WriteLine("Enter the type of data to delete (student/teacher):");
        string dataType = Console.ReadLine();

        if (dataType.ToLower() == "student")
        {
            Console.WriteLine("Enter the index of the student to delete:");
            int index = int.Parse(Console.ReadLine());
            RemoveRecord(fileName, index, sizeof(int) + index * (sizeof(double) * 2 + sizeof(int) + 2 * sizeof(char) * 100));
        }
        else if (dataType.ToLower() == "teacher")
        {
            Console.WriteLine("Enter the index of the teacher to delete:");
            int index = int.Parse(Console.ReadLine());
            RemoveRecord(fileName, index, sizeof(int) + students.Length * (sizeof(double) * 2 + sizeof(int) + 2 * sizeof(char) * 100) + index * (sizeof(double) + 2 * sizeof(char) * 100));
        }
        else
        {
            Console.WriteLine("Invalid data type.");
            return;
        }

        Console.WriteLine("Data deleted successfully.");
    }

    static void RemoveRecord(string fileName, int index, long offset)
    {
        using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
        {
            BinaryWriter writer = new BinaryWriter(stream);
            BinaryReader reader = new BinaryReader(stream);

            stream.Seek(offset, SeekOrigin.Begin);

            for (long i = offset; i < stream.Length - (sizeof(double) * 2 + sizeof(int) + 2 * sizeof(char) * 100); i++)
            {
                stream.Position = i + (sizeof(double) * 2 + sizeof(int) + 2 * sizeof(char) * 100);
                byte dataByte = reader.ReadByte();
                stream.Position = i;
                writer.Write(dataByte);
            }

            stream.SetLength(stream.Length - (sizeof(double) * 2 + sizeof(int) + 2 * sizeof(char) * 100));
        }
    }


    static void ListStudentsWithLowestGradesByFaculty(Test_AddData_Student[] students)
    {
        var groupedStudents = students.GroupBy(s => s.Faculty);
        foreach (var group in groupedStudents)
        {
            var lowestGrade = group.Min(s => s.AverageGrade);
            var studentsWithLowestGrade = group.Where(s => s.AverageGrade == lowestGrade);
            Console.WriteLine($"Faculty: {group.Key}");
            foreach (var student in studentsWithLowestGrade)
            {
                Console.WriteLine($"Student: {student.LastName}, Average Grade: {student.AverageGrade}, Scholarship: {student.Scholarship}");
            }
        }
    }

    static void CountAssociateProfessorsByFaculty(Teacher[] teachers)
    {
        var groupedTeachers = teachers.GroupBy(t => t.Faculty);
        foreach (var group in groupedTeachers)
        {
            var count = group.Count(t => t.Position == "Associate Professor");
            Console.WriteLine($"Faculty: {group.Key}, Associate Professors: {count}");
        }
    }

    static void CalculateTotalFundsByFaculty(Test_AddData_Student[] students, Teacher[] teachers)
    {
        var groupedStudents = students.GroupBy(s => s.Faculty);
        var groupedTeachers = teachers.GroupBy(t => t.Faculty);

        foreach (var group in groupedStudents)
        {
            double scholarshipFund = group.Sum(s => s.Scholarship);
            double salaryFund = groupedTeachers.FirstOrDefault(t => t.Key == group.Key)?.Sum(t => t.Salary) ?? 0;
            Console.WriteLine($"Faculty: {group.Key}, Scholarship Fund: {scholarshipFund}, Salary Fund: {salaryFund}");
        }
        Console.WriteLine("Do you want to continue? (yes/no)");
        string continueOption = Console.ReadLine();
        if (continueOption.ToLower() != "yes")
        {
            Console.WriteLine("Exiting the program...");
            return;
        }

    }
}