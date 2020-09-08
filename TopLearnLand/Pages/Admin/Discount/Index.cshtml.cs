using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearnLand_Core.Services.InterFaces;
using TopLearnLand_DataLayer.Entities.Order;

namespace TopLearnLand.Pages.Admin.Discount
{
    public class IndexModel : PageModel
    {
        private IOrderService _orderService;

        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #region Model

        public List<DisCount> DisCounts { get; set; }   

        #endregion
        public void OnGet()
        {
            DisCounts = _orderService.GetDisCounts();
        }
    }
}