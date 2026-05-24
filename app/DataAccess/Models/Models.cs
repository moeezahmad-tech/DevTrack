using System;

namespace DevTrack.DataAccess.Models
{
    public class PersonModel
    {
        public int    ID;
        public string Name;
        public string Email;
        public decimal Salary;
        public string Password;
        public DateTime JoiningDate;
    }

    public class EmployeeModel
    {
        public int    ID;
        public string Name;       // joined from Person
        public string Email;      // joined from Person
        public string Role;
        public string DeptID;
        public string DeptName;   // joined from Department
    }

    public class HeadOfDeptModel
    {
        public int    ID;
        public string Name;       // joined from Person
        public string Email;      // joined from Person
        public string OfficeNo;
        public string DeptID;
        public string DeptName;   // joined from Department
    }

    public class DepartmentModel
    {
        public string DeptID;
        public string DeptName;
    }

    public class ProjectModel
    {
        public string   ProjectID;
        public string   ProjectName;
        public string   DeptID;
        public string   DeptName;    // joined from Department
        public DateTime StartDate;
        public string   Status;
    }

    public class TaskItemModel
    {
        public string   TaskID;
        public string   Title;
        public string   TaskDescription;
        public string   Priority;
        public DateTime Deadline;
        public string   ProjectID;
        public string   ProjectName;  // joined
        public int      EmployeeID;
        public string   EmployeeName; // joined
    }

    public class ReportModel
    {
        public string   ReportID;
        public string   ReportTitle;
        public string   ReportDescription;
        public DateTime SubmissionDate;
        public string   StatusUpdate;
    }
}
