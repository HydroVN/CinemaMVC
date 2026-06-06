using MVCDatVePhim.Models;

namespace MVCDatVePhim.Services
{
    public interface ISuatChieuService
    {
        Task<List<SuatChieu>> LayTheoPhimAsync(int phimId, DateTime? ngay = null);
        Task<SuatChieu?>      LayTheoIdAsync(int id);
        Task<List<SuatChieu>> LayTatCaAsync();
        Task                  TaoSuatChieuAsync(SuatChieu suatChieu);
        Task                  CapNhatAsync(SuatChieu suatChieu);
        Task                  XoaAsync(int id);
        Task<bool>            TonTaiAsync(int id);
        Task<bool>            KiemTraTrungLichAsync(int phongChieuId, DateTime batDau, DateTime ketThuc, int? suatChieuIdExclude = null);
    }
}
