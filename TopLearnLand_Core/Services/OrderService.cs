using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TopLearnLand_Core.DTOs_ViewModels_.DisCount;
using TopLearnLand_Core.Services.InterFaces;
using TopLearnLand_DataLayer.Context;
using TopLearnLand_DataLayer.Entities.Course;
using TopLearnLand_DataLayer.Entities.Order;
using TopLearnLand_DataLayer.Entities.User;
using TopLearnLand_DataLayer.Entities.Wallet;

namespace TopLearnLand_Core.Services
{
    public class OrderService : IOrderService
    {
        private TopLearnLandContext _DBContext;
        private ICourseService _courseService;
        private IUserService _userService;


        public OrderService(TopLearnLandContext dbContext, ICourseService courseService, IUserService userService)
        {
            _DBContext = dbContext;
            _courseService = courseService;
            _userService = userService;
        }

        public Order GetOrderByOrderId(int orderId)
        {
            return _DBContext.Orders.Find(orderId);
        }

        public List<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return _DBContext.OrderDetails.Where(od => od.OrderId == orderId).ToList();
        }

        public OrderDetail GetOrderDetailById(int orderDetailId)
        {
            return _DBContext.OrderDetails.Find(orderDetailId);
        }

        public int GetOrderDetailCountByOrderId(int orderId)
        {
            return _DBContext.OrderDetails.Where(od => od.OrderId == orderId).Count();
        }

        public Order GetOrderForUserPanel(string userName, int orderId)
        {
            int userId = _userService.GetUserIdByUserName(userName);
            return _DBContext.Orders.Include(od => od.OrderDetails)
                .ThenInclude(od => od.Course)
                .FirstOrDefault(o => o.UserId == userId && o.OrderId == orderId);
        }

        public int AddOrder(string userName, int courseId)
        {
            var user = _userService.GetUserByUserName(userName);
            Order order = _DBContext.Orders.FirstOrDefault(order => order.UserId == user.UserId && !order.IsFinaly);
            var course = _courseService.GetCourseById(courseId);

            if (order == null)
            {
                order = new Order()
                {
                    UserId = user.UserId,
                    OrderCreateDate = DateTime.Now,
                    OrderSum = course.CoursePrice,
                    IsFinaly = false,
                };
                _DBContext.Orders.Add(order);
                _DBContext.SaveChanges();

                _DBContext.OrderDetails.Add(new OrderDetail()
                {
                    OrderId = order.OrderId,
                    CourseId = course.CourseId,
                    Count = +1,
                    Price = course.CoursePrice
                });
                _DBContext.SaveChanges();
            }
            else
            {
                OrderDetail orderDetail = _DBContext.OrderDetails
                    .FirstOrDefault(od => od.OrderId == order.OrderId && od.CourseId == courseId);

                if (orderDetail != null)
                {
                    orderDetail.Count += 1;
                    _DBContext.OrderDetails.Update(orderDetail);
                    _DBContext.SaveChanges();
                }
                else
                {
                    orderDetail = new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        CourseId = courseId,
                        Count = 1,
                        Price = course.CoursePrice
                    };
                    _DBContext.OrderDetails.Add(orderDetail);
                    _DBContext.SaveChanges();
                }

                UpdatePriceOrder(order.OrderId);
                _DBContext.SaveChanges();
            }

            return order.OrderId;
        }

        public void DeleteOrder(int orderId)
        {
            List<OrderDetail> orderDetails = GetOrderDetailsByOrderId(orderId);
            Order deleteOrder = GetOrderByOrderId(orderId);

            foreach (var orderDetail in orderDetails)
            {
                _DBContext.OrderDetails.Remove(orderDetail);
            }
            _DBContext.Orders.Remove(deleteOrder);
            _DBContext.SaveChanges();
        }

        public int DeleteProduct(int orderDetailId, string code)
        {
            OrderDetail deleteOrderDetail = GetOrderDetailById(orderDetailId);
            Order order = GetOrderByOrderId(deleteOrderDetail.OrderId);

            #region Check OrdeSum & Add Percent

            int percent = 0;
            int detailPrice = deleteOrderDetail.Count * deleteOrderDetail.Price;
            int detailPercent = 0;
            if (!string.IsNullOrEmpty(code))
            {
                percent = GetPercentByDiscountCode(code);
                detailPercent = (detailPrice * percent) / 100;
                order.OrderSum = (order.OrderSum) - (detailPercent);
                UpdateOrder(order);
            }

            #endregion

            int orderId = deleteOrderDetail.OrderId;

            order.OrderSum = (order.OrderSum) - (detailPrice);
            UpdateOrder(order);

            #region Remove OrderDetail(product)

            _DBContext.OrderDetails.Remove(deleteOrderDetail);
            _DBContext.SaveChanges();

            #endregion


            #region Remove Order If not Exist orderdetails

            if (GetOrderDetailCountByOrderId(orderId) <= 0)
            {
                _DBContext.Orders.Remove(order);
                _DBContext.SaveChanges();
            }

            #endregion
            return orderId;
        }

        public void UpdatePriceOrder(int orderId)
        {
            var order = GetOrderByOrderId(orderId);
            order.OrderSum = _DBContext.OrderDetails
                .Where(o => o.OrderId == order.OrderId)
                .Select(od => od.Count * od.Price).Sum();
        }

        public bool FinalyOrder(string userName, int orderId, int totalSumOrder)
        {
            int userId = _userService.GetUserIdByUserName(userName);

            Order order = _DBContext.Orders.Include(o => o.OrderDetails)
                .ThenInclude(od => od.Course)
                .FirstOrDefault(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null || order.IsFinaly)
            {
                return false;
            }

            if (_userService.BalanceUserWallet(userName) >= totalSumOrder)
            {
                order.IsFinaly = true;
                _userService.AddWallet(new Wallet
                {
                    WalletId = 0,
                    UserId = userId,
                    Amount = totalSumOrder,
                    CreateDate = DateTime.Now,
                    Description = "فاکتور شماره#" + order.OrderId,
                    IsPay = true,
                    WalletTypeId = 2
                });
                _DBContext.Orders.Update(order);

                foreach (var orderDetail in order.OrderDetails)
                {
                    _DBContext.UserCourses.Add(new UserCourse()
                    {
                        CourseId = orderDetail.CourseId,
                        UserId = userId
                    });
                }

                _DBContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Order> GetUserOrders(string userName)
        {
            int userId = _userService.GetUserIdByUserName(userName);
            return _DBContext.Orders.Include(order => order.OrderDetails)
                .ThenInclude(od => od.Course)
                .Where(order => order.UserId == userId).ToList();
        }

        public void UpdateOrder(Order order)
        {
            _DBContext.Orders.Update(order);
            _DBContext.SaveChanges();
        }

        public bool ExistOrder(int orderId)
        {
            return _DBContext.Orders.Any(o => o.OrderId == orderId);
        }

        public bool IsUserInCourse(string userName, int courseId)
        {
            int userId = _userService.GetUserIdByUserName(userName);
            return _DBContext.UserCourses.Any(uc => uc.CourseId == courseId && uc.UserId == userId);
        }

        public DiscountUseType UseDisCount(int orderId, string code)
        {
            DisCount discount = _DBContext.DisCounts.SingleOrDefault(d => d.DisCountCode == code);

            #region Check Discount States

            if (discount == null)
                return DiscountUseType.NotFound;

            if (discount.FromDate != null && discount.FromDate < DateTime.Now)
                return DiscountUseType.Expired;

            if (discount.ToDate != null && discount.ToDate >= DateTime.Now)
                return DiscountUseType.Expired;

            if (discount.UsableCount != null && discount.UsableCount < 1)
                return DiscountUseType.Finished;

            var order = GetOrderByOrderId(orderId);

            if (CheckUserUsedCode(code, discount, order.UserId))
                return DiscountUseType.UserUsed;

            #endregion


            #region Add Discount

            int percent = (order.OrderSum * discount.Percent) / 100;
            order.OrderSum = order.OrderSum - percent;
            UpdateOrder(order);

            #endregion

            if (discount.UsableCount != null)
            {
                discount.UsableCount -= 1;
            }
            _DBContext.DisCounts.Update(discount);

            #region Add UserDiscount

            UserDiscountCode userDiscount = GetUserDiscountById(order.UserId);
            if (userDiscount.UsedCount == 0)
            {
                _DBContext.UserDiscountCodes.Add(new UserDiscountCode()
                {
                    DisCountId = discount.DisCountId,
                    UserId = order.UserId,
                    UsedCount = 2 //TODO Configure In Admin
                });
                _DBContext.SaveChanges();
            }

            #endregion

            #region Update UserDiscount

            if (userDiscount != null && userDiscount.UsedCount > 0)
            {
                userDiscount.UsedCount -= 1;

                UpdateUserDiscount(userDiscount);
            }
            if (userDiscount != null && userDiscount.UsedCount == 0)
            {
                userDiscount.IsUsed = true;
                UpdateUserDiscount(userDiscount);
            }
            #endregion

            return DiscountUseType.Success;
        }

        public int GetPercentByDiscountCode(string code)
        {
            return _DBContext.DisCounts.SingleOrDefault(d => d.DisCountCode == code).Percent;
        }

        public DisCount GetDiscountByDiscountCode(string code)
        {
            return _DBContext.DisCounts.SingleOrDefault(discount => discount.DisCountCode == code);
        }

        public int GetUsedCountById(int userId)
        {
            return _DBContext.UserDiscountCodes.FirstOrDefault(ud => ud.UserId == userId).UsedCount;
        }

        public bool CheckUserUsedCode(string code, DisCount discount, int userId)
        {
            return _DBContext.UserDiscountCodes.Any(ud =>
                ud.UserId == userId && ud.DisCountId == discount.DisCountId && ud.IsUsed);
        }

        public bool CheckUserUsedCodeCount(string code, int orderId)
        {
            int discountId = GetDiscountByDiscountCode(code).DisCountId;
            var order = GetOrderByOrderId(orderId);
            UserDiscountCode userDiscount = GetUserDiscountById(order.UserId);

            if (userDiscount == null)
                return true;

            if (userDiscount.UsedCount == 0 && userDiscount.DisCountId == discountId)
            {
                return false;
            }

            return true;
        }

        public UserDiscountCode GetUserDiscountById(int userId)
        {
            return _DBContext.UserDiscountCodes.FirstOrDefault(u => u.UserId == userId && !u.IsUsed);
        }

        public void UpdateUserDiscount(UserDiscountCode userDiscount)
        {
            _DBContext.UserDiscountCodes.Update(userDiscount);
            _DBContext.SaveChanges();
        }

        public void AddDiscount(DisCount discount)
        {
            _DBContext.DisCounts.Add(discount);
            _DBContext.SaveChanges();
        }

        public void EditDiscount(DisCount discount)
        {
            _DBContext.DisCounts.Update(discount);
            _DBContext.SaveChanges();
        }

        public List<DisCount> GetDisCounts()
        {
            return _DBContext.DisCounts.ToList();
        }

        public DisCount GetDiscountById(int discountId)
        {
            return _DBContext.DisCounts.Find(discountId);
        }

        public bool IsExistCode(string code)
        {
            return _DBContext.DisCounts.Any(d => d.DisCountCode == code);
        }
    }
}
