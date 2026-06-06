using System.ComponentModel.DataAnnotations;

namespace MVCDatVePhim.Models
{
    public enum TrangThaiDatVe
    {
        [Display(Name = "Chờ Xác Nhận")] ChoDuyet,
        [Display(Name = "Đã Xác Nhận")]  DaXacNhan,
        [Display(Name = "Đã Huỷ")]       DaHuy
    }

    public class DatVe
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public int SuatChieuId { get; set; }

        [Display(Name = "Ngày Đặt")]
        public DateTime NgayDat { get; set; } = DateTime.Now;

        [Display(Name = "Tổng Tiền")]
        public decimal TongTien { get; set; }

        [Display(Name = "Trạng Thái")]
        public TrangThaiDatVe TrangThai { get; set; } = TrangThaiDatVe.ChoDuyet;

        [Display(Name = "Mã Vé")]
        public string MaVe { get; set; } = string.Empty;

        // Navigation
        public ApplicationUser User { get; set; } = null!;
        public SuatChieu SuatChieu { get; set; } = null!;
        public ICollection<ChiTietDatVe> ChiTietDatVes { get; set; } = new List<ChiTietDatVe>();
    }
}
