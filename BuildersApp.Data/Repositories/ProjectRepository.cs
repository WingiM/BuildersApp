using BuildersApp.Core.Enums;
using BuildersApp.Core.Models;
using BuildersApp.Core.Repositories;
using BuildersApp.Data.Models;
using Dapper;

namespace BuildersApp.Data.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationContext _context;

    public ProjectRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectInfo>> ListProjects(IndustryTypes industryType)
    {
        await using var session = _context.GetNpgsqlSession();
        const string sql = @"SELECT id, name, date_created from project WHERE industry_id = @industry_id";

        var res = await session.QueryAsync<ProjectInfo>(sql, new { industry_id = (int)industryType });

        return res;
    }

    public async Task<Project> GetProject(int id)
    {
        await using var session = _context.GetNpgsqlSession();
        var sql = @"SELECT * FROM project WHERE id=@id";
        var projectDb = await session.QueryFirstOrDefaultAsync<ProjectDb>(sql, new { id }) ?? throw new Exception();

        sql = @"SELECT d.id, d.name, pd.is_signed, pd.is_necessary 
                FROM project_document pd 
                    JOIN document d 
                        ON pd.document_id = d.id 
                WHERE pd.project_id = @projectId";
        var documents = await session.QueryAsync<Document>(sql, new { projectId = projectDb.Id });

        sql = @"SELECT id, data ->> 'Name' as ""Name"", role_id FROM ""user"" WHERE id = @id";
        var designer = await session.QueryFirstOrDefaultAsync<UserTuple>(sql, new { id = projectDb.DesignerId });
        var developer = await session.QueryFirstOrDefaultAsync<UserTuple>(sql, new { id = projectDb.DeveloperId });
        var author = await session.QueryFirstOrDefaultAsync<UserTuple>(sql, new { id = projectDb.CreatedById });

        var res = new Project
        {
            Designer = designer, Developer = developer, CreatedBy = author,
            IndustryType = (IndustryTypes)projectDb.IndustryType, Id = projectDb.Id,
            DateCreated = projectDb.DateCreated, Name = projectDb.Name,
            Documents = documents.ToList()
        };

        return res;
    }

    public async Task<bool> CreateProject(Project project)
    {
        await using var session = _context.GetNpgsqlSession();
        var sql = @"INSERT INTO project(name, industry_id, designer_id, developer_id, date_created, created_by) 
                    VALUES (@name, @industry_id, @designer_id, @developer_id, @date_created, @created_by) returning id";

        var projectId = await session.ExecuteScalarAsync<int>(sql,
            new
            {
                name = project.Name, industry_id = (int)project.IndustryType,
                designer_id = project.Designer.Id, developer_id = project.Developer.Id,
                date_created = project.DateCreated, created_by = project.CreatedBy.Id
            });

        sql = @"SELECT id FROM document WHERE industry_id = @industryId";
        var documentList = (await session.QueryAsync<int>(sql,
                new { industryId = (int)project.IndustryType }))
            .ToList()
            .Select(x => new { projectId, documentId = x, dateCreated = DateTime.Now });
        sql = @"INSERT INTO project_document(project_id, document_id, sign_date) 
                    VALUES (@projectId, @documentId, @dateCreated)";

        await session.ExecuteAsync(sql, documentList);

        return true;
    }
}