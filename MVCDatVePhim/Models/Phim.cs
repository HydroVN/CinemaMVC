using System.ComponentModel.DataAnnotations;

namespace MVCDatVePhim.Models
{
    public enum TheLoaiPhim
    {
        [Display(Name = "Hành Động")] HanhDong,
        [Display(Name = "Tình Cảm")]  TinhCam,
        [Display(Name = "Hài Hước")]  HaiHuoc,
        [Display(Name = "Kinh Dị")]   KinhDi,
        [Display(Name = "Khoa Học Viễn Tưởng")] KhoaHocVienTuong,
        [Display(Name = "Hoạt Hình")] HoatHinh,
        [Display(Name = "Tâm Lý")]    TamLy,
        [Display(Name = "Khác")]      Khac
    }

    public class Phim
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên phim không được để trống.")]
        [Display(Name = "Tên Phim")]
        public string TenPhim { get; set; } = string.Empty;

        [Display(Name = "Mô Tả")]
        public string MoTa { get; set; } = string.Empty;

        [Display(Name = "Thể Loại")]
        public TheLoaiPhim TheLoai { get; set; } = TheLoaiPhim.HanhDong;

        [Range(1, 600, ErrorMessage = "Thời lượng phải từ 1 đến 600 phút.")]
        [Display(Name = "Thời Lượng (phút)")]
        public int ThoiLuong { get; set; }

        [Range(0, 10, ErrorMessage = "Đánh giá phải từ 0 đến 10.")]
        [Display(Name = "Đánh Giá")]
        public double DanhGia { get; set; }

        [Display(Name = "Ảnh Poster")]
        public string UrlAnh { get; set; } = string.Empty;

        [Display(Name = "Link Trailer")]
        public string? UrlTrailer { get; set; }

        [Display(Name = "Ngày Khởi Chiếu")]
        public DateTime NgayKhoiChieu { get; set; }

        [Display(Name = "Đang Chiếu")]
        public bool DangChieu { get; set; } = true;

        [Display(Name = "Đạo Diễn")]
        public string DaoDien { get; set; } = string.Empty;

        [Display(Name = "Diễn Viên")]
        public string DienVien { get; set; } = string.Empty;

        [Display(Name = "Ngôn Ngữ")]
        public string NgonNgu { get; set; } = "Việt Nam";

        // Navigation
        public ICollection<SuatChieu> DanhSachSuatChieu { get; set; } = new List<SuatChieu>();
    }
}
