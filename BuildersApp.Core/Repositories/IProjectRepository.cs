using BuildersApp.Core.Models;

namespace BuildersApp.Core.Repositories;

public interface IProjectRepository
{
    public Task<IEnumerable<ProjectInfo>> ListProjects();
    public Task<Project> GetProject(int id);
}