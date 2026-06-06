using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCDatVePhim.Models;
using MVCDatVePhim.Services;
using MVCDatVePhim.ViewModels.Admin;

namespace MVCDatVePhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PhimController : Controller
    {
        private readonly IPhimService _phimService;

        public PhimController(IPhimService phimService) => _phimService = phimService;

        public async Task<IActionResult> Index()
        {
            var phims = await _phimService.LayTatCaAsync();
            return View(phims);
        }

        public IActionResult TaoMoi() => View(new AdminPhimFormViewModel { NgayKhoiChieu = DateTime.Today });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaoMoi(AdminPhimFormViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var phim = MapToPhim(vm);
            await _phimService.TaoPhimAsync(phim);
            TempData["ThongBao"] = $"Đã thêm phim \"{phim.TenPhim}\" thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChinhSua(int id)
        {
            var phim = await _phimService.LayTheoIdAsync(id);
            if (phim == null) return NotFound();
            return View(MapToViewModel(phim));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChinhSua(AdminPhimFormViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var phim = MapToPhim(vm);
            phim.Id  = vm.Id;
            await _phimService.CapNhatPhimAsync(phim);
            TempData["ThongBao"] = "Cập nhật phim thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Xoa(int id)
        {
            await _phimService.XoaPhimAsync(id);
            TempData["ThongBao"] = "Đã xoá phim thành công.";
            return RedirectToAction(nameof(Index));
        }

        // ── Helpers ───────────────────────────────────────────────
        private static Phim MapToPhim(AdminPhimFormViewModel vm) => new()
        {
            TenPhim       = vm.TenPhim,
            MoTa          = vm.MoTa,
            TheLoai       = vm.TheLoai,
            ThoiLuong     = vm.ThoiLuong,
            DanhGia       = vm.DanhGia,
            UrlAnh        = vm.UrlAnh,
            UrlTrailer    = vm.UrlTrailer,
            NgayKhoiChieu = vm.NgayKhoiChieu,
            DangChieu     = vm.DangChieu,
            DaoDien       = vm.DaoDien,
            DienVien      = vm.DienVien,
            NgonNgu       = vm.NgonNgu
        };

        private static AdminPhimFormViewModel MapToViewModel(Phim p) => new()
        {
            Id            = p.Id,
            TenPhim       = p.TenPhim,
            MoTa          = p.MoTa,
            TheLoai       = p.TheLoai,
            ThoiLuong     = p.ThoiLuong,
            DanhGia       = p.DanhGia,
            UrlAnh        = p.UrlAnh,
            UrlTrailer    = p.UrlTrailer,
            NgayKhoiChieu = p.NgayKhoiChieu,
            DangChieu     = p.DangChieu,
            DaoDien       = p.DaoDien,
            DienVien      = p.DienVien,
            NgonNgu       = p.NgonNgu
        };
    }
}
