using System;
using System.Collections.Generic;
using DevTrack.DataAccess.DAL;
using DevTrack.DataAccess.Models;

namespace DevTrack.BusinessLogic.Managers
{
    public class PersonManager
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
}
