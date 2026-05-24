using System;
using System.Collections.Generic;
using DevTrack.DataAccess.DAL;
using DevTrack.DataAccess.Models;

namespace DevTrack.BusinessLogic.Managers
{
    public class TaskManager
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
}
