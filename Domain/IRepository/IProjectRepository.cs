using Domain.Model;

namespace Domain.IRepository;

public interface IProjectRepository
{
    Task<IEnumerable<long>> GetProjectsIdAsync();
    Task<long> Add(long Id);
}