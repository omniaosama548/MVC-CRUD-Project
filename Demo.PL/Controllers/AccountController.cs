using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}
        //Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var User = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    FName = model.FName,
                    LName = model.LName,
                    IsAgree = model.IsAgree,
                };
                var Result=await _userManager.CreateAsync(User,model.Password );
                if (Result.Succeeded) { 
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }
        //Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var User=await _userManager.FindByEmailAsync(model.Email);
                if(User is not null)
                {
                   var Flag= await  _userManager.CheckPasswordAsync(User, model.Password);
                    if (Flag)
                    {
                        var Res=await _signInManager.PasswordSignInAsync(User,model.Password,model.RemeberMe,false);
                        if (Res.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                       
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Incorrect Password");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email is Not Exist ");
                }
            }
            return View(model);
        }
        //Sign Out
        public async Task<IActionResult> SIGNOUT()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        //Forget Passwprd
        public IActionResult ForgetPassword()
        {
            return View();
        }
        //send email
        [HttpPost]
        public async  Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User=await _userManager.FindByEmailAsync(model.Email); 
                if (User is not null)
                {
                    var token=await _userManager.GeneratePasswordResetTokenAsync(User);
                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { Email = User.Email, Token = token }, Request.Scheme);
                    var email = new Email()
                    {
                        Subject = "Reset Password",
                        To = model.Email,
                        Body = ResetPasswordLink
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email is not exist");
					return View("ForgetPassword", model);
				}
            }
            else
            {
                return View("ForgetPassword", model);
            }
        }
		//CheckYourInbox
        public IActionResult CheckYourInbox()
        {
            return View();  
        }
		//Reset Password
        public IActionResult ResetPassword(string email,string token)
        {
            TempData["email"]=email;
            TempData["token"]=token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email=(string)TempData["email"];
                string token=(string)TempData["token"];
                var user=await _userManager.FindByEmailAsync(email);
                var Res=await _userManager.ResetPasswordAsync(user,token,model.NewPassword);
                if (Res.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach (var item in Res.Errors) 
                        ModelState.AddModelError(string.Empty,item.Description);
                }
            }
            return View(model);
        }
	}
}
