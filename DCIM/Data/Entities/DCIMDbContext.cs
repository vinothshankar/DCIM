using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DCIM.Data.Entities
{
    public partial class DCIMDbContext : DbContext
    {
        private IWebHostEnvironment _hostingEnv;
        public DCIMDbContext(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnv = hostingEnvironment;
        }

        public DCIMDbContext(DbContextOptions<DCIMDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Connections> Connections { get; set; }
        public virtual DbSet<PortMaster> PortMaster { get; set; }
        public virtual DbSet<Rack> Rack { get; set; }
        public virtual DbSet<RackUnit> RackUnit { get; set; }
        public virtual DbSet<Server> Server { get; set; }
        public virtual DbSet<ServerPortDtl> ServerPortDtl { get; set; }
        public virtual DbSet<ServerSlotDtl> ServerSlotDtl { get; set; }
        public virtual DbSet<SlotMaster> SlotMaster { get; set; }
        public virtual DbSet<SlotPort> SlotPort { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string dbFilePath = _hostingEnv.ContentRootPath + "\\Data\\DCIM.mdf";
                optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + dbFilePath + ";Integrated Security=True;Connect Timeout=30");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Connections>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);
            });

            modelBuilder.Entity<PortMaster>(entity =>
            {
                entity.Property(e => e.PortName).IsFixedLength();
            });

            modelBuilder.Entity<Rack>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ImagePath).IsUnicode(false);

                entity.Property(e => e.Rack1).IsUnicode(false);
            });

            modelBuilder.Entity<RackUnit>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.RectangleXcoordinate).IsUnicode(false);

                entity.Property(e => e.RectangleYcoordinate).IsUnicode(false);

                entity.Property(e => e.Xcoordinate).IsUnicode(false);

                entity.Property(e => e.Ycoordinate).IsUnicode(false);
            });

            modelBuilder.Entity<Server>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ImagePath).IsUnicode(false);

                entity.Property(e => e.Server1).IsUnicode(false);
            });

            modelBuilder.Entity<ServerPortDtl>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Port).IsUnicode(false);
            });

            modelBuilder.Entity<ServerSlotDtl>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.SlotName).IsUnicode(false);
            });

            modelBuilder.Entity<SlotMaster>(entity =>
            {
                entity.Property(e => e.SlotName).IsUnicode(false);
            });

            modelBuilder.Entity<SlotPort>(entity =>
            {
                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.PortName).IsUnicode(false);

            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
