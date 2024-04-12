using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModel.Model;

namespace DataModel.Mapper
{
    public class ProjectMapper
    {

        public ProjectMapper()
        {
        }

        public long ToDomain(ProjectDataModel projectDM)
        {
            return projectDM.Id;
        }

        public IEnumerable<long> ToDomain(IEnumerable<ProjectDataModel> projectsDataModel)
        {
            List<long> projectsDomain = new List<long>();

            foreach(ProjectDataModel projectDomain in projectsDataModel)
            {
                long id = ToDomain(projectDomain);

                projectsDomain.Add(id);
            }

            return projectsDomain.AsEnumerable();
        }

        public ProjectDataModel ToDataModel(long projectId)
        {
            ProjectDataModel projectDataModel = new ProjectDataModel(projectId);

            return projectDataModel;
        }
    }
}