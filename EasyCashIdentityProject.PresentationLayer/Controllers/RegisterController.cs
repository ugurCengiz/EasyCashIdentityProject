﻿using EasyCashIdentityProject.DtoLayer.Dtos.AppUserDtos;
using EasyCashIdentityProject.EntityLayer.Concrete;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Cryptography;

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
	            Random random = new Random();
	            int code = random.Next(100000, 1000000);

				AppUser appUser = new AppUser()
                {
                    UserName = appUserRegisterDto.Username,
                    Name = appUserRegisterDto.Name,
                    SurName = appUserRegisterDto.SurName,
                    Email = appUserRegisterDto.Email,

                    //Düzeltilicek
                    City = "aaaa",
                    District = "bb",
                    ImageUrl = "cc",
                    ConfirmCode = code

                };

               

				var result = await _userManager.CreateAsync(appUser, appUserRegisterDto.Password);

                if (result.Succeeded )
                {
	                MimeMessage mimeMessage = new MimeMessage();
	                MailboxAddress mailboxAddressFrom = new MailboxAddress("Easy Cash Admin","ugur149976@gmail.com");
	                MailboxAddress mailboxAddressTo = new MailboxAddress("User", appUser.Email);

                    mimeMessage.From.Add(mailboxAddressFrom);
                    mimeMessage.To.Add(mailboxAddressTo);

                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.TextBody = $"Merhaba {appUserRegisterDto.Name} {appUserRegisterDto.SurName} kayıt işlemini gerçekleştirmek için oynay kodunuz:" + code;
                    mimeMessage.Body = bodyBuilder.ToMessageBody();

                    mimeMessage.Subject = "Easy Cash Onay Kodu";

                    SmtpClient client = new SmtpClient();
                    client.Connect("smtp.gmail.com",587,false);
                    client.Authenticate("ugur149976@gmail.com", "tdwvwctcrggnkjwn");
                    client.Send(mimeMessage);
                    client.Disconnect(true);

                    TempData["Mail"] = appUserRegisterDto.Email;


                    return RedirectToAction("Index", "ConfirmMail");
                }
                else
                {
	               
					foreach (var item in result.Errors )
	                {
		                ModelState.AddModelError("",item.Description);
		               
	                }
                }
                
            }

            return View();
        }

    }
}
