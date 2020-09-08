using System;
using System.Collections.Generic;
using System.Text;
using TopLearnLand_Core.DTOs_ViewModels_.DisCount;
using TopLearnLand_DataLayer.Entities.Order;
using TopLearnLand_DataLayer.Entities.User;

namespace TopLearnLand_Core.Services.InterFaces
{
    public interface IOrderService
    {
        Order GetOrderByOrderId(int orderId);
        List<OrderDetail> GetOrderDetailsByOrderId(int orderId);
        OrderDetail GetOrderDetailById(int orderDetailId);
        int GetOrderDetailCountByOrderId(int orderId);
        Order GetOrderForUserPanel(string userName,int orderId);
        int AddOrder(string userName,int courseId);
        void DeleteOrder(int orderId);
        int DeleteProduct(int orderDetailId, string code);
        void UpdatePriceOrder(int orderId);
        bool FinalyOrder(string userName, int orderId, int totalSumOrder);
        List<Order> GetUserOrders(string userName);
        void UpdateOrder(Order order);
        bool ExistOrder(int orderId);
        bool IsUserInCourse(string userName, int courseId);

        #region Discount

        DiscountUseType UseDisCount(int orderId,string code);
        int GetPercentByDiscountCode(string code);
        DisCount GetDiscountByDiscountCode(string code);
        int GetUsedCountById(int userId);
        bool CheckUserUsedCode(string code, DisCount discount, int userId);
        bool CheckUserUsedCodeCount(string code, int orderId);
        UserDiscountCode GetUserDiscountById(int userId);
        void UpdateUserDiscount(UserDiscountCode userDiscount);
        void AddDiscount(DisCount discount);
        void EditDiscount(DisCount discount);
        List<DisCount> GetDisCounts();
        DisCount GetDiscountById(int discountId);
        bool IsExistCode(string code);

        #endregion

    }
}
