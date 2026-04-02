using Arasan.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Arasan.Models;
using System.Data;
using System.Security.Cryptography;
using System.Text;
namespace Arasan.Controllers
{
    public class AccountController : Controller
    {
        DataTransactions _dtransactions;
        IConfiguration? _configuratio;
        ILoginService loginService;
        private string? _connectionString;
        private static readonly int KeySize = 256; // You can choose 128, 192, or 256 bits
        private static readonly int BlockSize = 128; // Block size for AES
        private static readonly string EncryptionKey = "Arasan"; // Use a strong key
        private static readonly string IV = "TaaiErp"; // Use a strong IV

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
            Response.Cookies.Delete("UserName");
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



        public IActionResult Login(LoginViewModel model)

        {
            //bool res = loginService.LoginCheck(model.Username, model.Password);
            //if (res == true)


            _dtransactions = new DataTransactions(_connectionString);
            bool isValidUser = false;//loginService.LoginCheck(model.Username, model.Password);
            try
            {
                string _selUser = "";
                DataTable emppp = _dtransactions.GetData("Select Username,password,eactive,empdept,EMPMASTID,IMGPATH from empmast where  eactive='Yes' and  username='" + model.Username + "' and password   ='" + model.Password + "'");
                if (emppp.Rows.Count > 0)
                {
                    _selUser = @"Select m.Username,m.password,m.eactive,d.DEPTNAME,m.EMPMASTID,m.IMGPATH,m.empdesign from empmast m,DDBASIC d where m.empdept=d.ddbasicid and  m.eactive='Yes' and  m.username='" + model.Username + "' and password   ='" + model.Password + "'";

                }
                else
                {
                    string encpass = Encrypt(model.Password);
                    _selUser = @"Select m.Username,m.password,m.eactive,d.DEPTNAME,m.EMPMASTID,m.IMGPATH,m.empdesign from empmast m,DDBASIC d where m.empdept=d.ddbasicid and  m.eactive='Yes' and  m.username='" + model.Username + "' and password   ='" + encpass + "'";

                }

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
                    Response.Cookies.Append("Department", _dtUser.Rows[0]["DEPTNAME"].ToString(), option);
                    Response.Cookies.Append("ImgPath", _dtUser.Rows[0]["IMGPATH"].ToString(), option);
                    Response.Cookies.Append("empdesign", _dtUser.Rows[0]["empdesign"].ToString(), option);

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
        public static string Encrypt(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = GetKey(EncryptionKey);  // Ensure correct key size
                aes.IV = GetIV(IV);               // Ensure IV is 16 bytes

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                    }
                    // Return Base64-encoded string
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
        private static byte[] GetKey(string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // If the key is too long, truncate it
            if (keyBytes.Length > 32)
            {
                Array.Resize(ref keyBytes, 32); // For AES-256
            }
            // If the key is too short, pad it with zeros
            else if (keyBytes.Length < 32)
            {
                Array.Resize(ref keyBytes, 32); // For AES-256
            }

            return keyBytes;
        }
        // Function to adjust the IV to 16 bytes (128-bit block size for AES)
        private static byte[] GetIV(string iv)
        {
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);

            // If the IV is too long, truncate it
            if (ivBytes.Length > 16)
            {
                Array.Resize(ref ivBytes, 16); // IV must be 16 bytes for AES
            }
            // If the IV is too short, pad it with zeros
            else if (ivBytes.Length < 16)
            {
                Array.Resize(ref ivBytes, 16); // IV must be 16 bytes for AES
            }

            return ivBytes;
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
