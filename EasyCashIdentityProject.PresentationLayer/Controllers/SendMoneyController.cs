﻿using EasyCashIdentityProject.BusinessLayer.Abstract;
using EasyCashIdentityProject.DataAccessLayer.Concrete;
using EasyCashIdentityProject.DtoLayer.Dtos.CustomerAccountProcessDtos;
using EasyCashIdentityProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EasyCashIdentityProject.PresentationLayer.Controllers
{
    public class SendMoneyController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICustomerAccountProcessService _customerAccountProcessService;

        public SendMoneyController(UserManager<AppUser> userManager, ICustomerAccountProcessService customerAccountProcessService)
        {
            _userManager = userManager;
            _customerAccountProcessService = customerAccountProcessService;
        }

        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(SendMoneyForCustomerAccountProcessDto sendMoneyForCustomerAccountProcessDto)
        {
            var context = new Context();

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var receiverAccountNumberId = context.CustomerAccounts
                .Where(x => x.CustomerAccountNumber == sendMoneyForCustomerAccountProcessDto.ReceiverAccountNumber)
                .Select(y => y.CustomerAccountId).FirstOrDefault();

            var senderAccountNumberId = context.CustomerAccounts.Where(x => x.AppUserId == user.Id)
                .Where(y => y.CustomerAccountCurrency == "Türk Lirası").Select(z => z.CustomerAccountId)
                .FirstOrDefault();



            sendMoneyForCustomerAccountProcessDto.SenderId = user.Id;
            sendMoneyForCustomerAccountProcessDto.ProcessDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            sendMoneyForCustomerAccountProcessDto.ProcessType = "Havale";
            sendMoneyForCustomerAccountProcessDto.ReceiverId = receiverAccountNumberId;



            var values = new CustomerAccountProcess();

            values.ProcessDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            values.SenderId = senderAccountNumberId;
            values.ProcessType = "Havale";
            values.ReceiverId = receiverAccountNumberId;
            values.Amount = sendMoneyForCustomerAccountProcessDto.Amount;
            values.Description = sendMoneyForCustomerAccountProcessDto.Description;

            _customerAccountProcessService.TInsert(values);

            return RedirectToAction("Index", "Deneme");
        }



    }
}
