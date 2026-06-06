using Microsoft.EntityFrameworkCore;
using MVCDatVePhim.Data;
using MVCDatVePhim.Models;

namespace MVCDatVePhim.Services
{
    public class PhimService : IPhimService
    {
        private readonly ApplicationDbContext _db;

        public PhimService(ApplicationDbContext db) => _db = db;

        public async Task<List<Phim>> LayTatCaAsync() =>
            await _db.Phims.OrderByDescending(p => p.NgayKhoiChieu).ToListAsync();

        public async Task<List<Phim>> LayDangChieuAsync() =>
            await _db.Phims
                     .Where(p => p.DangChieu && p.NgayKhoiChieu <= DateTime.Today)
                     .OrderByDescending(p => p.DanhGia)
                     .ToListAsync();

        public async Task<List<Phim>> LaySapChieuAsync() =>
            await _db.Phims
                     .Where(p => p.NgayKhoiChieu > DateTime.Today)
                     .OrderBy(p => p.NgayKhoiChieu)
                     .ToListAsync();

        public async Task<Phim?> LayTheoIdAsync(int id) =>
            await _db.Phims
                     .Include(p => p.DanhSachSuatChieu)
                         .ThenInclude(s => s.PhongChieu)
                             .ThenInclude(pc => pc.RapChieu)
                     .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<List<Phim>> TimKiemAsync(string? tuKhoa, string? theLoai, DateTime? ngayChieu = null)
        {
            var query = _db.Phims.AsQueryable();

            if (!string.IsNullOrWhiteSpace(tuKhoa))
                query = query.Where(p =>
                    p.TenPhim.Contains(tuKhoa) ||
                    p.DaoDien.Contains(tuKhoa)  ||
                    p.DienVien.Contains(tuKhoa));

            if (!string.IsNullOrWhiteSpace(theLoai) &&
                Enum.TryParse<TheLoaiPhim>(theLoai, out var tl))
                query = query.Where(p => p.TheLoai == tl);

            if (ngayChieu.HasValue)
            {
                var targetDate = ngayChieu.Value.Date;
                query = query.Where(p => _db.SuatChieus.Any(sc => sc.PhimId == p.Id && sc.ThoiGianBatDau.Date == targetDate && sc.DangHoatDong));
            }

            return await query.OrderByDescending(p => p.DanhGia).ToListAsync();
        }

        public async Task TaoPhimAsync(Phim phim)
        {
            _db.Phims.Add(phim);
            await _db.SaveChangesAsync();
        }

        public async Task CapNhatPhimAsync(Phim phim)
        {
            _db.Phims.Update(phim);
            await _db.SaveChangesAsync();
        }

        public async Task XoaPhimAsync(int id)
        {
            var phim = await _db.Phims.FindAsync(id);
            if (phim != null)
            {
                _db.Phims.Remove(phim);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<bool> TonTaiAsync(int id) =>
            await _db.Phims.AnyAsync(p => p.Id == id);
    }
}
