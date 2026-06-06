using System.ComponentModel.DataAnnotations;

namespace MVCDatVePhim.Models
{
    public class SuatChieu
    {
        public int Id { get; set; }

        public int PhimId { get; set; }

        public int PhongChieuId { get; set; }

        [Display(Name = "Thời Gian Bắt Đầu")]
        public DateTime ThoiGianBatDau { get; set; }

        [Display(Name = "Thời Gian Kết Thúc")]
        public DateTime ThoiGianKetThuc { get; set; }

        [Display(Name = "Giá Vé (VNĐ)")]
        public decimal GiaVe { get; set; }

        [Display(Name = "Đang Hoạt Động")]
        public bool DangHoatDong { get; set; } = true;

        // Navigation
        public Phim Phim { get; set; } = null!;
        public PhongChieu PhongChieu { get; set; } = null!;
        public ICollection<DatVe> DanhSachDatVe { get; set; } = new List<DatVe>();

        // Computed
        public string ThoiGianHienThi =>
            ThoiGianBatDau.ToString("HH:mm") + " - " + ThoiGianKetThuc.ToString("HH:mm");
        public string NgayHienThi =>
            ThoiGianBatDau.ToString("dd/MM/yyyy");
    }
}
