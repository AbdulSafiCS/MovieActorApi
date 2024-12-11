using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace dataModel;

public partial class MyFirstAppDatabaseContext : IdentityDbContext<AppUser>
{
    public MyFirstAppDatabaseContext()
    {
    }

    public MyFirstAppDatabaseContext(DbContextOptions<MyFirstAppDatabaseContext> options)
        : base(options)
    {
    }

    // DbSet for Actors
    public virtual DbSet<Actor> Actors { get; set; }

    // DbSet for Movies
    public virtual DbSet<Movie> Movies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) { return; }

        IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false);
        IConfigurationRoot configuration = builder.Build();

        optionsBuilder.UseSqlServer(
            configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("myFirstAppApi") // Specify the assembly containing migrations
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Actor Entity Configuration
        modelBuilder.Entity<Actor>(entity =>
        {
            // Relationship between Actor and Movie
            entity.HasOne(a => a.Movie) // An actor belongs to one movie
                .WithMany(m => m.Actors) // A movie can have many actors
                .HasForeignKey(a => a.MovieId) 
                .OnDelete(DeleteBehavior.Cascade) // Cascade delete: delete actors when a movie is deleted
                .HasConstraintName("FK_Actors_Movies"); // Foreign key constraint name

            entity.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100); 

            entity.Property(a => a.CharacterName)
                .IsRequired()
                .HasMaxLength(100); 
        });

        // Movie Entity Configuration
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(100); 

            entity.Property(m => m.Genre)
                .IsRequired()
                .HasMaxLength(50); 

            entity.Property(m => m.Rating)
                .HasColumnType("decimal(3,1)"); 
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
