using BuildersApp.Core.Enums;
using BuildersApp.Core.Models;

namespace BuildersApp.Core.Repositories;

public interface IProjectRepository
{
    public Task<IEnumerable<ProjectInfo>> ListProjects(IndustryTypes industryType);
    public Task<Project> GetProject(int id);
    public Task<bool> CreateProject(Project project);
}