using System.Collections.Generic;

namespace QuanLyTTNgoaiNgu.Models
{
    public class ReportViewModel
    {
        // Tổng quan
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalClasses { get; set; }

        // Thống kê chi tiết
        public List<ClassStudentCount> StudentsPerClass { get; set; }
        public List<CourseClassCount> ClassesPerCourse { get; set; }
        public List<CourseStudentCount> StudentsPerCourse { get; set; }
        public int UnenrolledStudents { get; set; }

        // Doanh thu
        public List<RevenuePerGroup> RevenuePerClass { get; set; }
        public List<RevenuePerGroup> RevenuePerCourse { get; set; }
    }

    public class ClassStudentCount
    {
        public string ClassName { get; set; }
        public int Count { get; set; }
    }

    public class CourseClassCount
    {
        public string CourseName { get; set; }
        public int Count { get; set; }
    }

    public class CourseStudentCount
    {
        public string CourseName { get; set; }
        public int Count { get; set; }
    }

    public class RevenuePerGroup
    {
        public string Key { get; set; }
        public double Amount { get; set; }
    }
}
