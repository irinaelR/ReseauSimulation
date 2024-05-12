
using Microsoft.EntityFrameworkCore;
using System;
using NetworkSimulation.Entities;


namespace Dentisterie.Data
{
    public class ReseauDbContext : DbContext
    {

        public DbSet<Serveur> serveurs { get; set; }
        public DbSet<UrlMapping> dns { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Database=reseausim;Username=postgres;Password=olafienby7;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Dent>(entity =>
            //{
            //    entity.ToTable("dents");

            //    entity.HasKey(e => e.Id);

            //    entity.Property(e => e.Id)
            //        .IsRequired()
            //        .HasColumnName("id");
            //    entity.Property(e => e.PrixReparation)
            //        .IsRequired()
            //        .HasColumnName("prix_reparation")
            //        .HasColumnType("numeric(14,2)");
            //    entity.Property(e => e.PrixRemplacement)
            //        .IsRequired()
            //        .HasColumnName("prix_remplacement")
            //        .HasColumnType("numeric(14,2)");
            //    entity.Property(e => e.PrixDetartrage)
            //        .IsRequired()
            //        .HasColumnName("prix_detartrage")
            //        .HasColumnType("numeric(14,2)");
            //    entity.Property(e => e.PrixExtraction)
            //        .IsRequired()
            //        .HasColumnName("prix_extraction")
            //        .HasColumnType("numeric(14,2)");
            //});

            //modelBuilder.Entity<Patient>(entity =>
            //{
            //    entity.ToTable("patients");

            //    entity.HasKey(e => e.Id);

            //    entity.Property(e => e.Id)
            //        .IsRequired()
            //        .HasColumnName("id")
            //        .HasColumnType("serial");
            //    entity.Property(e => e.Nom)
            //        .IsRequired()
            //        .HasColumnName("nom")
            //        .HasMaxLength(200);
            //    entity.Property(e => e.Dtn)
            //        .IsRequired()
            //        .HasColumnName("dtn");
            //});

            //modelBuilder.Entity<Consultation>(entity =>
            //{
            //    entity.ToTable("consultations");

            //    entity.HasKey(e => e.Id);

            //    entity.Property(e => e.Id)
            //        .IsRequired()
            //        .HasColumnName("id")
            //        .HasColumnType("serial");
            //    entity.Property(e => e.IdPatient)
            //        .HasColumnName("id_patient");
            //    entity.Property(e => e.DateConsultation)
            //        .HasColumnName("date_consultation");

            //    entity
            //        .HasOne(c => c.Patient)
            //        .WithMany(p => p.Consultations)
            //        .HasForeignKey(c => c.IdPatient)
            //        .HasConstraintName("FK_Consultations_Patient");

            //});

            //modelBuilder.Entity<Diagnostic>(entity =>
            //{
            //    entity.ToTable("diagnostics");

            //    entity.HasKey(e => e.Id);

            //    entity.Property(e => e.Id)
            //        .IsRequired()
            //        .HasColumnName("id")
            //        .HasColumnType("serial");
            //    entity.Property(e => e.Iddent)
            //        .HasColumnName("id_dent");
            //    entity.Property(e => e.IdConsultation)
            //        .HasColumnName("id_consultation");
            //    entity.Property(e => e.Note)
            //        .HasColumnName("note");

            //    entity
            //        .HasOne(c => c.Consultation)
            //        .WithMany(p => p.Diagnostics)
            //        .HasForeignKey(c => c.IdConsultation)
            //        .HasConstraintName("FK_Consultations_Diagnostics");

            //    entity
            //        .HasOne(c => c.Dent)
            //        .WithMany(p => p.Diagnostics)
            //        .HasForeignKey(c => c.Iddent)
            //        .HasConstraintName("FK_Dents_Diagnostics");

            //});

            // OnModelCreatingPartial(modelBuilder);
        }

        // partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}