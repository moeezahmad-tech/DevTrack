using System;
using System.Collections.Generic;
using DevTrack.DataAccess.DAL;
using DevTrack.DataAccess.Models;

namespace DevTrack.BusinessLogic.Managers
{
    public class HeadOfDeptManager
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
            PersonManager pManager = new PersonManager();
            pManager.RegisterPerson(person);
            hod.ID = person.ID;
            dal.Insert(hod);
        }
    }
}
