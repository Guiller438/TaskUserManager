using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TaskUserManager.Models;

namespace TaskUserManager.Data;

public partial class DbAb0bdeTalentseedsContext : DbContext
{
    public DbAb0bdeTalentseedsContext()
    {
    }

    public DbAb0bdeTalentseedsContext(DbContextOptions<DbAb0bdeTalentseedsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TfaCategory> TfaCategories { get; set; }

    public virtual DbSet<TfaRol> TfaRols { get; set; }

    public virtual DbSet<TfaTask> TfaTasks { get; set; }

    public virtual DbSet<TfaTeam> TfaTeams { get; set; }

    public virtual DbSet<TfaTeamstatus> TfaTeamstatuses { get; set; }

    public virtual DbSet<TfaUser> TfaUsers { get; set; }

    public virtual DbSet<TfaUsersTask> TfaUsersTasks { get; set; }
    public virtual DbSet<TfaTeamsCategories> TfaTeamsCategories { get; set; }
    public virtual DbSet<TfaTeamsColaborators> TfaTeamsColaborators { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AI");

        modelBuilder.Entity<TfaCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__TFA_CATE__23CAF1F8D2B0FF74");

            entity.ToTable("TFA_CATEGORIES");

            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.CategoryDeadLine).HasColumnName("categoryDeadLine");
            entity.Property(e => e.CategoryDescription)
                .HasMaxLength(100)
                .HasColumnName("categoryDescription");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("categoryName");
            entity.Property(e => e.CategoryPoints).HasColumnName("categoryPoints");
            entity.Property(e => e.ReducePoints).HasColumnName("reducePoints");
        });

        modelBuilder.Entity<TfaRol>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__TFA_ROL__5402365444BEE72A");

            entity.ToTable("TFA_ROL");

            entity.Property(e => e.RolId).HasColumnName("rolID");
            entity.Property(e => e.RolDescription)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("rolDescription");
            entity.Property(e => e.RolName)
                .HasMaxLength(50)
                .HasColumnName("rolName");
        });

        modelBuilder.Entity<TfaTask>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("PK__TFA_TASK__DD5D55A251682F04");

            entity.ToTable("TFA_TASKS");

            entity.Property(e => e.TaskId).HasColumnName("taskID");
            entity.Property(e => e.CategoryId).HasColumnName("categoryID");
            entity.Property(e => e.TaskName)
                .HasMaxLength(50)
                .HasColumnName("taskName");

            entity.HasOne(d => d.Category).WithMany(p => p.TfaTasks)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_categoryID");
        });

        modelBuilder.Entity<TfaTeam>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("PK__TFA_TEAM__5ED7534A6BB50E22");

            entity.ToTable("TFA_TEAMS");

            entity.Property(e => e.TeamId).HasColumnName("teamID");
            entity.Property(e => e.CategoriesId).HasColumnName("categoriesID");
            entity.Property(e => e.TeamDescription)
                .HasMaxLength(50)
                .HasColumnName("teamDescription");
            entity.Property(e => e.TeamLeadId).HasColumnName("teamLeadID");
            entity.Property(e => e.TeamName)
                .HasMaxLength(50)
                .HasColumnName("teamName");
            entity.Property(e => e.TeamStatusId).HasColumnName("teamStatusID");

            entity.HasOne(d => d.Categories).WithMany(p => p.TfaTeams)
                .HasForeignKey(d => d.CategoriesId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TFA_TEAMS_TFA_CATEGORIES");

            entity.HasOne(d => d.TeamLead).WithMany(p => p.TfaTeams)
                .HasForeignKey(d => d.TeamLeadId)
                .HasConstraintName("FK_teamLead");

            entity.HasOne(d => d.TeamStatus).WithMany(p => p.TfaTeams)
                .HasForeignKey(d => d.TeamStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_teamStatusID");

            entity.HasMany(d => d.TeamCollaborators)
                    .WithOne(p => p.Team) // Relación hacia TfaTeam
                    .HasForeignKey(p => p.ColaboratorTeamID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TFA_TEAMS_COLLABORATOR");

        });

        modelBuilder.Entity<TfaTeamstatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__TFA_TEAM__36257A388E96798A");

            entity.ToTable("TFA_TEAMSTATUS");

            entity.Property(e => e.StatusId).HasColumnName("statusID");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("statusName");
        });

        modelBuilder.Entity<TfaUser>(entity =>
        {
            entity.HasKey(e => e.UsersId).HasName("PK__TFA_USER__788FDD2552307940");

            entity.ToTable("TFA_USERS");

            entity.Property(e => e.UsersId).HasColumnName("usersID");
            entity.Property(e => e.Contrasenia)
                .HasMaxLength(999)
                .IsUnicode(false)
                .HasColumnName("contrasenia");
            entity.Property(e => e.RolId).HasColumnName("rolID");
            entity.Property(e => e.RolIdaddional).HasColumnName("rolIDAddional");
            entity.Property(e => e.UrlImage)
                .HasMaxLength(999)
                .IsUnicode(false)
                .HasColumnName("url_image");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(100)
                .HasColumnName("userEmail");
            entity.Property(e => e.UserLastName)
                .HasMaxLength(50)
                .HasColumnName("userLastName");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("userName");
            entity.Property(e => e.UserPoints).HasColumnName("userPoints");

            entity.HasOne(d => d.Rol).WithMany(p => p.TfaUserRols)
                .HasForeignKey(d => d.RolId)
                .HasConstraintName("FK_userRol");

            entity.HasOne(d => d.RolIdaddionalNavigation).WithMany(p => p.TfaUserRolIdaddionalNavigations)
                .HasForeignKey(d => d.RolIdaddional)
                .HasConstraintName("FK_userRolAddional");

            entity.HasMany(d => d.ColaboratorTeams)
                    .WithOne(p => p.User) // Relación hacia TfaUser
                    .HasForeignKey(p => p.ColaboratorUsersID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TFA_USERS_COLLABORATOR");

        });

        modelBuilder.Entity<TfaTeamsColaborators>(entity =>
        {
            entity.HasKey(e => new { e.ColaboratorTeamID, e.ColaboratorUsersID });

            entity.HasOne(e => e.Team)
                .WithMany(e => e.TeamCollaborators)
                .HasForeignKey(e => e.ColaboratorTeamID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TFA_TEAMS_COLLABORATOR");

            entity.HasOne(e => e.User)
                .WithMany(e => e.ColaboratorTeams)
                .HasForeignKey(e => e.ColaboratorUsersID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TFA_USERS_COLLABORATOR");

            entity.ToTable("TFA_TEAMS_COLABORATORS");
        });

        modelBuilder.Entity<TfaTeamsCategories>(entity =>
        {
            // Definir clave primaria compuesta
            entity.HasKey(e => new { e.TeamId, e.CategoriesId });

            // Configurar relaciones y claves foráneas
            entity.HasOne(e => e.Team)
                .WithMany(t => t.TeamsCategories)
                .HasForeignKey(e => e.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TFA_TEAMS_CATEGORIES_TEAM");

            entity.HasOne(e => e.Category)
                .WithMany(c => c.TeamsCategories)
                .HasForeignKey(e => e.CategoriesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TFA_TEAMS_CATEGORIES_CATEGORY");

            // Definir nombre de la tabla
            entity.ToTable("TFA_TEAMS_CATEGORIES");
        });

        modelBuilder.Entity<TfaUsersTask>(entity =>
        {
            // Definir la clave primaria compuesta
            entity.HasKey(e => new { e.UserTaskId, e.UserId })
                  .HasName("PK_TFA_USERS_TASK");

            // Configurar relación con TfaUser (clave foránea UserId)
            entity.HasOne(e => e.User)
                  .WithMany(u => u.TfaUsersTasks)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_TFA_USERS_TASK_USER");

            // Configurar relación con TfaTask (clave foránea UserTaskId)
            entity.HasOne(e => e.UserTask)
                  .WithMany(t => t.TfaUsersTasks)
                  .HasForeignKey(e => e.UserTaskId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK_TFA_USERS_TASK_TASK");

            // Opcional: Nombre de la tabla en la base de datos
            entity.ToTable("TFA_USERS_TASKS");
        });




        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
