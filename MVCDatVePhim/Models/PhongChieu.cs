using System.ComponentModel.DataAnnotations;

namespace MVCDatVePhim.Models
{
    public class PhongChieu
    {
        public int Id { get; set; }

        public int RapChieuId { get; set; }

        [Required(ErrorMessage = "Tên phòng không được để trống.")]
        [Display(Name = "Tên Phòng")]
        public string TenPhong { get; set; } = string.Empty;

        [Range(1, 26, ErrorMessage = "Số hàng phải từ 1 đến 26.")]
        [Display(Name = "Số Hàng")]
        public int SoHang { get; set; } = 8;

        [Range(1, 20, ErrorMessage = "Số cột phải từ 1 đến 20.")]
        [Display(Name = "Số Cột")]
        public int SoCot { get; set; } = 10;

        // Navigation
        public RapChieu? RapChieu { get; set; }
        public ICollection<Ghe> DanhSachGhe { get; set; } = new List<Ghe>();
        public ICollection<SuatChieu> DanhSachSuatChieu { get; set; } = new List<SuatChieu>();

        // Computed
        public int TongSoGhe => SoHang * SoCot;
    }
}
