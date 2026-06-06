using MVCDatVePhim.Models;

namespace MVCDatVePhim.ViewModels.Admin
{
    public class AdminDashboardViewModel
    {
        public int     TongPhim          { get; set; }
        public int     TongNguoiDung     { get; set; }
        public int     VeHomNay          { get; set; }
        public decimal DoanhThuThangNay  { get; set; }
        public int     SuatChieuHomNay   { get; set; }
        public List<DatVe> VeGanDay      { get; set; } = new();
    }
}
