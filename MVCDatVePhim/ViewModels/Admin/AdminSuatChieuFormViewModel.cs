using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCDatVePhim.ViewModels.Admin
{
    public class AdminSuatChieuFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Phim")]
        public int PhimId { get; set; }

        [Required]
        [Display(Name = "Phòng Chiếu")]
        public int PhongChieuId { get; set; }

        [Required]
        [Display(Name = "Thời Gian Bắt Đầu")]
        public DateTime ThoiGianBatDau { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Thời Gian Kết Thúc")]
        public DateTime ThoiGianKetThuc { get; set; } = DateTime.Now.AddHours(2);

        [Required]
        [Range(1, 10000000)]
        [Display(Name = "Giá Vé (VNĐ)")]
        public decimal GiaVe { get; set; }

        [Display(Name = "Đang Hoạt Động")]
        public bool DangHoatDong { get; set; } = true;

        // Dropdowns
        public IEnumerable<SelectListItem> DanhSachPhim    { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> DanhSachPhong   { get; set; } = new List<SelectListItem>();
    }
}
