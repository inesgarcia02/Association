using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataModel.Model
{
    public class ProjectDataModel
    {
        public long Id { get; set; }

        public ProjectDataModel() {}

        public ProjectDataModel(long id)
        {
            Id = id;
        }
    }
}