﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObjects;

namespace DataObjects.EntityFramework.ModelMapper
{
    /// <summary>
    /// Maps Entity Framework entities to business objects and vice versa.
    /// </summary>
    public class Mapper
    {
        /// <summary>
        /// Maps customer entity to customer business object.
        /// </summary>
        /// <param name="entity">A customer entity to be transformed.</param>
        /// <returns>A customer business object.</returns>
        internal static Customer Map(CustomerEntity entity)
        {
            return new Customer
            {
                CustomerId = entity.CustomerId,
                Company = entity.CompanyName,
                City = entity.City,
                Country = entity.Country,
                Version = entity.Version.AsBase64String()
            };
        }


        /// <summary>
        /// Maps customer business object to customer entity.
        /// </summary>
        /// <param name="customer">A customer business object.</param>
        /// <returns>A customer entity.</returns>
        internal static CustomerEntity Map(Customer customer)
        {
            return new CustomerEntity
            {
                CustomerId = customer.CustomerId,
                CompanyName = customer.Company,
                City = customer.City,
                Country = customer.Country
            };
        }


        /// <summary>
        /// Maps order entity to order business object.
        /// </summary>
        /// <param name="entity">An order entity.</param>
        /// <returns>An order business object.</returns>
        internal static Order Map(OrderEntity entity)
        {
            return new Order
            {
                OrderId = entity.OrderId,
                Freight = entity.Freight.HasValue ? (float)entity.Freight : default(float),
                OrderDate = entity.OrderDate,
                RequiredDate = entity.RequiredDate.HasValue ? (DateTime)entity.RequiredDate : default(DateTime),
                Version = entity.Version.AsBase64String()
            };
        }

        /// <summary>
        /// Maps order detail entity to order detail business object.
        /// </summary>
        /// <param name="entity">An order detail entity.</param>
        /// <returns>An order detail business object.</returns>
        internal static OrderDetail Map(OrderDetailEntity entity)
        {
            return new OrderDetail
            {
                ProductName = entity.Product == null ? "" : entity.Product.ProductName,
                Discount = (float)entity.Discount,
                Quantity = entity.Quantity,
                UnitPrice = (float)entity.UnitPrice,
                Version = entity.Version.AsBase64String()
            };
        }

        /// <summary>
        /// Maps product category entity to category business object.
        /// </summary>
        /// <param name="entity">A category entity.</param>
        /// <returns>A category business object.</returns>
        internal static Category Map(CategoryEntity entity)
        {
            return new Category
            {
                CategoryId = entity.CategoryId,
                Description = entity.Description,
                Name = entity.CategoryName,
                Version = entity.Version.AsBase64String()
            };
        }


        /// <summary>
        /// Maps product entity to product business object.
        /// </summary>
        /// <param name="entity">A product entity.</param>
        /// <returns>A product business object.</returns>
        internal static Product Map(ProductEntity entity)
        {
            return new Product
            {
                ProductId = entity.ProductId,
                ProductName = entity.ProductName,
                UnitPrice = (double)entity.UnitPrice,
                UnitsInStock = entity.UnitsInStock,
                Weight = entity.Weight,
                Version = entity.Version.AsBase64String()
            };
        }
    }
}
