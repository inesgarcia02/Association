namespace DataModel.Repository;

using Microsoft.EntityFrameworkCore;

using DataModel.Model;
using DataModel.Mapper;

using Domain.Model;
using Domain.IRepository;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class ProjectRepository : GenericRepository<ProjectRepository>, IProjectRepository
{    
    ProjectMapper _projectMapper;
    public ProjectRepository(AbsanteeContext context, ProjectMapper mapper) : base(context!)
    {
        _projectMapper = mapper;
    }

    public async Task<IEnumerable<long>> GetProjectsIdAsync()
    {
        try {
            IEnumerable<ProjectDataModel> projectsDataModel = await _context.Set<ProjectDataModel>()
                    .ToListAsync();

            IEnumerable<long> projectId = _projectMapper.ToDomain(projectsDataModel);

            return projectId;
        }
        catch
        {
            throw;
        }
    }

    public async Task<long> Add(long Id)
    {
        try {
            ProjectDataModel projectsDataModel = _projectMapper.ToDataModel(Id);

            EntityEntry<ProjectDataModel> projectDataModelEntityEntry = _context.Set<ProjectDataModel>().Add(projectsDataModel);
            
            await _context.SaveChangesAsync();

            ProjectDataModel projectDataModelSaved = projectDataModelEntityEntry.Entity;

            long projectSaved = _projectMapper.ToDomain(projectDataModelSaved);

            return projectSaved;    
        }
        catch
        {
            throw;
        }
    }
}