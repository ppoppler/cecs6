﻿using BusinessLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment5
{
    public class Program
    {

        public static void Main(string[] args)
        {
            IBusinessLayer blayer = new BusinessLayer.BusinessLayer();
            IEnumerable<Student> students = blayer.GetAllStudents();
            IEnumerable<Teacher> teachers = blayer.GetAllTeachers();
            IEnumerable<Course> courses = blayer.GetTeacherCourses(5);
            IEnumerable<Course> coursesList = blayer.GetAllCourses();
            foreach (var s in blayer.GetAllStudents())
            {
                Console.WriteLine(s.StudentName);
                foreach (var c in s.Courses)
                {
                    Console.WriteLine(c.CourseName);
                }
            }
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine(
                    "\n\nMenu\n" +
                    "Teach Actions:\n" +
                    "[1] Create\n" +
                    "[2] Update\n" +
                    "[3] Delete\n" +
                    "[4] Display All" +
                    "\n\n" +
                    "Course Actions:\n" +
                    "[5] Create\n" +
                    "[6] Update using Course ID\n" +
                    "[7] Update using Course Name\n" +
                    "[8] Delete\n" +
                    "[9] Display All\n" +
                    "\n[0] Exit");
                Console.Write("Choice: ");
                try
                {
                    int choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            Teacher t = AddTeacher();
                            if (t != null)
                            {
                                blayer.AddTeacher(t);
                                Console.WriteLine("Teacher added!");
                            }
                            break;
                        case 2:
                            UpdateTeacher(blayer);
                            break;
                        case 3:
                            RemoveTeacher(blayer);
                            break;
                        case 4:
                            DisplayAll<Teacher>(blayer);
                            break;
                        case 5:
                            AddCourse(blayer);
                            break;
                        case 6:
                            ModifyCourseByID(blayer);
                            break;
                        case 7:
                            ModifyCourseByName(blayer);
                            break;
                        case 8:
                            RemoveCourse(blayer);
                            break;
                        case 9:
                            DisplayAll<Course>(blayer);
                            break;
                        case 0:
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
                catch (Exception)
                {
                    Console.WriteLine("Wrong input type.\nPress any key to continue...");
                    Console.ReadKey();
                }
            }    
        }

        public static void AddCourse(IBusinessLayer DataAccess)
        {
            Course t;
            try
            {
                Console.Write("\nEnter a course name: ");
                string name = Console.ReadLine();
                if (name.Length > 0)
                {
                    t = new Course
                    {
                        CourseName = name,
                    };
                    DataAccess.AddCourse(t);
                }
            } catch (Exception)
            {
                Console.WriteLine("invalid input");
            }
        }

        public static void DisplayAll<T>(IBusinessLayer DataAccess)
        {
            if (DataAccess != null)
            {
                if (typeof(T).Equals(typeof(Course)))
                {
                    foreach (var course in DataAccess.GetAllCourses())
                    {
                        Console.WriteLine(course.CourseId + " " + course.CourseName);
                    }
                    return;
                }
                if (typeof(T).Equals(typeof(Teacher)))
                {
                    foreach (var teacher in DataAccess.GetAllTeachers())
                    {
                        Console.WriteLine(teacher.TeacherId + " " + teacher.TeacherName);
                    }
                    return;
                }
            }
        }

        public static void RemoveCourse(IBusinessLayer DataAccess)
        {
            DisplayAll<Course>(DataAccess);
            Course selected;
            Console.Write("\nEnter course id to remove: ");
            try
            {
                int courseID = Convert.ToInt32(Console.ReadLine());
                selected = DataAccess.GetCourseByID(courseID);
                if (selected != null){
                    foreach (var student in selected.Students)
                    {
                        student.Courses.Remove(selected);
                    }
                    selected.Teacher.Courses.Remove(selected);
                    DataAccess.RemoveCourse(selected);
                    Console.WriteLine(selected.CourseName + " has been removed!");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("invalid input");
            }
        }

        public static Teacher AddTeacher()
        {
            try
            {
                Console.Write("\nEnter a teacher name: ");
                string name = Console.ReadLine();
                return new Teacher {
                    TeacherName = name,
                };
            }
            catch (Exception)
            {

            }
            return null;
        }

        public static Teacher UpdateTeacher(IBusinessLayer DataAccess)
        {
            IEnumerable<Teacher> teacherList = DataAccess.GetAllTeachers();
            Console.WriteLine("\nTeacher List...");
            foreach (Teacher teacher in teacherList)
            {
                Console.WriteLine(teacher.TeacherId + " " + teacher.TeacherName);
            }
            Console.Write("\nEnter a teacher id to update: ");
            Teacher selected;
            try
            {
                int teacherID = Convert.ToInt32(Console.ReadLine());
                selected = DataAccess.GetTeacherByID(teacherID);
                Console.WriteLine("\nEnter a new teacher name");
                string name = Console.ReadLine();
                selected.TeacherName = name;
                return selected;
            }
            catch (Exception)
            {

            }
            return null;
        }

        public static bool RemoveTeacher(IBusinessLayer DataAccess)
        {
            IEnumerable<Teacher> teacherList = DataAccess.GetAllTeachers();
            Console.WriteLine("\nCourse List...");
            foreach (Teacher teacher in teacherList)
            {
                Console.WriteLine(teacher.TeacherId + " " + teacher.TeacherName);
            }
            Teacher selected;
            Console.Write("\nEnter teacher id to remove: ");
            try
            {
                int teacherID = Convert.ToInt32(Console.ReadLine());
                selected = DataAccess.GetTeacherByID(teacherID);
                if (selected != null)
                {
                    DataAccess.RemoveTeacher(selected);
                    Console.WriteLine(selected.TeacherName + " has been removed!");
                }
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("invalid input");
            }
            return false;
        }

        public static void ModifyCourseByID(IBusinessLayer DataAccess)
        {
            Console.WriteLine();
            DisplayAll<Course>(DataAccess);
            Console.Write("\nModify Course with ID: ");
            Course selected;
            try
            {
                int courseID = Convert.ToInt32(Console.ReadLine());
                selected = DataAccess.GetCourseByID(courseID);
                if (selected != null)
                {
                    bool valid = false;
                    while (!valid)
                    {
                        Console.Write("\nNew name: ");
                        string name = Console.ReadLine();
                        if (name.Length > 0)
                        {
                            selected.CourseName = name;
                            Console.WriteLine(selected.CourseName + " has been modified!");
                            DataAccess.UpdateCourse(selected);
                            valid = true;
                        }
                    }

                }
            }
            catch (Exception)
            {
                Console.WriteLine("invalid input");
            }
        }

        public static void ModifyCourseByName(IBusinessLayer DataAccess)
        {
            Console.WriteLine();
            DisplayAll<Course>(DataAccess);
            Console.Write("\nModify Course with Name: ");
            Course result;
            Course selected = null;
            try
            {
                string courseName = Console.ReadLine();
                Console.WriteLine(DataAccess.GetCourseByName(courseName).CourseName);
                result = DataAccess.GetCourseByName(courseName);
                if (result != null)
                    selected = DataAccess.GetCourseByID(result.CourseId);
                if (selected != null)
                {
                    bool valid = false;
                    while (!valid)
                    {
                        Console.Write("\nNew name: ");
                        string name = Console.ReadLine();
                        if (name.Length > 0)
                        {
                            selected.CourseName = name;
                            Console.WriteLine(selected.CourseName + " has been modified!");
                            DataAccess.UpdateCourse(selected);
                            valid = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("invalid input");
            }
        }
    }

       
    
}
