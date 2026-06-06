using System.ComponentModel.DataAnnotations;

namespace MVCDatVePhim.Models
{
    public enum LoaiGhe
    {
        [Display(Name = "Thường")] Thuong,
        [Display(Name = "VIP")]    VIP,
        [Display(Name = "Đôi")]    Doi
    }

    public class Ghe
    {
        public int Id { get; set; }

        public int PhongChieuId { get; set; }

        [Display(Name = "Hàng Ghế")]
        public char HangGhe { get; set; } // A, B, C ... J

        [Display(Name = "Số Ghế")]
        public int SoGhe { get; set; } // 1 .. 10

        [Display(Name = "Loại Ghế")]
        public LoaiGhe LoaiGhe { get; set; } = LoaiGhe.Thuong;

        // Navigation
        public PhongChieu PhongChieu { get; set; } = null!;
        public ICollection<ChiTietDatVe> ChiTietDatVes { get; set; } = new List<ChiTietDatVe>();

        // Computed label, e.g. "A1", "B5"
        public string NhanGhe => $"{HangGhe}{SoGhe}";
    }
}
