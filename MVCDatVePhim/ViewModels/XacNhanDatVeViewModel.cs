using MVCDatVePhim.Models;

namespace MVCDatVePhim.ViewModels
{
    public class XacNhanDatVeViewModel
    {
        public SuatChieu  SuatChieu   { get; set; } = null!;
        public List<Ghe>  GheDaChon   { get; set; } = new();
        public decimal    TongTien    { get; set; }
        public List<int>  GheIds      { get; set; } = new();
    }
}
