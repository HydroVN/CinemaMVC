using Microsoft.AspNetCore.Mvc;
using MVCDatVePhim.Services;

namespace MVCDatVePhim.Controllers
{
    public class PhimController : Controller
    {
        private readonly IPhimService _phimService;

        public PhimController(IPhimService phimService) => _phimService = phimService;

        // GET /Phim  or  /Phim?tuKhoa=...&theLoai=...&ngayChieu=...
        public async Task<IActionResult> Index(string? tuKhoa, string? theLoai, DateTime? ngayChieu)
        {
            var phims = await _phimService.TimKiemAsync(tuKhoa, theLoai, ngayChieu);
            ViewBag.TuKhoa   = tuKhoa;
            ViewBag.TheLoai  = theLoai;
            ViewBag.NgayChieu = ngayChieu?.ToString("yyyy-MM-dd");
            return View(phims);
        }

        // GET /Phim/ChiTiet/5
        public async Task<IActionResult> ChiTiet(int id)
        {
            var phim = await _phimService.LayTheoIdAsync(id);
            if (phim == null) return NotFound();
            return View(phim);
        }
    }
}
