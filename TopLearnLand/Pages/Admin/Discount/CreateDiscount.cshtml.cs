using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearnLand_Core.Services.InterFaces;
using TopLearnLand_DataLayer.Entities.Order;

namespace TopLearnLand.Pages.Admin.Discount
{
    public class CreateDiscountModel : PageModel
    {
        private IOrderService _orderService;

        public CreateDiscountModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #region MyRegion
        [BindProperty]
        public DisCount DisCount { get; set; }

        #endregion
        public void OnGet()
        {

        }

        public IActionResult OnPost(string fDate = "", string toDate = "")
        {
            if (fDate != null)
            {
                string[] fdate = fDate.Split('/');
                DisCount.FromDate = new DateTime(int.Parse(fdate[0]),
                    int.Parse(fdate[1]),
                    int.Parse(fdate[2]),
                    new PersianCalendar());
            }

            if (toDate != null)
            {
                string[] todate = toDate.Split('/');
                DisCount.ToDate = new DateTime(int.Parse(todate[0]),
                    int.Parse(todate[1]),
                    int.Parse(todate[2]),
                    new PersianCalendar());
            }

            if (!ModelState.IsValid && _orderService.IsExistCode(DisCount.DisCountCode))
                return Page();

            _orderService.AddDiscount(DisCount);

            return RedirectToPage("Index");
        }

        //admin/discount/createDiscount?handler=checkcode
        //admin/discount/createDiscount/checkcode
        public IActionResult OnGetCheckCode(string code)
        {
            return Content(_orderService.IsExistCode(code).ToString());
        }
    }
}