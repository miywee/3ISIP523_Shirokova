using System;
using System.Collections.Generic;

public abstract class Person
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public virtual string GetInfo()
    {
        return $"Имя: {Name}, Возраст: {Age}";
    }

    public abstract string GetRole();
}

public class Student : Person
{
    private static int nextId = 1;
    public int StudentId { get; }
    private List<Course> courses;

    public Student(string name, int age) 
        : base(name, age)
    {
        StudentId = nextId++;
        courses = new List<Course>();
    }

    public void EnrollInCourse(Course course)
    {
        if (course != null && !courses.Contains(course))
        {
            courses.Add(course);
            course.AddStudent(this);
        }
    }

    public Course[] GetCourses()
    {
        return courses.ToArray();
    }

    public override string GetInfo()
    {
        return base.GetInfo() + $", ID студента: {StudentId}";
    }

    public override string GetRole()
    {
        return "Студент";
    }
}

public class Teacher : Person
{
    private static int nextId = 1;
    public int TeacherId { get; }
    private List<Course> coursesTeaching;

    public Teacher(string name, int age) 
        : base(name, age)
    {
        TeacherId = nextId++;
        coursesTeaching = new List<Course>();
    }

    public void AssignToCourse(Course course)
    {
        if (course != null && !coursesTeaching.Contains(course))
        {
            coursesTeaching.Add(course);
            course.AssignTeacher(this);
        }
    }

    public Course[] GetTeachingCourses()
    {
        return coursesTeaching.ToArray();
    }

    public override string GetInfo()
    {
        return base.GetInfo() + $", ID преподавателя: {TeacherId}";
    }

    public override string GetRole()
    {
        return "Преподаватель";
    }
}

public class Course
{
    private static int nextId = 1;
    public int CourseId { get; }
    public string CourseName { get; set; }
    public string Description { get; set; }
    public Teacher Teacher { get; private set; }
    private List<Student> enrolledStudents;

    public Course(string courseName, string description)
    {
        CourseId = nextId++;
        CourseName = courseName;
        Description = description;
        enrolledStudents = new List<Student>();
    }

    public void AssignTeacher(Teacher teacher)
    {
        Teacher = teacher;
    }

    public void AddStudent(Student student)
    {
        if (student != null && !enrolledStudents.Contains(student))
        {
            enrolledStudents.Add(student);
        }
    }

    public Student[] GetEnrolledStudents()
    {
        return enrolledStudents.ToArray();
    }

    public string GetCourseInfo()
    {
        string teacherInfo = Teacher != null ? Teacher.Name : "Не назначен";
        return $"Курс: {CourseName} (ID: {CourseId})\nОписание: {Description}\nПреподаватель: {teacherInfo}\nСтудентов записано: {enrolledStudents.Count}";
    }
}

public class UniversitySystem
{
    private List<Student> students;
    private List<Teacher> teachers;
    private List<Course> courses;

    public UniversitySystem()
    {
        students = new List<Student>();
        teachers = new List<Teacher>();
        courses = new List<Course>();
    }
    
    public void AddStudent(string name, int age)
    {
        Student student = new Student(name, age);
        students.Add(student);
        Console.WriteLine($"Студент добавлен: {student.GetInfo()}");
    }

    public void DisplayAllStudents()
    {
        Console.WriteLine("\nВСЕ СТУДЕНТЫ");
        if (students.Count == 0)
        {
            Console.WriteLine("Студентов нет в системе.");
            return;
        }

        for (int i = 0; i < students.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {students[i].GetInfo()}");
        }
    }

    public void DisplayStudentInfo(int studentId)
    {
        Student student = FindStudentById(studentId);
        if (student != null)
        {
            Console.WriteLine($"\nИНФОРМАЦИЯ О СТУДЕНТЕ");
            Console.WriteLine(student.GetInfo());
            
            Course[] studentCourses = student.GetCourses();
            Console.WriteLine("Записан на курсы:");
            if (studentCourses.Length == 0)
            {
                Console.WriteLine("Не записан на курсы");
            }
            else
            {
                for (int i = 0; i < studentCourses.Length; i++)
                {
                    Console.WriteLine($"  {i + 1}. {studentCourses[i].CourseName}");
                }
            }
        }
        else
        {
            Console.WriteLine("Студент не найден.");
        }
    }
    
    public void AddTeacher(string name, int age)
    {
        Teacher teacher = new Teacher(name, age);
        teachers.Add(teacher);
        Console.WriteLine($"Преподаватель добавлен: {teacher.GetInfo()}");
    }

    public void DisplayAllTeachers()
    {
        Console.WriteLine("\nВСЕ ПРЕПОДАВАТЕЛИ");
        if (teachers.Count == 0)
        {
            Console.WriteLine("Преподавателей нет в системе.");
            return;
        }

        for (int i = 0; i < teachers.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {teachers[i].GetInfo()}");
        }
    }
    
    public void AddCourse(string courseName, string description)
    {
        Course course = new Course(courseName, description);
        courses.Add(course);
        Console.WriteLine($"Курс добавлен: {course.CourseName} (ID: {course.CourseId})");
    }

    public void DisplayAllCourses()
    {
        Console.WriteLine("\nВСЕ КУРСЫ");
        if (courses.Count == 0)
        {
            Console.WriteLine("Курсов нет в системе.");
            return;
        }

        for (int i = 0; i < courses.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {courses[i].GetCourseInfo()}");
            Console.WriteLine(" ");
        }
    }

    public void DisplayCourseDetails(int courseId)
    {
        Course course = FindCourseById(courseId);
        if (course != null)
        {
            Console.WriteLine($"\nДЕТАЛИ КУРСА");
            Console.WriteLine(course.GetCourseInfo());
            
            Student[] enrolledStudents = course.GetEnrolledStudents();
            Console.WriteLine("Список студентов:");
            if (enrolledStudents.Length == 0)
            {
                Console.WriteLine("  Студентов нет");
            }
            else
            {
                for (int i = 0; i < enrolledStudents.Length; i++)
                {
                    Console.WriteLine($"  {i + 1}. {enrolledStudents[i].Name}");
                }
            }
        }
        else
        {
            Console.WriteLine("Курс не найден.");
        }
    }
    
    public void EnrollStudentInCourse(int studentId, int courseId)
    {
        Student student = FindStudentById(studentId);
        Course course = FindCourseById(courseId);

        if (student != null && course != null)
        {
            student.EnrollInCourse(course);
            Console.WriteLine($"Студент {student.Name} записан на курс {course.CourseName}");
        }
        else
        {
            Console.WriteLine("Студент или курс не найден.");
        }
    }

    public void AssignTeacherToCourse(int teacherId, int courseId)
    {
        Teacher teacher = FindTeacherById(teacherId);
        Course course = FindCourseById(courseId);

        if (teacher != null && course != null)
        {
            teacher.AssignToCourse(course);
            Console.WriteLine($"Преподаватель {teacher.Name} назначен на курс {course.CourseName}");
        }
        else
        {
            Console.WriteLine("Преподаватель или курс не найден.");
        }
    }
    
    private Student FindStudentById(int studentId)
    {
        for (int i = 0; i < students.Count; i++)
        {
            if (students[i].StudentId == studentId)
                return students[i];
        }
        return null;
    }

    private Teacher FindTeacherById(int teacherId)
    {
        for (int i = 0; i < teachers.Count; i++)
        {
            if (teachers[i].TeacherId == teacherId)
                return teachers[i];
        }
        return null;
    }

    private Course FindCourseById(int courseId)
    {
        for (int i = 0; i < courses.Count; i++)
        {
            if (courses[i].CourseId == courseId)
                return courses[i];
        }
        return null;
    }
}

class Program
{
    static void Main(string[] args)
    {
        UniversitySystem university = new UniversitySystem();
        bool exit = false;

        Console.WriteLine("СИСТЕМА УПРАВЛЕНИЯ УНИВЕРСИТЕТОМ");

        while (!exit)
        {
            Console.WriteLine("\nМЕНЮ");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Показать всех студентов");
            Console.WriteLine("3. Информация о студенте");
            Console.WriteLine("4. Добавить преподавателя");
            Console.WriteLine("5. Показать всех преподавателей");
            Console.WriteLine("6. Добавить курс");
            Console.WriteLine("7. Показать все курсы");
            Console.WriteLine("8. Детали курса");
            Console.WriteLine("9. Записать студента на курс");
            Console.WriteLine("10. Назначить преподавателя на курс");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите опцию: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddStudentMenu(university);
                    break;
                case "2":
                    university.DisplayAllStudents();
                    break;
                case "3":
                    DisplayStudentInfoMenu(university);
                    break;
                case "4":
                    AddTeacherMenu(university);
                    break;
                case "5":
                    university.DisplayAllTeachers();
                    break;
                case "6":
                    AddCourseMenu(university);
                    break;
                case "7":
                    university.DisplayAllCourses();
                    break;
                case "8":
                    DisplayCourseDetailsMenu(university);
                    break;
                case "9":
                    EnrollStudentMenu(university);
                    break;
                case "10":
                    AssignTeacherMenu(university);
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор");
                    break;
            }
        }
    }

    static void AddStudentMenu(UniversitySystem university)
    {
        Console.Write("Введите имя студента: ");
        string name = Console.ReadLine();
        Console.Write("Введите возраст студента: ");
        int age = int.Parse(Console.ReadLine());

        university.AddStudent(name, age);
    }

    static void AddTeacherMenu(UniversitySystem university)
    {
        Console.Write("Введите имя преподавателя: ");
        string name = Console.ReadLine();
        Console.Write("Введите возраст преподавателя: ");
        int age = int.Parse(Console.ReadLine());

        university.AddTeacher(name, age);
    }

    static void AddCourseMenu(UniversitySystem university)
    {
        Console.Write("Введите название курса: ");
        string name = Console.ReadLine();
        Console.Write("Введите описание курса: ");
        string description = Console.ReadLine();

        university.AddCourse(name, description);
    }

    static void DisplayStudentInfoMenu(UniversitySystem university)
    {
        Console.Write("Введите ID студента: ");
        int id = int.Parse(Console.ReadLine());
        university.DisplayStudentInfo(id);
    }

    static void DisplayCourseDetailsMenu(UniversitySystem university)
    {
        Console.Write("Введите ID курса: ");
        int id = int.Parse(Console.ReadLine());
        university.DisplayCourseDetails(id);
    }

    static void EnrollStudentMenu(UniversitySystem university)
    {
        Console.Write("Введите ID студента: ");
        int studentId = int.Parse(Console.ReadLine());
        Console.Write("Введите ID курса: ");
        int courseId = int.Parse(Console.ReadLine());
        university.EnrollStudentInCourse(studentId, courseId);
    }

    static void AssignTeacherMenu(UniversitySystem university)
    {
        Console.Write("Введите ID преподавателя: ");
        int teacherId = int.Parse(Console.ReadLine());
        Console.Write("Введите ID курса: ");
        int courseId = int.Parse(Console.ReadLine());
        university.AssignTeacherToCourse(teacherId, courseId);
    }
}