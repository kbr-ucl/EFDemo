namespace CleanEntityFrameworkDemo.Domain.Entities;

public class AuthorEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    // https://code-maze.com/dotnet-collections-ienumerable-iqueryable-icollection/
    public ICollection<ArticleEntity> Articles { get; set; }
}