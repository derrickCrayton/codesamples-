using Sabio.Web.Services;
using Sabio.Web.Domain;
using Sabio.Web.Models.Responses;
using Sabio.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Host.SystemWeb;
using System.Web.Mvc;
using System.IO;
using Sabio.Web.Models.Requests.Uploads;
using Sabio.Web.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Sabio.Web.Controllers;
using Microsoft.Practices.Unity;
using Sabio.Web.Services.Interfaces;

namespace Sabio.Web.Controllers
{

	[RoutePrefix("public")]
	[Authorize]
	public class PublicController : BaseController
	{


		//....// ===================================== DEPENDENCY INJECTION START ===============================================

		//Used in BaseController
		//[Dependency]
		//public IUserProfileService _UserProfileService { get; set; }
		[Dependency]
		public ITokenService _TokenService { get; set; }


		//....// ===================================== DEPENDENCY INJECTION START ===============================================




		// sign up with email
		[Route("emailsignup")]
		[AllowAnonymous]
		public ActionResult EmailSignup()
		{
			return View();
		}



		// Second attempt at user registration
		[Route("registration")]
		[AllowAnonymous]
		public ActionResult Registration()
		{
			var model = new RegisterViewModel();
			return View(model);
		}


		// POST Registration
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterViewModel model)
		{


			if (ModelState.IsValid)
			{
				var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

				var userManager = System.Web
				   .HttpContext.Current.GetOwinContext()
				   .GetUserManager<ApplicationUserManager>();



				var result = await userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					var AC = new AccountController();
					await AC.SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

					// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771   
					// Send an email with this link   
					// string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.I;d);   
					// var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.I;d, code = code }, protocol: Request.Url.Scheme);   
					// await UserManager.SendEmailAsync(user.I;d, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");   
					//Assign Role to user Here      
					await userManager.AddToRoleAsync(user.Id, "Company Owner");
					//Ends Here    
					return RedirectToAction("Index", "Users");
				}
			}

			// If we got this far, something failed, redisplay form   
			return View(model);
		}



//         if (ModelState.IsValid)
//        {
//            var user = new ApplicationUser() { UserName = model.UserName };

//        RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
//            IdentityRole role = new IdentityRole("Admin");
//        await RoleManager.CreateAsync(role);

//        // Store Gender as Claim
//        user.Claims.Add(new IdentityUserClaim() { ClaimType = ClaimTypes.Gender, ClaimValue = "Male" });
//            //user.Roles.Add(new IdentityUserRole() { RoleId=role.I;d, UserId=user.I;d });
//            var result = await UserManager.CreateAsync(user, model.Password);
//            if (result.Succeeded)
//            {
//                //await UserManager.AddToRoleAsync(user.I;d, "Admin");

//                await SignInAsync(user, isPersistent: false);
//                return RedirectToAction("Index", "Home");
//    }
//            else
//            {
//                AddErrors(result);
//}
//        }



		// confirm email
		[Route("ConfirmAccount/{id:Guid}")]
		[AllowAnonymous]
		public ActionResult ConfirmAccount(Guid id)
		{
			//Guid token;
			//Guid.TryParse(input: id, result: out token);

			string Message;

			if (_UserProfileService.ConfirmUserEmail(id))
			{
				Message = @"Congratulations! Your Email has been confirmed.";
			}
			else
			{
				Message = "Oops! Looks like something went wrong.";
			}
			ViewBag.Message = Message;

			return View();
		}



		// Profile Page
		public ActionResult ProfilePage()
		{
			ViewBag.Message = "Your Profile Page.";

			//Any Logic

			// We will be seding some kind of ViewModel
			//BaseViewModel vm = new BaseViewModel();
			return View("ProfileNG");
		}

	  



		// Search Page
		public ActionResult CompanyContactList()
		{
			ViewBag.Message = "Search for users.";

			//Any Logic
			return View("CompanyContactListNG");
		}
	


		/// forgotpassword
		[Route("forgotpassword")]
		[AllowAnonymous]
		public ActionResult ForgotPassword()
		{
			return View("ForgotPasswordNG");
		}



		//// resetpassword 
		[Route("resetpassword/{token:guid}")]
		[AllowAnonymous]
		public ActionResult ResetPassword(Guid token)
		{
			ItemViewModel<string> vm = new ItemViewModel<string>();

			if (_TokenService.GetToken(token) != null)
			{
				vm.Item = token.ToString();
				
			}
			else 
			{
				vm.Item = "0";
			}

			return View("ResetPasswordNG", vm);

		}

		[Route("setpassword/{token:guid}")]
		[AllowAnonymous]
		public ActionResult SetPasswordNG(Guid token)
		{
			ItemViewModel<string> vm = new ItemViewModel<string>();

			if (_TokenService.GetToken(token) != null)
			{
				vm.Item = token.ToString();

			}
			else
			{
				vm.Item = "0";
			}

			return View("SetPasswordNG", vm);

		}


		[Route("dropzonejsTEST")]
		public ActionResult DropzonejsTEST()
		{
			return View();
		}



		[HttpPost]
		[Route("dropzonejsTEST")]
		public ActionResult DropzonejsTEST(UploadRequest model)
		{
			HttpPostedFileBase file = model.File;

			if (file.ContentLength > 0)
			{

				string fileName = Path.GetFileName(file.FileName);
				string path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
				file.SaveAs(path);
			}
			return RedirectToAction("DropzonejsTEST");
		}



		// Store Front Test Page (Seller View) 
		[Route("storefrontNG")]
		public ActionResult StoreFront()
		{
			BaseViewModel model = new BaseViewModel();

			return View("StorefrontNG", model);
		}



		// Store Front Test Page (Buyer View)
		[Route("storefrontview")]
		public ActionResult StoreFrontView()
		{
			var model = new BaseViewModel();

			return View(model);
		}



		[Route("TimeLine/quoterequest")]
		public ActionResult TimeLine()
		{
			return View();
		}
	


		[Route("ratings")]
		public ActionResult Ratings()
		{
			return View("RatingsNG");
		}


		[Route("messages")]
		public ActionResult Messages()
		{
			return View("MessagesNG");
		}

		[Route("marketplace/quoterequest")]
		public ActionResult MarketPlace()
		{
			return View("MarketPlaceQR");
		}

		[Route("bidContractTest")]
		public ActionResult BidContractTest()
		{
			return View();
		}

		[Route("BuyerProfiles")]
		public ActionResult BuyerPublicProfilesList()
		{
			return View();
		}

		[Route("BuyerProfiles/{companyid:int}")]
		public ActionResult BuyerPublicDetail(int companyid)
		{

			ItemViewModel<int> vm = new ItemViewModel<int>();

			vm.Item = companyid;

			return View("BuyerPublicDetail", vm);
		}

		[Route("SupplierProfiles")]
		[AllowAnonymous]
		public ActionResult SupplierPublicProfilesList()
		{
			return View();
		}

	}
}
