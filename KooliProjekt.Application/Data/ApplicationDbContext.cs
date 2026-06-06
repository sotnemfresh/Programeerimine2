using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Toode> Tooted { get; set; }
        public DbSet<Klient> Kliendid { get; set; }
        public DbSet<Arve> Arved { get; set; }
        public DbSet<Tellimus> Tellimused { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Lisame lihtsad konfiguratsioonid, et EF teaks suhted ja cascade käitumise.
            modelBuilder.Entity<Klient>()
                .HasMany(k => k.Arved)
                .WithOne(a => a.Klient)
                .HasForeignKey(a => a.KlientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Arve>()
                .HasMany(a => a.Tellimused)
                .WithOne(t => t.Arve)
                .HasForeignKey(t => t.ArveId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Toode>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<Tellimus>()
                .HasOne(t => t.Toode)
                .WithMany()
                .HasForeignKey(t => t.ToodeId)
                .OnDelete(DeleteBehavior.Restrict); // ära kustuta toodet kui real on viiteid
        }
    }
}
