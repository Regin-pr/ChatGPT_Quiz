using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GPTQuiz_WPF
{
    public partial class GPTQuiz_DBContext : DbContext
    {
        public GPTQuiz_DBContext()
        {
        }

        public GPTQuiz_DBContext(DbContextOptions<GPTQuiz_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Antwort> Antworten { get; set; } = null!;
        public virtual DbSet<Frage> Fragen { get; set; } = null!;
        public virtual DbSet<Teilnehmer> Teilnehmers { get; set; } = null!;
        public virtual DbSet<ThemaText> ThemaTexte { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GPTQuiz_DB;Integrated Security=SSPI");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Antwort>(entity =>
            {
                entity.HasKey(e => new { e.FrageId, e.Buchstabe })
                    .HasName("PK_FrageBuchstabe");

                entity.ToTable("Antwort");

                entity.Property(e => e.FrageId).HasColumnName("FrageID");

                entity.Property(e => e.Buchstabe)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Text).HasMaxLength(200);

                entity.HasOne(d => d.Frage)
                    .WithMany(p => p.Antworts)
                    .HasForeignKey(d => d.FrageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Antwort__FrageID__787EE5A0");
            });

            modelBuilder.Entity<Frage>(entity =>
            {
                entity.ToTable("Frage");

                entity.Property(e => e.FrageId).HasColumnName("FrageID");

                entity.Property(e => e.Text).HasMaxLength(200);

                entity.Property(e => e.ThemaId).HasColumnName("ThemaID");

                entity.HasOne(d => d.Thema)
                    .WithMany(p => p.Fragen)
                    .HasForeignKey(d => d.ThemaId)
                    .HasConstraintName("FK__Frage__ThemaID__75A278F5");
            });

            modelBuilder.Entity<Teilnehmer>(entity =>
            {
                entity.ToTable("Teilnehmer");

                entity.Property(e => e.TeilnehmerId).HasColumnName("TeilnehmerID");

                entity.Property(e => e.Name).HasMaxLength(15);

                entity.Property(e => e.PunkteGesamt).HasDefaultValueSql("((0))");

                entity.Property(e => e.PunkteRunde).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<ThemaText>(entity =>
            {
                entity.HasKey(e => e.ThemaId)
                    .HasName("PK__Thema_Te__F08485F21F627D02");

                entity.ToTable("Thema_Text");

                entity.Property(e => e.ThemaId).HasColumnName("ThemaID");

                entity.Property(e => e.Bezeichnung).HasMaxLength(4000);

                entity.Property(e => e.IstThema).HasColumnName("IstThema");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
