using Microsoft.EntityFrameworkCore;
using MVCDatVePhim.Data;
using MVCDatVePhim.Models;

namespace MVCDatVePhim.Services
{
    public class DatVeService : IDatVeService
    {
        private readonly ApplicationDbContext _db;
        private readonly IGheService          _ghe;

        public DatVeService(ApplicationDbContext db, IGheService ghe)
        {
            _db  = db;
            _ghe = ghe;
        }

        /// <summary>Creates a booking atomically; returns null if any seat is taken.</summary>
        public async Task<DatVe?> TaoAsync(string userId, int suatChieuId, List<int> gheIds)
        {
            // Verify every seat is still available
            foreach (var gheId in gheIds)
                if (!await _ghe.GheTrongAsync(gheId, suatChieuId))
                    return null;

            var suatChieu = await _db.SuatChieus.FindAsync(suatChieuId);
            if (suatChieu == null) return null;

            var ghes = await _ghe.LayTheoIdsAsync(gheIds);

            // Build booking
            var datVe = new DatVe
            {
                UserId      = userId,
                SuatChieuId = suatChieuId,
                NgayDat     = DateTime.Now,
                TrangThai   = TrangThaiDatVe.DaXacNhan,
                MaVe        = GenerateMaVe(),
                TongTien    = ghes.Sum(g => g.LoaiGhe == LoaiGhe.VIP
                                           ? suatChieu.GiaVe * 1.5m
                                           : suatChieu.GiaVe),
                ChiTietDatVes = gheIds.Select(gId =>
                {
                    var ghe = ghes.First(g => g.Id == gId);
                    return new ChiTietDatVe
                    {
                        GheId  = gId,
                        GiaVe  = ghe.LoaiGhe == LoaiGhe.VIP ? suatChieu.GiaVe * 1.5m : suatChieu.GiaVe
                    };
                }).ToList()
            };

            _db.DatVes.Add(datVe);
            await _db.SaveChangesAsync();
            return datVe;
        }

        public async Task<bool> HuyAsync(int datVeId, string userId)
        {
            var datVe = await _db.DatVes.FirstOrDefaultAsync(d => d.Id == datVeId && d.UserId == userId);
            if (datVe == null || datVe.TrangThai == TrangThaiDatVe.DaHuy)
                return false;

            datVe.TrangThai = TrangThaiDatVe.DaHuy;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<DatVe>> LayTheoNguoiDungAsync(string userId) =>
            await _db.DatVes
                     .Include(d => d.SuatChieu).ThenInclude(s => s.Phim)
                     .Include(d => d.SuatChieu).ThenInclude(s => s.PhongChieu).ThenInclude(pc => pc.RapChieu)
                     .Include(d => d.ChiTietDatVes).ThenInclude(c => c.Ghe)
                     .Where(d => d.UserId == userId)
                     .OrderByDescending(d => d.NgayDat)
                     .ToListAsync();

        public async Task<DatVe?> LayTheoIdAsync(int id) =>
            await _db.DatVes
                     .Include(d => d.User)
                     .Include(d => d.SuatChieu).ThenInclude(s => s.Phim)
                     .Include(d => d.SuatChieu).ThenInclude(s => s.PhongChieu).ThenInclude(pc => pc.RapChieu)
                     .Include(d => d.ChiTietDatVes).ThenInclude(c => c.Ghe)
                     .FirstOrDefaultAsync(d => d.Id == id);

        public async Task<List<DatVe>> LayTatCaAsync() =>
            await _db.DatVes
                     .Include(d => d.User)
                     .Include(d => d.SuatChieu).ThenInclude(s => s.Phim)
                     .OrderByDescending(d => d.NgayDat)
                     .ToListAsync();

        public async Task<int> TongVeHomNayAsync() =>
            await _db.DatVes.CountAsync(d =>
                d.NgayDat.Date == DateTime.Today &&
                d.TrangThai    != TrangThaiDatVe.DaHuy);

        public async Task<decimal> DoanhThuThangNayAsync() =>
            await _db.DatVes
                     .Where(d => d.NgayDat.Month == DateTime.Now.Month
                              && d.NgayDat.Year  == DateTime.Now.Year
                              && d.TrangThai      != TrangThaiDatVe.DaHuy)
                     .SumAsync(d => d.TongTien);

        public async Task<bool> CapNhatTrangThaiAsync(int id, TrangThaiDatVe trangThai)
        {
            var ve = await _db.DatVes.FindAsync(id);
            if (ve == null) return false;

            ve.TrangThai = trangThai;
            await _db.SaveChangesAsync();
            return true;
        }

        private static string GenerateMaVe() =>
            "VE" + DateTime.Now.ToString("yyyyMMddHHmmss") +
            new Random().Next(1000, 9999).ToString();
    }
}
