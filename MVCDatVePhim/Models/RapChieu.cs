using System.ComponentModel.DataAnnotations;

namespace MVCDatVePhim.Models
{
    public class RapChieu
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên rạp không được để trống.")]
        [Display(Name = "Tên Rạp")]
        public string TenRap { get; set; } = string.Empty;

        [Display(Name = "Địa Chỉ")]
        public string DiaChi { get; set; } = string.Empty;

        [Display(Name = "Thành Phố")]
        public string ThanhPho { get; set; } = string.Empty;

        [Display(Name = "Số Điện Thoại")]
        public string? SoDienThoai { get; set; }

        // Navigation
        public ICollection<PhongChieu> DanhSachPhong { get; set; } = new List<PhongChieu>();
    }
}
