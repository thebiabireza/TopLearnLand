using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopLearnLand_Core.Services.InterFaces;
using ZarinpalSandbox;

namespace TopLearnLand.Controllers
{
    public class HomeController : Controller
    {
        private IUserService _userService;
        private ICourseService _courseService;

        public HomeController(IUserService userService, ICourseService courseService)
        {
            _userService = userService;
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            return View(_courseService.GetCourses().Item1);
        }

        #region OnlinePayment

        [Route("OnlinePayment/{walletId}")]
        public async Task<IActionResult> OnlinePayment(int walletId)
        {
            try
            {
                #region Check CallBack OnlinePayment 

                if (HttpContext.Request.Query["Status"] != "" &&
                    HttpContext.Request.Query["Status"] == "OK" &&
                    HttpContext.Request.Query["Authority"] != "")
                {
                    var wallet = _userService.GetWalletByWalletId(walletId);

                    string authority = HttpContext.Request.Query["Authority"].ToString();
                    var payment = new Payment(wallet.Amount);

                    var respone = await payment.Verification(authority).ConfigureAwait(true);

                    if (respone.Status == 100)
                    {
                        ViewBag.Code = respone.RefId;
                        ViewBag.IsSuccess = true;
                        wallet.IsPay = true;
                        _userService.UpdateWallet(wallet);

                        Redirect("/UserPanel/Wallet");
                    }

                }

                #endregion
            }
            catch (Exception error)
            {
                ViewBag.Error = error.Message;
            }
            return View();
        }

        #endregion

        #region CKEditor File Upload To Server

        [HttpPost]
        [Route("file-upload")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MyImages", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);
            }

            var url = $"/MyImages/{fileName}";

            return Json(new { uploaded = true, url });
        }

        #endregion

        #region GetSubGroup For Create Course

        [Route("Home/GetSubGroups/{groupId}")]
        public JsonResult GetSubGroups(int groupId)
        {
            List<SelectListItem> subGroupsList = new List<SelectListItem>()
            {
                new SelectListItem(){Text="select", Value = ""}
            };
            subGroupsList.AddRange(_courseService.GetSubGroupForManageCourse(groupId));

            return Json(new SelectList(subGroupsList, "Value", "Text"));
        }

        #endregion
    }
}