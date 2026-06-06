using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVCDatVePhim.Data;
using MVCDatVePhim.Models;
using MVCDatVePhim.Services;
using MVCDatVePhim.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;

namespace MVCDatVePhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SuatChieuController : Controller
    {
        private readonly ISuatChieuService _suatChieuService;
        private readonly ApplicationDbContext _db;

        public SuatChieuController(ISuatChieuService sc, ApplicationDbContext db)
        {
            _suatChieuService = sc;
            _db               = db;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _suatChieuService.LayTatCaAsync();
            return View(list);
        }

        public async Task<IActionResult> TaoMoi()
        {
            var vm = new AdminSuatChieuFormViewModel
            {
                ThoiGianBatDau  = DateTime.Now.AddHours(1),
                ThoiGianKetThuc = DateTime.Now.AddHours(3),
                DanhSachPhim    = await GetPhimSelectList(),
                DanhSachPhong   = await GetPhongSelectList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaoMoi(AdminSuatChieuFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.DanhSachPhim  = await GetPhimSelectList();
                vm.DanhSachPhong = await GetPhongSelectList();
                return View(vm);
            }

            // Check overlap
            if (await _suatChieuService.KiemTraTrungLichAsync(vm.PhongChieuId, vm.ThoiGianBatDau, vm.ThoiGianKetThuc))
            {
                ModelState.AddModelError("", "Lỗi: Phòng chiếu đã có lịch chiếu khác hoạt động trong khoảng thời gian này!");
                vm.DanhSachPhim  = await GetPhimSelectList();
                vm.DanhSachPhong = await GetPhongSelectList();
                return View(vm);
            }

            var sc = new SuatChieu
            {
                PhimId          = vm.PhimId,
                PhongChieuId    = vm.PhongChieuId,
                ThoiGianBatDau  = vm.ThoiGianBatDau,
                ThoiGianKetThuc = vm.ThoiGianKetThuc,
                GiaVe           = vm.GiaVe,
                DangHoatDong    = vm.DangHoatDong
            };
            await _suatChieuService.TaoSuatChieuAsync(sc);
            TempData["ThongBao"] = "Đã thêm suất chiếu thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChinhSua(int id)
        {
            var sc = await _suatChieuService.LayTheoIdAsync(id);
            if (sc == null) return NotFound();

            return View(new AdminSuatChieuFormViewModel
            {
                Id              = sc.Id,
                PhimId          = sc.PhimId,
                PhongChieuId    = sc.PhongChieuId,
                ThoiGianBatDau  = sc.ThoiGianBatDau,
                ThoiGianKetThuc = sc.ThoiGianKetThuc,
                GiaVe           = sc.GiaVe,
                DangHoatDong    = sc.DangHoatDong,
                DanhSachPhim    = await GetPhimSelectList(),
                DanhSachPhong   = await GetPhongSelectList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChinhSua(AdminSuatChieuFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.DanhSachPhim  = await GetPhimSelectList();
                vm.DanhSachPhong = await GetPhongSelectList();
                return View(vm);
            }

            // Check overlap (excluding the current showtime ID)
            if (await _suatChieuService.KiemTraTrungLichAsync(vm.PhongChieuId, vm.ThoiGianBatDau, vm.ThoiGianKetThuc, vm.Id))
            {
                ModelState.AddModelError("", "Lỗi: Phòng chiếu đã có lịch chiếu khác hoạt động trong khoảng thời gian này!");
                vm.DanhSachPhim  = await GetPhimSelectList();
                vm.DanhSachPhong = await GetPhongSelectList();
                return View(vm);
            }

            var sc = new SuatChieu
            {
                Id              = vm.Id,
                PhimId          = vm.PhimId,
                PhongChieuId    = vm.PhongChieuId,
                ThoiGianBatDau  = vm.ThoiGianBatDau,
                ThoiGianKetThuc = vm.ThoiGianKetThuc,
                GiaVe           = vm.GiaVe,
                DangHoatDong    = vm.DangHoatDong
            };
            await _suatChieuService.CapNhatAsync(sc);
            TempData["ThongBao"] = "Cập nhật suất chiếu thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Xoa(int id)
        {
            await _suatChieuService.XoaAsync(id);
            TempData["ThongBao"] = "Đã xoá suất chiếu.";
            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetPhimSelectList() =>
            (await _db.Phims.OrderBy(p => p.TenPhim).ToListAsync())
             .Select(p => new SelectListItem(p.TenPhim, p.Id.ToString()));

        private async Task<IEnumerable<SelectListItem>> GetPhongSelectList() =>
            (await _db.PhongChieus.Include(pc => pc.RapChieu).ToListAsync())
             .Select(pc => new SelectListItem($"{(pc.RapChieu?.TenRap ?? "N/A")} — {pc.TenPhong}", pc.Id.ToString()));
    }
}
