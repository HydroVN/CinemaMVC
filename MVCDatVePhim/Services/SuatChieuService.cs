using Microsoft.EntityFrameworkCore;
using MVCDatVePhim.Data;
using MVCDatVePhim.Models;

namespace MVCDatVePhim.Services
{
    public class SuatChieuService : ISuatChieuService
    {
        private readonly ApplicationDbContext _db;

        public SuatChieuService(ApplicationDbContext db) => _db = db;

        public async Task<List<SuatChieu>> LayTheoPhimAsync(int phimId, DateTime? ngay = null)
        {
            var query = _db.SuatChieus
                .Include(s => s.PhongChieu).ThenInclude(pc => pc.RapChieu)
                .Where(s => s.PhimId == phimId && s.DangHoatDong && s.ThoiGianBatDau >= DateTime.Now);

            if (ngay.HasValue)
                query = query.Where(s => s.ThoiGianBatDau.Date == ngay.Value.Date);

            return await query.OrderBy(s => s.ThoiGianBatDau).ToListAsync();
        }

        public async Task<SuatChieu?> LayTheoIdAsync(int id) =>
            await _db.SuatChieus
                     .Include(s => s.Phim)
                     .Include(s => s.PhongChieu).ThenInclude(pc => pc.RapChieu)
                     .Include(s => s.PhongChieu).ThenInclude(pc => pc.DanhSachGhe)
                     .FirstOrDefaultAsync(s => s.Id == id);

        public async Task<List<SuatChieu>> LayTatCaAsync() =>
            await _db.SuatChieus
                     .Include(s => s.Phim)
                     .Include(s => s.PhongChieu).ThenInclude(pc => pc.RapChieu)
                     .OrderByDescending(s => s.ThoiGianBatDau)
                     .ToListAsync();

        public async Task TaoSuatChieuAsync(SuatChieu suatChieu)
        {
            _db.SuatChieus.Add(suatChieu);
            await _db.SaveChangesAsync();
        }

        public async Task CapNhatAsync(SuatChieu suatChieu)
        {
            _db.SuatChieus.Update(suatChieu);
            await _db.SaveChangesAsync();
        }

        public async Task XoaAsync(int id)
        {
            var sc = await _db.SuatChieus.FindAsync(id);
            if (sc != null) { _db.SuatChieus.Remove(sc); await _db.SaveChangesAsync(); }
        }

        public async Task<bool> TonTaiAsync(int id) =>
            await _db.SuatChieus.AnyAsync(s => s.Id == id);

        public async Task<bool> KiemTraTrungLichAsync(int phongChieuId, DateTime batDau, DateTime ketThuc, int? suatChieuIdExclude = null)
        {
            var query = _db.SuatChieus.Where(sc => 
                sc.PhongChieuId == phongChieuId && 
                sc.DangHoatDong && 
                batDau < sc.ThoiGianKetThuc && 
                ketThuc > sc.ThoiGianBatDau);

            if (suatChieuIdExclude.HasValue && suatChieuIdExclude.Value > 0)
            {
                query = query.Where(sc => sc.Id != suatChieuIdExclude.Value);
            }

            return await query.AnyAsync();
        }
    }
}
