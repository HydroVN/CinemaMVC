using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCDatVePhim.Data;
using MVCDatVePhim.Models;
using MVCDatVePhim.Services;
using MVCDatVePhim.ViewModels.Admin;
using Microsoft.EntityFrameworkCore;

namespace MVCDatVePhim.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IDatVeService             _datVeService;
        private readonly IPhimService              _phimService;
        private readonly ISuatChieuService         _suatChieuService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext      _db;

        public DashboardController(
            IDatVeService dv,
            IPhimService phim,
            ISuatChieuService sc,
            UserManager<ApplicationUser> um,
            ApplicationDbContext db)
        {
            _datVeService     = dv;
            _phimService      = phim;
            _suatChieuService = sc;
            _userManager      = um;
            _db               = db;
        }

        public async Task<IActionResult> Index()
        {
            var suatChieuHomNay = await _db.SuatChieus
                .CountAsync(s => s.ThoiGianBatDau.Date == DateTime.Today && s.DangHoatDong);

            var vm = new AdminDashboardViewModel
            {
                TongPhim         = (await _phimService.LayTatCaAsync()).Count,
                TongNguoiDung    = _userManager.Users.Count(),
                VeHomNay         = await _datVeService.TongVeHomNayAsync(),
                DoanhThuThangNay = await _datVeService.DoanhThuThangNayAsync(),
                SuatChieuHomNay  = suatChieuHomNay,
                VeGanDay         = (await _datVeService.LayTatCaAsync()).Take(8).ToList()
            };
            return View(vm);
        }
    }
}
