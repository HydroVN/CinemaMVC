using MVCDatVePhim.Models;

namespace MVCDatVePhim.ViewModels
{
    public class ChonGheViewModel
    {
        public SuatChieu   SuatChieu     { get; set; } = null!;
        public List<Ghe>   DanhSachGhe   { get; set; } = new();
        public List<int>   GheDaDat      { get; set; } = new(); // booked seat IDs
        public int         SoHang        { get; set; }
        public int         SoCot         { get; set; }
    }
}
