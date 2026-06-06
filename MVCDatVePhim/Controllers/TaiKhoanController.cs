using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCDatVePhim.Models;
using MVCDatVePhim.ViewModels;

namespace MVCDatVePhim.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser>   _userManager;

        public TaiKhoanController(SignInManager<ApplicationUser> signIn, UserManager<ApplicationUser> um)
        {
            _signInManager = signIn;
            _userManager   = um;
        }

        // GET /TaiKhoan/DangNhap
        public IActionResult DangNhap(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST /TaiKhoan/DangNhap
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangNhap(DangNhapViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _signInManager.PasswordSignInAsync(
                vm.Email, vm.MatKhau, vm.GhiNho, lockoutOnFailure: false);

            if (result.Succeeded)
                return LocalRedirect(vm.ReturnUrl ?? "/");

            ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng.");
            return View(vm);
        }

        // GET /TaiKhoan/DangKy
        public IActionResult DangKy()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            return View();
        }

        // POST /TaiKhoan/DangKy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKy(DangKyViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var user = new ApplicationUser
            {
                UserName    = vm.Email,
                Email       = vm.Email,
                HoTen       = vm.HoTen,
                PhoneNumber = vm.SoDienThoai
            };

            var result = await _userManager.CreateAsync(user, vm.MatKhau);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["ThongBao"] = $"Chào mừng {user.HoTen} đến với CineViet!";
                return RedirectToAction("Index", "Home");
            }

            foreach (var err in result.Errors)
                ModelState.AddModelError(string.Empty, err.Description);

            return View(vm);
        }

        // POST /TaiKhoan/DangXuat
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangXuat()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET /TaiKhoan/KhongCoQuyen
        public IActionResult KhongCoQuyen()
        {
            return View();
        }
    }
}
