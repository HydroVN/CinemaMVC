using MVCDatVePhim.Models;

namespace MVCDatVePhim.Services
{
    public interface IDatVeService
    {
        Task<DatVe?>      TaoAsync(string userId, int suatChieuId, List<int> gheIds);
        Task<bool>        HuyAsync(int datVeId, string userId);
        Task<List<DatVe>> LayTheoNguoiDungAsync(string userId);
        Task<DatVe?>      LayTheoIdAsync(int id);
        Task<List<DatVe>> LayTatCaAsync();
        Task<int>         TongVeHomNayAsync();
        Task<decimal>     DoanhThuThangNayAsync();
        Task<bool>        CapNhatTrangThaiAsync(int id, TrangThaiDatVe trangThai);
    }
}
