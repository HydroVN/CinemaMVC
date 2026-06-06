using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCDatVePhim.Models;

namespace MVCDatVePhim.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Phim>         Phims         { get; set; }
        public DbSet<RapChieu>     RapChieus     { get; set; }
        public DbSet<PhongChieu>   PhongChieus   { get; set; }
        public DbSet<Ghe>          Ghes          { get; set; }
        public DbSet<SuatChieu>    SuatChieus    { get; set; }
        public DbSet<DatVe>        DatVes        { get; set; }
        public DbSet<ChiTietDatVe> ChiTietDatVes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── Table & Column Mappings to match assignment requirements ──
            modelBuilder.Entity<Phim>().ToTable("Movies");
            modelBuilder.Entity<Phim>().Property(p => p.TenPhim).HasColumnName("Title");
            modelBuilder.Entity<Phim>().Property(p => p.MoTa).HasColumnName("Description");
            modelBuilder.Entity<Phim>().Property(p => p.ThoiLuong).HasColumnName("Duration");
            modelBuilder.Entity<Phim>().Property(p => p.UrlAnh).HasColumnName("Image_URL");
            modelBuilder.Entity<Phim>().Property(p => p.TheLoai).HasColumnName("Genre_Id");
            modelBuilder.Entity<Phim>().Property(p => p.DangChieu).HasColumnName("Status");
            modelBuilder.Entity<Phim>().Property(p => p.NgayKhoiChieu).HasColumnName("Created_At");

            modelBuilder.Entity<SuatChieu>().ToTable("Showtimes");
            modelBuilder.Entity<SuatChieu>().Property(s => s.PhimId).HasColumnName("Movie_Id");
            modelBuilder.Entity<SuatChieu>().Property(s => s.GiaVe).HasColumnName("Ticket_Price");
            modelBuilder.Entity<SuatChieu>().Property(s => s.ThoiGianBatDau).HasColumnName("Start_Time");
            modelBuilder.Entity<SuatChieu>().Property(s => s.ThoiGianKetThuc).HasColumnName("End_Time");

            modelBuilder.Entity<DatVe>().ToTable("Bookings");
            modelBuilder.Entity<DatVe>().Property(d => d.TongTien).HasColumnName("Total_Price");
            modelBuilder.Entity<DatVe>().Property(d => d.NgayDat).HasColumnName("Booking_Date");
            modelBuilder.Entity<DatVe>().Property(d => d.TrangThai).HasColumnName("Status");

            modelBuilder.Entity<ChiTietDatVe>().ToTable("Booking_Items");
            modelBuilder.Entity<ChiTietDatVe>().Property(c => c.DatVeId).HasColumnName("Booking_Id");
            modelBuilder.Entity<ChiTietDatVe>().Property(c => c.GheId).HasColumnName("Ghe_Id");
            modelBuilder.Entity<ChiTietDatVe>().Property(c => c.GiaVe).HasColumnName("Price");

            // ── Ghe ──────────────────────────────────────────────
            // Unique seat position per room
            modelBuilder.Entity<Ghe>()
                .HasIndex(g => new { g.PhongChieuId, g.HangGhe, g.SoGhe })
                .IsUnique();

            // ── SuatChieu ─────────────────────────────────────────
            modelBuilder.Entity<SuatChieu>()
                .Property(s => s.GiaVe)
                .HasColumnType("decimal(18,2)");

            // ── DatVe ─────────────────────────────────────────────
            modelBuilder.Entity<DatVe>()
                .Property(d => d.TongTien)
                .HasColumnType("decimal(18,2)");

            // Prevent cascade delete from User -> DatVe
            modelBuilder.Entity<DatVe>()
                .HasOne(d => d.User)
                .WithMany(u => u.DanhSachDatVe)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ── ChiTietDatVe ──────────────────────────────────────
            modelBuilder.Entity<ChiTietDatVe>()
                .Property(c => c.GiaVe)
                .HasColumnType("decimal(18,2)");

            // Prevent cascade from Ghe -> ChiTietDatVe (already has cascade from DatVe)
            modelBuilder.Entity<ChiTietDatVe>()
                .HasOne(c => c.Ghe)
                .WithMany(g => g.ChiTietDatVes)
                .HasForeignKey(c => c.GheId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
