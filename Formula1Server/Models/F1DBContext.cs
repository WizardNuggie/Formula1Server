using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Formula1Server.Models;

public partial class F1DBContext : DbContext
{
    public F1DBContext()
    {
    }

    public F1DBContext(DbContextOptions<F1DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=DbSql;User ID=AdminLogin;Password=rokazyo123;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.ArticleId).HasName("PK__Articles__9C6270E8E29194F8");

            entity.HasMany(d => d.Subjects).WithMany(p => p.Articles)
                .UsingEntity<Dictionary<string, object>>(
                    "ArticlesSubject",
                    r => r.HasOne<Subject>().WithMany()
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ArticlesS__Subje__31EC6D26"),
                    l => l.HasOne<Article>().WithMany()
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__ArticlesS__Artic__30F848ED"),
                    j =>
                    {
                        j.HasKey("ArticleId", "SubjectId").HasName("PK__Articles__06A3CAD282A5B1D4");
                        j.ToTable("ArticlesSubjects");
                    });

            entity.HasMany(d => d.Writers).WithMany(p => p.Articles)
                .UsingEntity<Dictionary<string, object>>(
                    "WritersArticle",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("WriterId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__WritersAr__Write__2C3393D0"),
                    l => l.HasOne<Article>().WithMany()
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__WritersAr__Artic__2B3F6F97"),
                    j =>
                    {
                        j.HasKey("ArticleId", "WriterId").HasName("PK__WritersA__80E46FE531B90D3E");
                        j.ToTable("WritersArticles");
                    });
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__Subjects__AC1BA3A827AB637D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CD5E1ABC4");

            entity.HasOne(d => d.UserType).WithMany(p => p.Users).HasConstraintName("FK__Users__UserTypeI__267ABA7A");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("PK__UserType__40D2D8162B88EB1A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
