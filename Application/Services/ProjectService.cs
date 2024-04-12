using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.IRepository;

namespace Application.Services
{
    public class ProjectService
    {
        private readonly IProjectRepository _projectRepository;
        
        public ProjectService(IProjectRepository projectRepository) {
            _projectRepository = projectRepository;
        }

        public async Task<long> Add(long id)
        {

            return await _projectRepository.Add(id);
        }
    }
}