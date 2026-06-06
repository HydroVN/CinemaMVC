using Microsoft.EntityFrameworkCore;
using MVCDatVePhim.Data;
using MVCDatVePhim.Models;

namespace MVCDatVePhim.Services
{
    public class GheService : IGheService
    {
        private readonly ApplicationDbContext _db;

        public GheService(ApplicationDbContext db) => _db = db;

        public async Task<List<Ghe>> LayTheoPhongAsync(int phongChieuId) =>
            await _db.Ghes
                     .Where(g => g.PhongChieuId == phongChieuId)
                     .OrderBy(g => g.HangGhe).ThenBy(g => g.SoGhe)
                     .ToListAsync();

        /// <summary>Returns GheIds already booked for a given showtime (active bookings only).</summary>
        public async Task<List<int>> LayGheDaDatAsync(int suatChieuId) =>
            await _db.ChiTietDatVes
                     .Where(c => c.DatVe.SuatChieuId == suatChieuId
                              && c.DatVe.TrangThai != TrangThaiDatVe.DaHuy)
                     .Select(c => c.GheId)
                     .ToListAsync();

        public async Task<bool> GheTrongAsync(int gheId, int suatChieuId) =>
            !await _db.ChiTietDatVes
                      .AnyAsync(c => c.GheId == gheId
                                  && c.DatVe.SuatChieuId == suatChieuId
                                  && c.DatVe.TrangThai != TrangThaiDatVe.DaHuy);

        public async Task<List<Ghe>> LayTheoIdsAsync(List<int> gheIds) =>
            await _db.Ghes.Where(g => gheIds.Contains(g.Id)).ToListAsync();
    }
}
