using System.ComponentModel.DataAnnotations;
using MVCDatVePhim.Models;

namespace MVCDatVePhim.ViewModels.Admin
{
    public class AdminPhimFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên phim.")]
        [Display(Name = "Tên Phim")]
        public string TenPhim { get; set; } = string.Empty;

        [Display(Name = "Mô Tả")]
        public string MoTa { get; set; } = string.Empty;

        [Display(Name = "Thể Loại")]
        public TheLoaiPhim TheLoai { get; set; }

        [Range(1, 600)]
        [Display(Name = "Thời Lượng (phút)")]
        public int ThoiLuong { get; set; }

        [Range(0, 10)]
        [Display(Name = "Đánh Giá (0–10)")]
        public double DanhGia { get; set; }

        [Display(Name = "URL Ảnh Poster")]
        public string UrlAnh { get; set; } = string.Empty;

        [Display(Name = "URL Trailer")]
        public string? UrlTrailer { get; set; }

        [Display(Name = "Ngày Khởi Chiếu")]
        public DateTime NgayKhoiChieu { get; set; } = DateTime.Today;

        [Display(Name = "Đang Chiếu")]
        public bool DangChieu { get; set; } = true;

        [Display(Name = "Đạo Diễn")]
        public string DaoDien { get; set; } = string.Empty;

        [Display(Name = "Diễn Viên")]
        public string DienVien { get; set; } = string.Empty;

        [Display(Name = "Ngôn Ngữ")]
        public string NgonNgu { get; set; } = "Tiếng Việt";
    }
}
