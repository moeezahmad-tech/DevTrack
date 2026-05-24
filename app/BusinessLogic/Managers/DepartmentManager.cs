using System;
using System.Collections.Generic;
using DevTrack.DataAccess.DAL;
using DevTrack.DataAccess.Models;

namespace DevTrack.BusinessLogic.Managers
{
    public class DepartmentManager
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
}
