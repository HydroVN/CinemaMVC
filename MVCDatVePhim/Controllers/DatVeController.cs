using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCDatVePhim.Services;
using MVCDatVePhim.ViewModels;
using System.Security.Claims;

namespace MVCDatVePhim.Controllers
{
    [Authorize]
    public class DatVeController : Controller
    {
        private readonly ISuatChieuService _suatChieuService;
        private readonly IGheService       _gheService;
        private readonly IDatVeService     _datVeService;

        public DatVeController(ISuatChieuService sc, IGheService ghe, IDatVeService dv)
        {
            _suatChieuService = sc;
            _gheService       = ghe;
            _datVeService     = dv;
        }

        // GET /DatVe/ChonGhe/5
        public async Task<IActionResult> ChonGhe(int id)
        {
            var suatChieu = await _suatChieuService.LayTheoIdAsync(id);
            if (suatChieu == null) return NotFound();

            var ghes      = await _gheService.LayTheoPhongAsync(suatChieu.PhongChieuId);
            var gheDaDat  = await _gheService.LayGheDaDatAsync(id);

            var vm = new ChonGheViewModel
            {
                SuatChieu   = suatChieu,
                DanhSachGhe = ghes,
                GheDaDat    = gheDaDat,
                SoHang      = suatChieu.PhongChieu.SoHang,
                SoCot       = suatChieu.PhongChieu.SoCot
            };
            return View(vm);
        }

        // POST /DatVe/XacNhan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XacNhan(int suatChieuId, List<int> gheIds)
        {
            if (!gheIds.Any())
            {
                TempData["LoiGhe"] = "Vui lòng chọn ít nhất một ghế.";
                return RedirectToAction(nameof(ChonGhe), new { id = suatChieuId });
            }

            var suatChieu = await _suatChieuService.LayTheoIdAsync(suatChieuId);
            if (suatChieu == null) return NotFound();

            var ghes     = await _gheService.LayTheoIdsAsync(gheIds);
            var tongTien = ghes.Sum(g => g.LoaiGhe == Models.LoaiGhe.VIP
                                        ? suatChieu.GiaVe * 1.5m
                                        : suatChieu.GiaVe);

            var vm = new XacNhanDatVeViewModel
            {
                SuatChieu = suatChieu,
                GheDaChon = ghes,
                TongTien  = tongTien,
                GheIds    = gheIds
            };
            return View(vm);
        }

        // POST /DatVe/ThanhToan
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThanhToan(int suatChieuId, List<int> gheIds)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var datVe  = await _datVeService.TaoAsync(userId, suatChieuId, gheIds);

            if (datVe == null)
            {
                TempData["LoiGhe"] = "Một số ghế bạn chọn vừa được người khác đặt. Vui lòng chọn lại.";
                return RedirectToAction(nameof(ChonGhe), new { id = suatChieuId });
            }

            return RedirectToAction(nameof(ThanhCong), new { id = datVe.Id });
        }

        // GET /DatVe/ThanhCong/5
        public async Task<IActionResult> ThanhCong(int id)
        {
            var datVe = await _datVeService.LayTheoIdAsync(id);
            if (datVe == null) return NotFound();
            return View(datVe);
        }

        // GET /DatVe/VeCuaToi
        public async Task<IActionResult> VeCuaToi()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var veList = await _datVeService.LayTheoNguoiDungAsync(userId);
            return View(veList);
        }

        // POST /DatVe/Huy/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Huy(int id)
        {
            var userId  = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var ketQua  = await _datVeService.HuyAsync(id, userId);
            TempData["ThongBao"] = ketQua
                ? "Huỷ vé thành công."
                : "Không thể huỷ vé này.";
            return RedirectToAction(nameof(VeCuaToi));
        }
    }
}
