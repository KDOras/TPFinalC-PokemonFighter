using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WpfCours.Model;

public partial class ExerciceMonsterContext : DbContext
{
    public ExerciceMonsterContext()
    {
    }

    public ExerciceMonsterContext(DbContextOptions<ExerciceMonsterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Login> Logins { get; set; }

    public virtual DbSet<Monster> Monsters { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Spell> Spells { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MSI\\SQLEXPRESS;Database=ExerciceMonster;Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Login__3214EC27402D30B1");

            entity.ToTable("Login");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Monster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Monster__3214EC27932AF3F5");

            entity.ToTable("Monster");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .HasColumnName("ImageURL");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasMany(d => d.Spells).WithMany(p => p.Monsters)
                .UsingEntity<Dictionary<string, object>>(
                    "MonsterSpell",
                    r => r.HasOne<Spell>().WithMany()
                        .HasForeignKey("SpellId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__MonsterSp__Spell__31EC6D26"),
                    l => l.HasOne<Monster>().WithMany()
                        .HasForeignKey("MonsterId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__MonsterSp__Monst__30F848ED"),
                    j =>
                    {
                        j.HasKey("MonsterId", "SpellId").HasName("PK__MonsterS__293EA4DFBCB0EB77");
                        j.ToTable("MonsterSpell");
                        j.IndexerProperty<int>("MonsterId").HasColumnName("MonsterID");
                        j.IndexerProperty<int>("SpellId").HasColumnName("SpellID");
                    });
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Player__3214EC27B2FD30D9");

            entity.ToTable("Player");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LoginId).HasColumnName("LoginID");
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Login).WithMany(p => p.Players)
                .HasForeignKey(d => d.LoginId)
                .HasConstraintName("FK__Player__LoginID__267ABA7A");

            entity.HasMany(d => d.Monsters).WithMany(p => p.Players)
                .UsingEntity<Dictionary<string, object>>(
                    "PlayerMonster",
                    r => r.HasOne<Monster>().WithMany()
                        .HasForeignKey("MonsterId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PlayerMon__Monst__2E1BDC42"),
                    l => l.HasOne<Player>().WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PlayerMon__Playe__2D27B809"),
                    j =>
                    {
                        j.HasKey("PlayerId", "MonsterId").HasName("PK__PlayerMo__378F20A4B30EA4CC");
                        j.ToTable("PlayerMonster");
                        j.IndexerProperty<int>("PlayerId").HasColumnName("PlayerID");
                        j.IndexerProperty<int>("MonsterId").HasColumnName("MonsterID");
                    });
        });

        modelBuilder.Entity<Spell>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Spell__3214EC2754B42A55");

            entity.ToTable("Spell");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
