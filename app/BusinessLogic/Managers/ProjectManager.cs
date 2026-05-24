using System;
using System.Collections.Generic;
using DevTrack.DataAccess.DAL;
using DevTrack.DataAccess.Models;

namespace DevTrack.BusinessLogic.Managers
{
    public class ProjectManager
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
}
