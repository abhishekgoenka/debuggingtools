using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPNETMVCApplication.ActionServiceReference;

namespace ASPNETMVCApplication.Areas.Admin.Models
{
    /// <summary>
    /// Static class that maps Customer and Order Model objects to 
    /// Data Transfer Objects (DTOs) and vice versa.
    /// This class contains extension methods only.
    /// </summary>
    public static class Mapper
    {
        /// <summary>
        /// Maps customer DTO to customer Model.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static CustomerModel ToModel(this Customer customer)
        {
            return new CustomerModel
            {
                CustomerId = customer.CustomerId,
                CompanyName = customer.Company,
                City = customer.City,
                Country = customer.Country,
                LastOrderDate = string.Format("{0:MM/dd/yyyy}", customer.LastOrderDate),
                NumOrders = customer.NumOrders,
                Version = customer.Version
            };
        }

        /// <summary>
        /// Maps customer Model to customer DTO.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Customer FromModel(this CustomerModel model)
        {
            return new Customer
            {
                CustomerId = model.CustomerId,
                Company = model.CompanyName,
                City = model.City,
                Country = model.Country,
                Version = model.Version
            };
        }

        /// <summary>
        /// Maps list of customer DTOs to list of customer Models.
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>
        public static List<CustomerModel> ToModel(this List<Customer> customers)
        {
            var models = new List<CustomerModel>();

            if (customers != null && customers.Count > 0)
                customers.ForEach(c => models.Add(c.ToModel()));

            return models;
        }

        /// <summary>
        /// Maps list of customer Models to list of customer DTOs.
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static List<Customer> FromModel(this List<CustomerModel> models)
        {
            var customers = new List<Customer>();

            if (models != null && models.Count > 0)
                models.ForEach(m => customers.Add(m.FromModel()));

            return customers;
        }


        /// <summary>
        /// Maps order DTO to order Model.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static OrderModel ToModel(this Order order)
        {
            return new OrderModel
            {
                OrderId = order.OrderId,
                OrderDate = string.Format("{0:MM/dd/yyyy}", order.OrderDate),
                RequiredDate = string.Format("{0:MM/dd/yyyy}", order.RequiredDate),
                Shipping = string.Format("{0:c}", order.Freight),
                Version = order.Version
            };
        }

        /// <summary>
        /// Maps list of order DTOs to list of order Models.
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
        public static List<OrderModel> ToModel(this List<Order> orders)
        {
            var models = new List<OrderModel>();

            if (orders != null && orders.Count > 0)
                orders.ForEach(c => models.Add(c.ToModel()));

            return models;
        }

        /// <summary>
        /// Maps detail DTO to order detail Model.
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
        public static OrderDetailModel ToModel(this OrderDetail orderDetail)
        {
            return new OrderDetailModel
            {
                ProductName = orderDetail.ProductName,
                Quantity = orderDetail.Quantity,
                UnitPrice = string.Format("{0:c}", orderDetail.UnitPrice),
                Discount = string.Format("{0:c}", orderDetail.Discount)
            };
        }

        /// <summary>
        /// Maps list of order detail DTOs to list of order detail Models.
        /// </summary>
        /// <param name="orderDetails"></param>
        /// <returns></returns>
        public static List<OrderDetailModel> ToModel(this List<OrderDetail> orderDetails)
        {
            var models = new List<OrderDetailModel>();

            if (orderDetails != null && orderDetails.Count > 0)
                orderDetails.ForEach(c => models.Add(c.ToModel()));

            return models;
        }
    }
}


