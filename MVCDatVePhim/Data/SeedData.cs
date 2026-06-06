using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCDatVePhim.Data;
using MVCDatVePhim.Models;

namespace MVCDatVePhim.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var db          = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure DB is created / migrated
            await db.Database.MigrateAsync();

            // ── Roles ──────────────────────────────────────────────
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

            // ── Admin User ─────────────────────────────────────────
            if (await userManager.FindByEmailAsync("admin@cineviet.vn") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName    = "admin@cineviet.vn",
                    Email       = "admin@cineviet.vn",
                    HoTen       = "Quản Trị Viên",
                    PhoneNumber = "0901234567",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");
            }

            // ── Only seed once ─────────────────────────────────────
            if (await db.Phims.AnyAsync()) return;

            // ── Rạp Chiếu ─────────────────────────────────────────
            var rap = new RapChieu
            {
                TenRap    = "CineViet Hồ Chí Minh",
                DiaChi    = "123 Nguyễn Huệ, Quận 1",
                ThanhPho  = "Hồ Chí Minh",
                SoDienThoai = "028-1234-5678"
            };
            db.RapChieus.Add(rap);
            await db.SaveChangesAsync();

            // ── Phòng Chiếu + Ghế ─────────────────────────────────
            var phong = new PhongChieu
            {
                RapChieuId = rap.Id,
                TenPhong   = "Phòng 1 — Standard",
                SoHang     = 8,
                SoCot      = 10
            };
            db.PhongChieus.Add(phong);
            await db.SaveChangesAsync();

            // Auto-generate seats (last 2 rows = VIP)
            for (int h = 0; h < phong.SoHang; h++)
            {
                char hang = (char)('A' + h);
                for (int col = 1; col <= phong.SoCot; col++)
                {
                    db.Ghes.Add(new Ghe
                    {
                        PhongChieuId = phong.Id,
                        HangGhe      = hang,
                        SoGhe        = col,
                        LoaiGhe      = h >= phong.SoHang - 2 ? LoaiGhe.VIP : LoaiGhe.Thuong
                    });
                }
            }
            await db.SaveChangesAsync();

            // ── Phim Mẫu ──────────────────────────────────────────
            var phims = new List<Phim>
            {
                new() {
                    TenPhim       = "Avengers: Endgame",
                    MoTa          = "Sau những sự kiện tàn khốc của Avengers: Infinity War, vũ trụ đang nằm trong tình trạng hỗn loạn. Với sự giúp đỡ của các đồng minh còn lại, các Avengers tập hợp thêm một lần nữa để đảo ngược các hành động của Thanos.",
                    TheLoai       = TheLoaiPhim.HanhDong,
                    ThoiLuong     = 181,
                    DanhGia       = 8.4,
                    UrlAnh        = "https://upload.wikimedia.org/wikipedia/en/0/0d/Avengers_Endgame_official_poster.jpg",
                    UrlTrailer    = "https://www.youtube.com/watch?v=TcMBFSGVi1c",
                    NgayKhoiChieu = new DateTime(2019, 4, 26),
                    DangChieu     = true,
                    DaoDien       = "Anthony Russo, Joe Russo",
                    DienVien      = "Robert Downey Jr., Chris Evans, Mark Ruffalo",
                    NgonNgu       = "Tiếng Anh (Lồng tiếng Việt)"
                },
                new() {
                    TenPhim       = "Lật Mặt 7: Một Điều Ước",
                    MoTa          = "Phần tiếp theo của thương hiệu phim Việt đình đám, kể về những câu chuyện xúc động đan xen trong cuộc sống đời thường.",
                    TheLoai       = TheLoaiPhim.TamLy,
                    ThoiLuong     = 128,
                    DanhGia       = 7.2,
                    UrlAnh        = "https://upload.wikimedia.org/wikipedia/vi/b/b7/L%E1%BA%ADt_M%E1%BA%B7t_7_poster.jpg",
                    NgayKhoiChieu = DateTime.Today.AddDays(-7),
                    DangChieu     = true,
                    DaoDien       = "Lý Hải",
                    DienVien      = "Lý Hải, Minh Hà, Louis Nguyễn",
                    NgonNgu       = "Tiếng Việt"
                },
                new() {
                    TenPhim       = "Inside Out 2",
                    MoTa          = "Riley bước vào tuổi thiếu niên với những cảm xúc mới xuất hiện: Lo Lắng, Ganh Tị, Chán Nản và Xấu Hổ.",
                    TheLoai       = TheLoaiPhim.HoatHinh,
                    ThoiLuong     = 100,
                    DanhGia       = 7.8,
                    UrlAnh        = "https://upload.wikimedia.org/wikipedia/en/thumb/9/9e/Inside_Out_2_poster.jpg/220px-Inside_Out_2_poster.jpg",
                    NgayKhoiChieu = DateTime.Today.AddDays(-14),
                    DangChieu     = true,
                    DaoDien       = "Kelsey Mann",
                    DienVien      = "Amy Poehler, Maya Hawke, Kensington Tallman",
                    NgonNgu       = "Tiếng Anh (Lồng tiếng Việt)"
                },
                new() {
                    TenPhim       = "Deadpool & Wolverine",
                    MoTa          = "Wade Wilson đồng hành cùng Logan trong một cuộc phiêu lưu điên rồ xuyên đa vũ trụ.",
                    TheLoai       = TheLoaiPhim.HanhDong,
                    ThoiLuong     = 128,
                    DanhGia       = 8.1,
                    UrlAnh        = "https://upload.wikimedia.org/wikipedia/en/4/44/Deadpool_%26_Wolverine_poster.jpg",
                    NgayKhoiChieu = DateTime.Today.AddDays(14),
                    DangChieu     = false,
                    DaoDien       = "Shawn Levy",
                    DienVien      = "Ryan Reynolds, Hugh Jackman",
                    NgonNgu       = "Tiếng Anh (Lồng tiếng Việt)"
                }
            };
            db.Phims.AddRange(phims);
            await db.SaveChangesAsync();

            // ── Suất Chiếu Mẫu ────────────────────────────────────
            var dangChieuPhims = phims.Where(p => p.DangChieu).ToList();
            var suatChieus = new List<SuatChieu>();
            foreach (var phim in dangChieuPhims)
            {
                for (int day = 0; day < 3; day++)
                {
                    foreach (var hour in new[] { 10, 14, 18, 20 })
                    {
                        suatChieus.Add(new SuatChieu
                        {
                            PhimId          = phim.Id,
                            PhongChieuId    = phong.Id,
                            ThoiGianBatDau  = DateTime.Today.AddDays(day).AddHours(hour),
                            ThoiGianKetThuc = DateTime.Today.AddDays(day).AddHours(hour).AddMinutes(phim.ThoiLuong + 15),
                            GiaVe           = hour >= 18 ? 120000m : 90000m,
                            DangHoatDong    = true
                        });
                    }
                }
            }
            db.SuatChieus.AddRange(suatChieus);
            await db.SaveChangesAsync();
        }
    }
}
