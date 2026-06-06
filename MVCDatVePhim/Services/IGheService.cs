using MVCDatVePhim.Models;

namespace MVCDatVePhim.Services
{
    public interface IGheService
    {
        Task<List<Ghe>>        LayTheoPhongAsync(int phongChieuId);
        Task<List<int>>        LayGheDaDatAsync(int suatChieuId);
        Task<bool>             GheTrongAsync(int gheId, int suatChieuId);
        Task<List<Ghe>>        LayTheoIdsAsync(List<int> gheIds);
    }
}
