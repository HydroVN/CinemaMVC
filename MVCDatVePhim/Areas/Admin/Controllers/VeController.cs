using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCDatVePhim.Services;

namespace MVCDatVePhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VeController : Controller
    {
        private readonly IDatVeService _datVeService;

        public VeController(IDatVeService datVeService) => _datVeService = datVeService;

        public async Task<IActionResult> Index()
        {
            var ves = await _datVeService.LayTatCaAsync();
            return View(ves);
        }

        public async Task<IActionResult> ChiTiet(int id)
        {
            var ve = await _datVeService.LayTheoIdAsync(id);
            if (ve == null) return NotFound();
            return View(ve);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatTrangThai(int id, MVCDatVePhim.Models.TrangThaiDatVe trangThai)
        {
            var thanhCong = await _datVeService.CapNhatTrangThaiAsync(id, trangThai);
            if (!thanhCong) return NotFound();

            TempData["ThongBao"] = "Cập nhật trạng thái vé thành công.";
            return RedirectToAction(nameof(ChiTiet), new { id = id });
        }
    }
}
