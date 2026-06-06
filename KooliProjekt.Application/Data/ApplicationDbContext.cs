﻿using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KooliProjekt.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Toode> Tooted { get; set; }
        public DbSet<Klient> Kliendid { get; set; }
        public DbSet<Arve> Arved { get; set; }
        public DbSet<Tellimus> Tellimused { get; set; }
        public DbSet<TellimuseRida> TellimusedRida { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fix incorrect columns from old DB
            modelBuilder.Entity<Klient>(entity =>
            {
                entity.Property(e => e.FirstName).HasColumnName("Eesnimi");
                entity.Property(e => e.LastName).HasColumnName("Perenimi");
            });

            // Tellimus 1 - 1 Arve
            modelBuilder.Entity<Arve>()
                .HasOne(a => a.Tellimus)
                .WithOne(t => t.Arve)
                .HasForeignKey<Arve>(a => a.TellimusId)
                .OnDelete(DeleteBehavior.Restrict); // IMPORTANT

            // Tellimus 1 - many TellimuseRead
            modelBuilder.Entity<TellimuseRida>()
                .HasOne(r => r.Tellimus)
                .WithMany(t => t.TellimuseRead)
                .HasForeignKey(r => r.TellimusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Toode 1 - many TellimuseRead
            modelBuilder.Entity<TellimuseRida>()
                .HasOne(r => r.Toode)
                .WithMany()
                .HasForeignKey(r => r.ToodeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}