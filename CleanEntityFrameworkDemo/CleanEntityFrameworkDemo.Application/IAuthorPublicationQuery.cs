namespace CleanEntityFrameworkDemo.Application;

public interface IAuthorPublicationQuery
{
    AuthorPublicationQueryResultDto GetAuthorPublication(int authorId);
}

public class AuthorPublicationQueryResultDto
{
    public AuthorDto Author { get; set; }
}

public class AuthorDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<ArticleDto> Articles { get; set; }
}

public class ArticleDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<AuthorDto> Authors { get; set; }
}