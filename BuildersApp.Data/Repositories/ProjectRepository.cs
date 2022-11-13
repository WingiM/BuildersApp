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
        const string sql =
            @"SELECT p.id, name, date_created, data ->> 'Name' as ""AuthorName"" from project p LEFT JOIN ""user"" on p.created_by = ""user"".id WHERE industry_id = @industry_id";

        var res = await session.QueryAsync<ProjectInfo>(sql, new { industry_id = (int)industryType });

        return res;
    }

    public async Task<Document> GetDocument(int documentId)
    {
        await using var session = _context.GetNpgsqlSession();
        var sql =
            @"SELECT pd.id, d.name, pd.date_created, pd.sign_date as ""DateSigned"", pd.is_signed, pd.is_necessary 
                FROM project_document pd 
                    JOIN document d 
                        ON pd.document_id = d.id 
                WHERE pd.id = @documentId";

        return await session.QueryFirstAsync<Document>(sql, new { documentId });
    }

    public async Task<IEnumerable<ProjectInfo>> ListProjectsToDeveloper(int developerId)
    {
        await using var session = _context.GetNpgsqlSession();
        const string sql =
            @"SELECT id, name, date_created, data ->> 'Name' as ""AuthorName"" from project JOIN ""user"" on project.created_by = ""user"".id WHERE developer_id = @developer_id";

        var res = await session.QueryAsync<ProjectInfo>(sql, new { developer_id = developerId });

        return res;
    }

    public async Task<IEnumerable<ProjectInfo>> ListProjectsToDesigner(int designerId)
    {
        await using var session = _context.GetNpgsqlSession();
        const string sql =
            @"SELECT p.id, name, date_created, data ->> 'Name' as ""AuthorName"" from project p JOIN ""user"" on p.created_by = ""user"".id WHERE designer_id = @designer_id";

        var res = await session.QueryAsync<ProjectInfo>(sql, new { designer_id = designerId });

        return res;
    }

    public async Task<Project> GetProject(int id, Roles role)
    {
        await using var session = _context.GetNpgsqlSession();
        var parameters = new DynamicParameters();
        var sql = @"SELECT * FROM project WHERE id=@id";
        var projectDb = await session.QueryFirstOrDefaultAsync<ProjectDb>(sql, new { id }) ?? throw new Exception();

        sql = @"SELECT pd.id, d.name, pd.date_created, pd.sign_date as ""DateSigned"", pd.is_signed, pd.is_necessary 
                FROM project_document pd 
                    JOIN document d 
                        ON pd.document_id = d.id 
                WHERE pd.project_id = @projectId";
        if (role != Roles.Customer)
        {
            sql += "AND d.role_id = @roleId";
            parameters.Add("roleId", (int)role);
        }

        parameters.Add("projectId", projectDb.Id);

        var documents = await session.QueryAsync<Document>(sql, parameters);

        sql = @"SELECT id, data ->> 'Name' as ""Name"", role_id FROM ""user"" WHERE id = @id";
        var designer = await session.QueryFirstOrDefaultAsync<UserTuple>(sql, new { id = projectDb.DesignerId });
        var developer = await session.QueryFirstOrDefaultAsync<UserTuple>(sql, new { id = projectDb.DeveloperId });
        var author = await session.QueryFirstOrDefaultAsync<UserTuple>(sql, new { id = projectDb.CreatedById });

        var res = new Project
        {
            Designer = designer, Developer = developer, CreatedBy = author,
            DeveloperId = developer.Id, DesignerId = designer.Id,
            IndustryType = (IndustryTypes)projectDb.IndustryType, Id = projectDb.Id,
            DateCreated = projectDb.DateCreated, Name = projectDb.Name,
            Documents = documents.ToList()
        };

        return res;
    }

    public async Task UpdateDocument(Document document)
    {
        await using var session = _context.GetNpgsqlSession();
        var parameters = new DynamicParameters();

        var sql = @"SELECT is_signed FROM project_document WHERE id=@documentId";
        var existingDocumentSigned = await session.QuerySingleAsync<bool>(sql, new { documentId = document.Id });

        sql = "UPDATE project_document SET ";
        if (!existingDocumentSigned && document.IsSigned)
        {
            sql += "sign_date = @signDate, ";
            parameters.Add("signDate", DateTime.Now);
        }
        else if (existingDocumentSigned && !document.IsSigned)
        {
            sql += "sign_date = @signDate, ";
            parameters.Add("signDate", null);
        }

        sql += "is_signed = @isSigned, is_necessary = @isNecessary WHERE id=@documentId";
        parameters.Add("isSigned", document.IsSigned);
        parameters.Add("isNecessary", document.IsNecessary);
        parameters.Add("documentId", document.Id);
        await session.ExecuteAsync(sql, parameters);
    }

    public async Task<bool> CreateProject(Project project)
    {
        await using var session = _context.GetNpgsqlSession();
        var sql = @"INSERT INTO project(name, industry_id, designer_id, developer_id, created_by) 
                    VALUES (@name, @industry_id, @designer_id, @developer_id, @created_by) returning id";

        var projectId = await session.ExecuteScalarAsync<int>(sql,
            new
            {
                name = project.Name, industry_id = (int)project.IndustryType,
                designer_id = project.DesignerId, developer_id = project.DeveloperId,
                date_created = project.DateCreated, created_by = project.CreatedById
            });

        sql = @"SELECT id FROM document WHERE industry_id = @industryId";
        var documentList = (await session.QueryAsync<int>(sql,
                new { industryId = (int)project.IndustryType }))
            .ToList()
            .Select(x => new { projectId, documentId = x, dateCreated = DateTime.Now });
        sql = @"INSERT INTO project_document(project_id, document_id, date_created) 
                    VALUES (@projectId, @documentId, @dateCreated)";

        await session.ExecuteAsync(sql, documentList);

        return true;
    }
}