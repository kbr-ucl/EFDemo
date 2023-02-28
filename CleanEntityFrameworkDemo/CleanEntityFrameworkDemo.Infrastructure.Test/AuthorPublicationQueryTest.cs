using System.Data;
using AutoMapper;
using CleanEntityFrameworkDemo.Application;
using CleanEntityFrameworkDemo.Domain.Entities;
using CleanEntityFrameworkDemo.Infrastructure.Database;
using CleanEntityFrameworkDemo.Infrastructure.Mapping;
using CleanEntityFrameworkDemo.Infrastructure.QueriesImplentation;
using Moq;
using Moq.EntityFrameworkCore;

namespace CleanEntityFrameworkDemo.Infrastructure.Test;

public class AuthorPublicationQueryTest
{
    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(2, 1, 2)]
    [InlineData(3, 2, 1)]
    public void GetAuthorPublication(int authorId, 
        int atricleCount, int authorCount)
    {
        // Arrange
        var authorsList = GetFakeAuthorsList(); //new List<AuthorEntity>();
        var articlesList =  GetFakeArticlesList(authorsList); //new List<ArticleEntity>();

        var publicationContextMock = new Mock<PublicationContext>();
        publicationContextMock.Setup(x => x.Authors).ReturnsDbSet(authorsList);
        publicationContextMock.Setup(x => x.Articles).ReturnsDbSet(articlesList);

        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<QueryProfile>();
        });

        var mapper = new Mapper(mapperConfiguration);

        var sut = new AuthorPublicationQuery(publicationContextMock.Object, mapper)
            as IAuthorPublicationQuery;


        // Act
        var actual = sut.GetAuthorPublication(authorId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(atricleCount, actual.Author.Articles.Count);
        Assert.Equal(authorCount, actual.Author.Articles.
            SelectMany(a => a.Authors).DistinctBy(a => a.Id).Count());

    }



    private List<AuthorEntity> GetFakeAuthorsList()
    {
        var authors = new List<AuthorEntity>();

        var author = new AuthorEntity {Id = 1, Name = "Author 1"};
        var coAuthor = new AuthorEntity {Id = 2, Name = "Author 2"};
        var noAuthor = new AuthorEntity {Id = 3, Name = "Author 3"};

        authors.Add(author);
        authors.Add(coAuthor);
        authors.Add(noAuthor);


        return authors;
    }

    private List<ArticleEntity> GetFakeArticlesList(List<AuthorEntity> authorsList)
    {
        var articles = new List<ArticleEntity>();
        var artikel1 = new ArticleEntity {Id = 1, Title = "Artikel 1"};
        var artikel2 = new ArticleEntity {Id = 2, Title = "Artikel 2"};
        var artikel3 = new ArticleEntity {Id = 3, Title = "Artikel 3"};
        articles.Add(artikel1);
        articles.Add(artikel2);
        articles.Add(artikel3);

        var authors = authorsList.Where(a => a.Id <= 2).ToList();
        authors.ForEach(a => a.Articles = new List<ArticleEntity>(new []{artikel1}));
        artikel1.Authors = authors;

        var noAuth = authorsList.Where(a => a.Id == 3).ToList();
        var noAuthArticles = new List<ArticleEntity>(new []{artikel2, artikel3});
        noAuth.ForEach(a => a.Articles= noAuthArticles);

        noAuthArticles.ForEach(a => a.Authors = noAuth);

        return articles;
    }
}