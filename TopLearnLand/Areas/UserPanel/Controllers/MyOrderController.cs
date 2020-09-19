using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using TopLearnLand_Core.DTOs_ViewModels_.DisCount;
using TopLearnLand_Core.Services.InterFaces;

namespace TopLearnLand.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class MyOrderController : Controller
    {
        private IOrderService _orderService;
        private ICourseService _courseService;
        private IWebHostEnvironment _environment;

        public MyOrderController(IOrderService orderService, ICourseService courseService, IWebHostEnvironment environment)
        {
            _orderService = orderService;
            _courseService = courseService;
            _environment = environment;
        }

        public IActionResult Index(bool deletedOrder = false)
        {
            ViewBag.deletedOrder = deletedOrder;
            return View(_orderService.GetUserOrders(User.Identity.Name));
        }

        [Route("UserPanel/MyOrder/ShowOrder/{orderId}")]
        public IActionResult ShowOrder(int orderId, bool finaly = false, string type = "", string code = "")
        {
            var order = _orderService.GetOrderForUserPanel(User.Identity.Name, orderId);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.finaly = finaly;
            ViewBag.code = code;
            ViewBag.type = type;
            return View(order);
        }

        [Route("UserPanel/MyOrder/FinalyOrder/{orderId}/{totalSumOrder}")]
        public IActionResult FinalyOrder(int orderId, int totalSumOrder)
        {
            if (_orderService.FinalyOrder(User.Identity.Name, orderId, totalSumOrder))
            {
                return Redirect("/UserPanel/MyOrder/ShowOrder/" + orderId + "?finaly=true");
            }

            return BadRequest();
        }

        public IActionResult UseDiscount(int orderId, string code)
        {
            if (_orderService.CheckUserUsedCodeCount(code, orderId))
            {
                DiscountUseType discountType = _orderService.UseDisCount(orderId, code);
                return Redirect("/UserPanel/MyOrder/ShowOrder/" + orderId + "?type=" + discountType.ToString() + "&code=" + code);
            }

            return Redirect("/UserPanel/MyOrder/ShowOrder/" + orderId + "?type=UserUsed");
        }


        [Route("UserPanel/MyOrder/DeleteOrder/{orderId}")]
        public IActionResult DeleteOrder(int orderId)
        {
            _orderService.DeleteOrder(orderId);
            return Redirect("/UserPanel/MyOrder?deletedOrder=true");
        }


        [Route("UserPanel/MyOrder/DeleteProduct/{orderDetailId}")]
        public IActionResult DeleteProduct(int orderDetailId, string code)
        {
            int orderId = _orderService.DeleteProduct(orderDetailId, code);

            if (_orderService.ExistOrder(orderId))
            {
                return Redirect("/UserPanel/MyOrder/ShowOrder/" + orderId);
            }
            else
            {
                return Redirect("/UserPanel/MyOrder?deletedOrder=true");
            }
        }
    }
}