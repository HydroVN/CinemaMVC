using System.ComponentModel.DataAnnotations;

namespace MVCDatVePhim.Models
{
    public class ChiTietDatVe
    {
        public int Id { get; set; }

        public int DatVeId { get; set; }

        public int GheId { get; set; }

        [Display(Name = "Giá Vé")]
        public decimal GiaVe { get; set; }

        // Navigation
        public DatVe DatVe { get; set; } = null!;
        public Ghe Ghe { get; set; } = null!;
    }
}
