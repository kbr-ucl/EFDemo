using CleanEntityFrameworkDemo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanEntityFrameworkDemo.Infrastructure.Database {
    public class PublicationContext :DbContext
    {
        public PublicationContext(DbContextOptions<PublicationContext> options)
            : base(options)
        {
        }

        public DbSet<AuthorEntity> Authors { get; set; }
        public DbSet<ArticleEntity> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorEntity>().ToTable("Author");
            modelBuilder.Entity<ArticleEntity>().ToTable("Article");

            // https://stackoverflow.com/questions/64919574/change-name-of-generated-join-table-many-to-many-ef-core-5
            // https://github.com/dotnet/efcore/issues/23258
            modelBuilder.Entity<AuthorEntity>()
                .HasMany(left => left.Articles)
                .WithMany(right => right.Authors)
                .UsingEntity<Dictionary<string, object>>("AuthorArticle",
                    j => j.HasOne<ArticleEntity>()
                        .WithMany()
                        .HasForeignKey("ArticleId"),
                    j => j.HasOne<AuthorEntity>()
                        .WithMany()
                        .HasForeignKey("AuthorId")
                );
        }
    }
}
