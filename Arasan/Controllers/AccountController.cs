using Arasan.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Arasan.Models;

namespace Arasan.Controllers
{
    public class AccountController : Controller
    {

        ILoginService loginService;
        public AccountController(ILoginService _loginService)
        {
            loginService = _loginService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = "")
        {
           // var model = new LoginViewModel { ReturnUrl = returnUrl };
            ViewBag.ReturnUrl = returnUrl;
            LoginViewModel L = new LoginViewModel();
            return View(L);
        }


        public IActionResult Login([Bind] LoginViewModel model)
        {
            int res = loginService.LoginCheck(model.Username, model.Password);
            if (res == 1)
            {
                TempData["msg"] = "You are welcome to Admin Section";
                return RedirectToAction(actionName: "Index", controllerName: "Home");
            }
            else
            {
                TempData["msg"] = "Admin id or Password is wrong.!";
            }
            return View(model);
            }

        //    [HttpPost]
        //public IActionResult Login(LoginViewModel model )
        //{
        //    LoginViewModel L = new LoginViewModel();
        //    if (ModelState.IsValid)
        //    {
        //        //string strout = model.
        //    }
        //        return View(L);
        //}

        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _signInManager.PasswordSignInAsync(model.Username,
        //           model.Password, model.RememberMe, false);

        //        if (result.Succeeded)
        //        {
        //            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
        //            {
        //                return Redirect(model.ReturnUrl);
        //            }
        //            else
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }
        //        }
        //    }
        //    ModelState.AddModelError("", "Invalid login attempt");
        //    return View(model);
        //}
        [HttpPost]
        public IActionResult Logout()
        {
            //await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
