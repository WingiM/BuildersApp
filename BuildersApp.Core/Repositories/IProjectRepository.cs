using BuildersApp.Core.Enums;
using BuildersApp.Core.Models;

namespace BuildersApp.Core.Repositories;

public interface IProjectRepository
{
    public Task<IEnumerable<ProjectInfo>> ListProjects(IndustryTypes industryType);
    public Task<Document> GetDocument(int documentId);
    public Task<Project> GetProject(int id, Roles role);
    public Task UpdateDocument(Document document);
    public Task<bool> CreateProject(Project project);
}