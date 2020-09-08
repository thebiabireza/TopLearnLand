using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearnLand_Core.DTOs_ViewModels_;
using TopLearnLand_Core.Services.InterFaces;
using ZarinpalSandbox;

namespace TopLearnLand.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class WalletController : Controller
    {
        private readonly IUserService _userService;

        public WalletController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("UserPanel/Wallet")]
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["WalletUser"] = _userService.GetWalletUser(User.Identity.Name);
            return View();
        }

        [Route("UserPanel/Wallet")]
        [HttpPost]
        public async Task<IActionResult> Index(ChargeWalletViewModel chargeWallet)
        {
            try
            {
                string currenyUser = User.Identity.Name;

                if (!ModelState.IsValid)
                {
                    ViewData["WalletUser"] = _userService.GetWalletUser(currenyUser);
                    return View(chargeWallet);
                }

                int chargeWalletId = 0;
                if (chargeWallet.Amount > 0)
                {
                    chargeWalletId = _userService.ChargeWallet(currenyUser, "شارژ کیف پول", chargeWallet.Amount);
                }
                else
                {
                    ViewData["WalletUser"] = _userService.GetWalletUser(currenyUser);
                    ViewBag.Ammount = false;
                    return View(chargeWallet);
                }

                #region Online Payment

                var payment = new Payment(chargeWallet.Amount);
                string callBackUrl = "https://localhost:44364/OnlinePayment/" + chargeWalletId;
                string email = "xrezab.a.b0016@gmail.com";
                var response = await payment.PaymentRequest("شارژ کیف پول", callBackUrl, email, "09303483049")
                    .ConfigureAwait(true);

                if (response.Status == 100)
                {
                    return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + response.Authority);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception error)
            {
                ViewBag.Error = error.Message;
            }

            #endregion

            return null;
        }
    }
}