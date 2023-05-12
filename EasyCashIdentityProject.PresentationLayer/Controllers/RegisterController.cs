using EasyCashIdentityProject.BusinessLayer.ValidationRules.AppUserValidationRules;
using EasyCashIdentityProject.DtoLayer.Dtos.AppUserDtos;
using EasyCashIdentityProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EasyCashIdentityProject.PresentationLayer.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(AppUserRegisterDto appUserRegisterDto)
        {
            if (ModelState.IsValid )
            {
                AppUser appUser = new AppUser()
                {
                    UserName = appUserRegisterDto.Username,
                    Name = appUserRegisterDto.Name,
                    SurName = appUserRegisterDto.SurName,
                    Email = appUserRegisterDto.Email,

                    //Düzeltilicek
                    City = "aaaa",
                    District = "bb",
                    ImageUrl = "cc"

                };

                var validator = new AppUserRegisterValidator();
				var validation = validator.Validate(appUserRegisterDto);

				var result = await _userManager.CreateAsync(appUser, appUserRegisterDto.Password);

                if (result.Succeeded  && !validation.IsValid )
                {
                    return RedirectToAction("Index", "ConfirmMail");
                }
                else
                {
	                foreach (var item in result.Errors )
	                {
		                ModelState.AddModelError("",item.Description);
		                foreach (var valid in validation.Errors)
		                {
							ModelState.AddModelError("", valid.ErrorMessage);
						}
	                }
                }
                
            }

            return View();
        }

    }
}
