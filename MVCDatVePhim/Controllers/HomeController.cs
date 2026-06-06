using Microsoft.AspNetCore.Mvc;
using MVCDatVePhim.Models;
using MVCDatVePhim.Services;

namespace MVCDatVePhim.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPhimService _phimService;

        public HomeController(IPhimService phimService) => _phimService = phimService;

        public async Task<IActionResult> Index()
        {
            ViewBag.DangChieu = await _phimService.LayDangChieuAsync();
            ViewBag.SapChieu  = await _phimService.LaySapChieuAsync();
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
