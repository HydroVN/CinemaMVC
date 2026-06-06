using Microsoft.AspNetCore.Identity;

namespace MVCDatVePhim.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string HoTen { get; set; } = string.Empty;

        public ICollection<DatVe> DanhSachDatVe { get; set; } = new List<DatVe>();
    }
}
