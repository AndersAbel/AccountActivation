using AccountActivation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace AccountActivation.Controllers
{
    // Let any authorized user create another user in this example.
    [Authorize]
    public class UserController : Controller
    {
        ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateUserModel model)
        { 
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };
                var result = await UserManager.CreateAsync(user); // Create without password.
                if(result.Succeeded)
                {
                    await SendActivationMail(user);
                    return RedirectToAction("CreateConfirmation");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return View(model);
        }

        private async Task SendActivationMail(ApplicationUser user)
        {
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

            // Using protocol param will force creation of an absolut url. We
            // don't want to send a relative URL by e-mail.
            var callbackUrl = Url.Action(
                "ResetPassword", 
                "Account", 
                new { userId = user.Id, code = code}, 
                protocol: Request.Url.Scheme);

            string body = @"<h4>Welcome to my system!</h4>
<p>To get started, please <a href=""" + callbackUrl + @""">activate</a> your account.</p>
<p>The account must be activated within 24 hours from receving this mail.</p>";
            
            await UserManager.SendEmailAsync(user.Id, "Welcome to my system!", body);
        }

        public ActionResult CreateConfirmation()
        {
            return View();
        }
    }
}