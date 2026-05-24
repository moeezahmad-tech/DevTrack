using System;
using System.Collections.Generic;
using DevTrack.DataAccess.DAL;
using DevTrack.DataAccess.Models;

namespace DevTrack.BusinessLogic.Managers
{
    public class EmployeeManager
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
            // PersonManager validates and inserts the Person row
            PersonManager pManager = new PersonManager();
            pManager.RegisterPerson(person);
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
}
