using System;
using System.Collections.Generic;
using CompanyManagement.DAL;
using CompanyManagement.Models;

namespace CompanyManagement.BLL
{
    // ── Person BLL ────────────────────────────────────────────────────────────
    public class PersonBLL
    {
        PersonDAL dal = new PersonDAL();

        public PersonModel Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new Exception("Email and Password are required.");
            return dal.Login(email.Trim(), password);
        }

        public void RegisterPerson(PersonModel p)
        {
            if (p.ID <= 0)
                throw new Exception("ID must be a positive integer.");
            if (string.IsNullOrWhiteSpace(p.Name))
                throw new Exception("Name is required.");
            if (string.IsNullOrWhiteSpace(p.Email) || !p.Email.Contains("@"))
                throw new Exception("A valid Email is required.");
            if (string.IsNullOrWhiteSpace(p.Password) || p.Password.Length < 6)
                throw new Exception("Password must be at least 6 characters.");
            if (p.Salary < 0)
                throw new Exception("Salary cannot be negative.");
            if (dal.IDExists(p.ID))
                throw new Exception("A person with this ID already exists.");
            if (dal.EmailExists(p.Email))
                throw new Exception("This email is already registered.");
            dal.InsertPerson(p);
        }

        public List<PersonModel> GetAll()
        {
            return dal.GetAllPersons();
        }
    }

    // ── Department BLL ────────────────────────────────────────────────────────
    public class DepartmentBLL
    {
        DepartmentDAL dal = new DepartmentDAL();

        public List<DepartmentModel> GetAll()
        {
            return dal.GetAll();
        }

        public void Add(DepartmentModel d)
        {
            if (string.IsNullOrWhiteSpace(d.DeptID))
                throw new Exception("Department ID is required.");
            if (string.IsNullOrWhiteSpace(d.DeptName))
                throw new Exception("Department Name is required.");
            if (dal.IDExists(d.DeptID))
                throw new Exception("Department ID already exists.");
            dal.Insert(d);
        }

        public void Update(DepartmentModel d)
        {
            if (string.IsNullOrWhiteSpace(d.DeptName))
                throw new Exception("Department Name is required.");
            dal.Update(d);
        }

        public void Delete(string deptID)
        {
            dal.Delete(deptID);
        }
    }

    // ── Employee BLL ──────────────────────────────────────────────────────────
    public class EmployeeBLL
    {
        EmployeeDAL dal    = new EmployeeDAL();
        PersonDAL   pDal   = new PersonDAL();

        public List<EmployeeModel> GetAll()
        {
            return dal.GetAll();
        }

        public EmployeeModel GetByID(int id)
        {
            return dal.GetByID(id);
        }

        // HOD registers a brand-new employee: creates Person row then Employee row
        public void RegisterNewEmployee(PersonModel person, EmployeeModel emp)
        {
            if (string.IsNullOrWhiteSpace(emp.Role))
                throw new Exception("Role is required.");
            // PersonBLL validates and inserts the Person row
            PersonBLL pBll = new PersonBLL();
            pBll.RegisterPerson(person);
            emp.ID = person.ID;
            dal.Insert(emp);
        }

        public void Update(EmployeeModel emp)
        {
            if (string.IsNullOrWhiteSpace(emp.Role))
                throw new Exception("Role is required.");
            dal.Update(emp);
        }

        public void Delete(int id)
        {
            // Deleting from Employee only; Person row stays unless cascaded.
            // Because FK is ON DELETE CASCADE from Person, delete Person to remove both.
            dal.Delete(id);
        }

        public bool IsEmployee(int id)
        {
            return dal.IsEmployee(id);
        }
    }

    // ── HeadOfDept BLL ────────────────────────────────────────────────────────
    public class HeadOfDeptBLL
    {
        HeadOfDeptDAL dal = new HeadOfDeptDAL();

        public bool IsHOD(int id)
        {
            return dal.IsHOD(id);
        }

        public HeadOfDeptModel GetByID(int id)
        {
            return dal.GetByID(id);
        }

        public List<HeadOfDeptModel> GetAll()
        {
            return dal.GetAll();
        }

        // HOD can promote a person to HOD
        public void RegisterNewHOD(PersonModel person, HeadOfDeptModel hod)
        {
            if (string.IsNullOrWhiteSpace(hod.OfficeNo))
                throw new Exception("Office Number is required.");
            PersonBLL pBll = new PersonBLL();
            pBll.RegisterPerson(person);
            hod.ID = person.ID;
            dal.Insert(hod);
        }
    }

    // ── Project BLL ───────────────────────────────────────────────────────────
    public class ProjectBLL
    {
        ProjectDAL dal = new ProjectDAL();

        public List<ProjectModel> GetAll()
        {
            return dal.GetAll();
        }

        public void Add(ProjectModel pm)
        {
            if (string.IsNullOrWhiteSpace(pm.ProjectID))
                throw new Exception("Project ID is required.");
            if (string.IsNullOrWhiteSpace(pm.ProjectName))
                throw new Exception("Project Name is required.");
            if (dal.IDExists(pm.ProjectID))
                throw new Exception("Project ID already exists.");
            dal.Insert(pm);
        }

        public void Update(ProjectModel pm)
        {
            if (string.IsNullOrWhiteSpace(pm.ProjectName))
                throw new Exception("Project Name is required.");
            if (string.IsNullOrWhiteSpace(pm.Status))
                throw new Exception("Status is required.");
            dal.Update(pm);
        }

        public void Delete(string projectID)
        {
            dal.Delete(projectID);
        }
    }

    // ── Task BLL ──────────────────────────────────────────────────────────────
    public class TaskBLL
    {
        TaskDAL dal = new TaskDAL();

        public List<TaskItemModel> GetAll()
        {
            return dal.GetAll();
        }

        public List<TaskItemModel> GetByEmployee(int employeeID)
        {
            return dal.GetByEmployee(employeeID);
        }

        public void Add(TaskItemModel tm)
        {
            if (string.IsNullOrWhiteSpace(tm.TaskID))
                throw new Exception("Task ID is required.");
            if (string.IsNullOrWhiteSpace(tm.Title))
                throw new Exception("Title is required.");
            if (string.IsNullOrWhiteSpace(tm.Priority))
                throw new Exception("Priority is required.");
            if (dal.IDExists(tm.TaskID))
                throw new Exception("Task ID already exists.");
            dal.Insert(tm);
        }

        public void Update(TaskItemModel tm)
        {
            if (string.IsNullOrWhiteSpace(tm.Title))
                throw new Exception("Title is required.");
            dal.Update(tm);
        }

        public void Delete(string taskID)
        {
            dal.Delete(taskID);
        }
    }

    // ── Report BLL ────────────────────────────────────────────────────────────
    public class ReportBLL
    {
        ReportDAL dal = new ReportDAL();

        public List<ReportModel> GetAll()
        {
            return dal.GetAll();
        }

        public void Submit(ReportModel r)
        {
            if (string.IsNullOrWhiteSpace(r.ReportID))
                throw new Exception("Report ID is required.");
            if (string.IsNullOrWhiteSpace(r.ReportTitle))
                throw new Exception("Report Title is required.");
            if (dal.IDExists(r.ReportID))
                throw new Exception("Report ID already exists.");
            r.StatusUpdate = "Pending";
            dal.Insert(r);
        }

        public void Approve(string reportID)
        {
            dal.UpdateStatus(reportID, "Approved");
        }

        public void Reject(string reportID)
        {
            dal.UpdateStatus(reportID, "Rejected");
        }

        public void Delete(string reportID)
        {
            dal.Delete(reportID);
        }
    }
}
