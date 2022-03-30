using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPNETMVCApplication.ActionServiceReference;

namespace ASPNETMVCApplication.Areas.Shop.Models
{
    /// <summary>
    /// Static class that maps Product and Cart Model objects to 
    /// Data Transfer Objects (DTOs) and vice versa.
    /// This class contains extension methods only.
    /// </summary>
    public static class Mapper
    {
        /// <summary>
        /// Maps product DTO to product Model.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static ProductModel ToModel(this Product product)
        {
            return new ProductModel
            {
                ProductId = product.ProductId,
                CategoryName = product.Category != null ? product.Category.Name : null,
                Name = product.ProductName,
                Price = string.Format("{0:c}", product.UnitPrice),
                Weight = product.Weight,
                UnitsInStock = product.UnitsInStock
            };
        }

        /// <summary>
        /// Maps list of product DTOs to list of product Models.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static List<ProductModel> ToModel(this List<Product> products)
        {
            var models = new List<ProductModel>();
            if (products != null && products.Count > 0)
              products.ForEach(c => models.Add(c.ToModel()));

            return models;
        }

        /// <summary>
        /// Maps shopping cart item DTO to shopping cart item Model.
        /// </summary>
        /// <param name="cartItem"></param>
        /// <returns></returns>
        public static CartItemModel ToModel(this ShoppingCartItem cartItem)
        {
            return new CartItemModel
            {
                CartItemId = cartItem.Id, 
                ProductId = cartItem.Id, 
                ProductName = cartItem.Name, 
                Quantity = cartItem.Quantity, 
                TotalPrice = string.Format("{0:c}", cartItem.UnitPrice * cartItem.Quantity), 
                UnitPrice = string.Format("{0:c}", cartItem.UnitPrice) 
            };
        }

        /// <summary>
        /// Maps Shopping Cart DTO to shopping cart Model.
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static CartModel ToModel(this ShoppingCart cart)
        {
            var items = new List<CartItemModel>();
            if (cart != null && cart.CartItems != null && cart.CartItems.Length > 0)
               cart.CartItems.ToList().ForEach(i => items.Add(i.ToModel()));

            return new CartModel { CartItems = items, Shipping = string.Format("{0:c}", cart.Shipping), ShippingMethod = cart.ShippingMethod, SubTotal = string.Format("{0:c}", cart.SubTotal), Total = string.Format("{0:c}", cart.Total) };
        }
    }
}