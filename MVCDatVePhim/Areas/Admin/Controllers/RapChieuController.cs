using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCDatVePhim.Data;
using MVCDatVePhim.Models;
using Microsoft.EntityFrameworkCore;

namespace MVCDatVePhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RapChieuController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RapChieuController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var raps = await _db.RapChieus.Include(r => r.DanhSachPhong).ToListAsync();
            return View(raps);
        }

        public IActionResult TaoMoi() => View(new RapChieu());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TaoMoi(RapChieu rap)
        {
            if (!ModelState.IsValid) return View(rap);
            _db.RapChieus.Add(rap);
            await _db.SaveChangesAsync();
            TempData["ThongBao"] = "Đã thêm rạp chiếu thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChinhSua(int id)
        {
            var rap = await _db.RapChieus.FindAsync(id);
            if (rap == null) return NotFound();
            return View(rap);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChinhSua(RapChieu rap)
        {
            if (!ModelState.IsValid) return View(rap);
            _db.RapChieus.Update(rap);
            await _db.SaveChangesAsync();
            TempData["ThongBao"] = "Cập nhật rạp chiếu thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Xoa(int id)
        {
            var rap = await _db.RapChieus.FindAsync(id);
            if (rap != null) { _db.RapChieus.Remove(rap); await _db.SaveChangesAsync(); }
            TempData["ThongBao"] = "Đã xoá rạp chiếu.";
            return RedirectToAction(nameof(Index));
        }

        // ── Phòng chiếu ───────────────────────────────────────────
        public async Task<IActionResult> PhongChieu(int rapId)
        {
            var rap   = await _db.RapChieus.Include(r => r.DanhSachPhong).FirstOrDefaultAsync(r => r.Id == rapId);
            if (rap == null) return NotFound();
            ViewBag.Rap = rap;
            return View(rap.DanhSachPhong.ToList());
        }

        public IActionResult ThemPhong(int rapId)
        {
            ViewBag.RapId = rapId;
            return View(new PhongChieu { RapChieuId = rapId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemPhong(PhongChieu phong)
        {
            if (!ModelState.IsValid) { ViewBag.RapId = phong.RapChieuId; return View(phong); }

            _db.PhongChieus.Add(phong);
            await _db.SaveChangesAsync();

            // Auto-generate seats for this room
            for (int h = 0; h < phong.SoHang; h++)
            {
                char hang = (char)('A' + h);
                for (int col = 1; col <= phong.SoCot; col++)
                {
                    _db.Ghes.Add(new Ghe
                    {
                        PhongChieuId = phong.Id,
                        HangGhe      = hang,
                        SoGhe        = col,
                        LoaiGhe      = h >= phong.SoHang - 2 ? LoaiGhe.VIP : LoaiGhe.Thuong
                    });
                }
            }
            await _db.SaveChangesAsync();

            TempData["ThongBao"] = $"Đã thêm phòng \"{phong.TenPhong}\" với {phong.SoHang * phong.SoCot} ghế.";
            return RedirectToAction(nameof(PhongChieu), new { rapId = phong.RapChieuId });
        }
    }
}
