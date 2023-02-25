using AutoMapper;
using CleanEntityFrameworkDemo.Application;
using CleanEntityFrameworkDemo.Domain.Entities;

namespace CleanEntityFrameworkDemo.Infrastructure.Mapping;

// https://docs.automapper.org/en/stable/Configuration.html
public class QueryProfile : Profile
{
    public QueryProfile()
    {
        CreateMap<AuthorEntity, AuthorDto>().MaxDepth(2);
        CreateMap<ArticleEntity, ArticleDto>().PreserveReferences();
    }
}