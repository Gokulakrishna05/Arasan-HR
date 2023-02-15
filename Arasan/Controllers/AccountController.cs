using Arasan.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Arasan.Models;
using System.Data;

namespace Arasan.Controllers
{
    public class AccountController : Controller
    {
        DataTransactions _dtransactions;
        IConfiguration? _configuratio;
        ILoginService loginService;
        private string? _connectionString;
        public AccountController(ILoginService _loginService, IConfiguration _configuratio)
        {
            loginService = _loginService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
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


        //public IActionResult Login([Bind] LoginViewModel model)
        //{
        //    int res = loginService.LoginCheck(model.Username, model.Password);
        //    if (res == 1)
        //    {
        //        TempData["msg"] = "You are welcome to Admin Section";
        //        return RedirectToAction(actionName: "Index", controllerName: "Home");
        //    }
        //    else
        //    {
        //        TempData["msg"] = "Admin id or Password is wrong.!";
        //    }
        //    return View(model);
        //}

        [HttpPost]


       // public IActionResult Login(LoginViewModel model )

        //public IActionResult Login(LoginViewModel model)
       // { 



        public IActionResult Login(LoginViewModel model )

        { 
            //bool res = loginService.LoginCheck(model.Username, model.Password);
            //if (res == true)

            _dtransactions = new DataTransactions(_connectionString);
            bool isValidUser = false;//loginService.LoginCheck(model.Username, model.Password);
            try
            {
                string _selUser = @"Select Username,password,eactive,empdept,EMPMASTID from empmast where  eactive='Yes' and  username='" + model.Username + "' and password   ='" + model.Password + "'";
                DataTable _dtUser = new DataTable();
                _dtUser = _dtransactions.GetData(_selUser);
                if (_dtUser.Rows.Count > 0)
                {
                    CookieOptions option = new CookieOptions();
                    //HttpCookie Cookie = Request.Cookies["Arasan"];
                    //if (expireTime.HasValue)
                    //    option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
                    //else
                    option.Expires = DateTime.Now.AddMonths(3);
                    Response.Cookies.Append("UserId", _dtUser.Rows[0]["EMPMASTID"].ToString(), option);
                    Response.Cookies.Append("UserName", _dtUser.Rows[0]["Username"].ToString(), option);
                    Response.Cookies.Append("Department", _dtUser.Rows[0]["empdept"].ToString(), option);

                    //HttpContext.Session.SetString("UserId", _dtUser.Rows[0]["EMPMASTID"].ToString());
                    //HttpContext.Session.SetString("UserName", _dtUser.Rows[0]["Username"].ToString());
                    //HttpContext.Session.SetString("Department", _dtUser.Rows[0]["empdept"].ToString());

                    //var userId = HttpContext.Request.Cookies["UserId"];

                    isValidUser = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            if (isValidUser == true)

            {
                //TempData["msg"] = "You are welcome to Admin Section";
                // return RedirectToAction(actionName: "Index", controllerName: "Home");
                return RedirectToAction(actionName: "BranchSelection", controllerName: "BranchSelection");
            }
            else
            {
                TempData["msg"] = "Admin id or Password is wrong.!";
            }
            return View(model);

        }

        //    [HttpPost]
        //public IActionResult Login(LoginViewModel model )

        //}

        //[HttpPost]
        //public IActionResult Login(LoginViewModel model)


        //{
        //    LoginViewModel L = new LoginViewModel();
        //    if (ModelState.IsValid)
        //    {
        //        //string strout = model.
        //    }

        //        return View(L);


        //        return View(L);

        //    return View(L);


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
        //[HttpPost]
        //public IActionResult Logout()
        //{
        //    //await _signInManager.SignOutAsync();
        //    return RedirectToAction("Login", "Account");
        //}
    }
}
