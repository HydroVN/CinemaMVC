using MVCDatVePhim.Models;

namespace MVCDatVePhim.Services
{
    public interface IPhimService
    {
        Task<List<Phim>> LayTatCaAsync();
        Task<List<Phim>> LayDangChieuAsync();
        Task<List<Phim>> LaySapChieuAsync();
        Task<Phim?>      LayTheoIdAsync(int id);
        Task<List<Phim>> TimKiemAsync(string? tuKhoa, string? theLoai, DateTime? ngayChieu = null);
        Task             TaoPhimAsync(Phim phim);
        Task             CapNhatPhimAsync(Phim phim);
        Task             XoaPhimAsync(int id);
        Task<bool>       TonTaiAsync(int id);
    }
}
