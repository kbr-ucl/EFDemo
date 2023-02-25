using AutoMapper;
using CleanEntityFrameworkDemo.Application;
using CleanEntityFrameworkDemo.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace CleanEntityFrameworkDemo.Infrastructure.QueriesImplentation;

public class AuthorPublicationQuery : IAuthorPublicationQuery
{
    private readonly PublicationContext _db;
    private readonly IMapper _mapper;

    public AuthorPublicationQuery(PublicationContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    AuthorPublicationQueryResultDto IAuthorPublicationQuery.GetAuthorPublication(int authorId)
    {
        var dbResult = _db.Authors.Include(a => a.Articles).ThenInclude(b => b.Authors)
            .FirstOrDefault(c => c.Id == authorId);
        if (dbResult == null) return new AuthorPublicationQueryResultDto();

        var authorDto = _mapper.Map<AuthorDto>(dbResult);
        return new AuthorPublicationQueryResultDto {Author = authorDto};
    }
}