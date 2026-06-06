using Microsoft.AspNetCore.Mvc;
using MVCDatVePhim.Services;

namespace MVCDatVePhim.Controllers
{
    public class SuatChieuController : Controller
    {
        private readonly ISuatChieuService _suatChieuService;
        private readonly IPhimService      _phimService;

        public SuatChieuController(ISuatChieuService suatChieuService, IPhimService phimService)
        {
            _suatChieuService = suatChieuService;
            _phimService      = phimService;
        }

        // GET /SuatChieu/TheoBo/5?ngay=2026-06-06
        public async Task<IActionResult> TheoBo(int id, DateTime? ngay)
        {
            var phim = await _phimService.LayTheoIdAsync(id);
            if (phim == null) return NotFound();

            var ngayChon   = ngay ?? DateTime.Today;
            var suatChieus = await _suatChieuService.LayTheoPhimAsync(id, ngayChon);

            ViewBag.Phim    = phim;
            ViewBag.NgayChon = ngayChon;
            return View(suatChieus);
        }
    }
}
