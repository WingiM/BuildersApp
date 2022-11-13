using BuildersApp.Core.Enums;
using BuildersApp.Core.Models;
using BuildersApp.Core.Repositories;
using BuildersApp.Data.Models;
using Dapper;
using MongoDB.Driver;

namespace BuildersApp.Data.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationContext _context;
    private readonly IMongoCollection<BaseDesignerForm> _forms;

    public ProjectRepository(ApplicationContext context, MongoConnection connection)
    {
        _context = context;
        _forms = connection.Database!.GetCollection<BaseDesignerForm>("Forms");
    }

    public async Task<IEnumerable<ProjectInfo>> ListProjects(IndustryTypes industryType)
    {
        await using var session = _context.GetNpgsqlSession();
        const string sql =
            @"SELECT p.id, name, date_created, data ->> 'Name' as ""AuthorName"" from project p LEFT JOIN ""user"" on p.created_by = ""user"".id WHERE industry_id = @industry_id";

        var res = await session.QueryAsync<ProjectInfo>(sql, new { industry_id = (int)industryType });

        return res;
    }

    public async Task<IEnumerable<ProjectInfo>> ListProjects(Roles role, int id)
    {
        await using var session = _context.GetNpgsqlSession();
        string sql =
            @"SELECT project.id, name, date_created, data ->> 'Name' as ""AuthorName"" 
                from project 
                    JOIN ""user"" u on project.created_by = u.id WHERE ";
        if (role == Roles.Designer)
        {
            sql += "designer_id = @id";
        }
        else if (role == Roles.Developer)
        {
            sql += "developer_id = @id";
        }

        var res = await session.QueryAsync<ProjectInfo>(sql, new { id });

        return res;
    }

    public async Task<IEnumerable<Document>> ListMissingDocumentsForProject(int projectId, IndustryTypes industryType)
    {
        await using var session = _context.GetNpgsqlSession();
        var sql =
            @"SELECT * FROM document d 
                WHERE industry_id in (@industryId, 1) 
                  AND id NOT IN 
                      (SELECT document_id FROM project_document WHERE project_id = @projectId)";

        return await session.QueryAsync<Document>(sql, new { projectId, industryId = (int)industryType });
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
            sql += " AND d.role_id = @roleId";
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
            IndustryType = (IndustryTypes)projectDb.IndustryId, Id = projectDb.Id,
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

        await session.ExecuteAsync(sql,
            new
            {
                name = project.Name, industry_id = (int)project.IndustryType,
                designer_id = project.DesignerId, developer_id = project.DeveloperId,
                date_created = project.DateCreated, created_by = project.CreatedById
            });

        return true;
    }

    public async Task AddDocumentToProject(int projectId, int documentId)
    {
        await using var session = _context.GetNpgsqlSession();
        var sql = @"INSERT INTO project_document(project_id, document_id) VALUES (@projectId, @documentId)";
        await session.ExecuteAsync(sql, new { projectId, documentId });
    }

    public async Task<BaseDesignerForm?> GetForm(int projectId)
    {
        return await (await _forms.FindAsync(x => x.ProjectId == projectId)).SingleOrDefaultAsync();
    }

    public async Task AddForm(BaseDesignerForm gasForm)
    {
        await _forms.InsertOneAsync(gasForm);
    }
}