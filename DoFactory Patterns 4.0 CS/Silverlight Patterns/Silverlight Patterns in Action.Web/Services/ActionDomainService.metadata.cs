using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.ServiceModel.DomainServices.Server;

//
//
// Note. This file is auto-generated, but editable.
// Here you typically add property attributes, primarily for property validation.
//  
// For Patterns-in-Action we only added a couple [Include] attributes.
//
//
namespace Silverlight_Patterns_in_Action.Web
{
    // The MetadataTypeAttribute identifies CategoryMetadata as the class
    // that carries additional metadata for the Category class.
    [MetadataTypeAttribute(typeof(Category.CategoryMetadata))]
    public partial class Category
    {
        // This class allows you to attach custom attributes to properties
        // of the Category class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class CategoryMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private CategoryMetadata()
            {
            }

            public int CategoryId { get; set; }

            public string CategoryName { get; set; }

            public string Description { get; set; }

            public EntityCollection<Product> Products { get; set; }

            public byte[] Version { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies CustomerMetadata as the class
    // that carries additional metadata for the Customer class.
    [MetadataTypeAttribute(typeof(Customer.CustomerMetadata))]
    public partial class Customer
    {

        // This class allows you to attach custom attributes to properties
        // of the Customer class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class CustomerMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private CustomerMetadata()
            {
            }

            public string City { get; set; }

            public string CompanyName { get; set; }

            public string Country { get; set; }

            public int CustomerId { get; set; }

            public EntityCollection<Order> Orders { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies OrderMetadata as the class
    // that carries additional metadata for the Order class.
    [MetadataTypeAttribute(typeof(Order.OrderMetadata))]
    public partial class Order
    {

        // This class allows you to attach custom attributes to properties
        // of the Order class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class OrderMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private OrderMetadata()
            {
            }

            [Include]
            public Customer Customer { get; set; }

            public int CustomerId { get; set; }

            public Nullable<decimal> Freight { get; set; }

            public DateTime OrderDate { get; set; }

            public EntityCollection<OrderDetail> OrderDetails { get; set; }

            public int OrderId { get; set; }

            public Nullable<DateTime> RequiredDate { get; set; }

            public Nullable<DateTime> ShippedDate { get; set; }

            public byte[] Version { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies OrderDetailMetadata as the class
    // that carries additional metadata for the OrderDetail class.
    [MetadataTypeAttribute(typeof(OrderDetail.OrderDetailMetadata))]
    public partial class OrderDetail
    {

        // This class allows you to attach custom attributes to properties
        // of the OrderDetail class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class OrderDetailMetadata
        {
            // Metadata classes are not meant to be instantiated.
            private OrderDetailMetadata()
            {
            }

            public double Discount { get; set; }

            public Order Order { get; set; }

            public int OrderId { get; set; }

            [Include]
            public Product Product { get; set; }

            public int ProductId { get; set; }

            public int Quantity { get; set; }

            public decimal UnitPrice { get; set; }

            public byte[] Version { get; set; }
        }
    }

    // The MetadataTypeAttribute identifies ProductMetadata as the class
    // that carries additional metadata for the Product class.
    [MetadataTypeAttribute(typeof(Product.ProductMetadata))]
    public partial class Product
    {

        // This class allows you to attach custom attributes to properties
        // of the Product class.
        //
        // For example, the following marks the Xyz property as a
        // required property and specifies the format for valid values:
        //    [Required]
        //    [RegularExpression("[A-Z][A-Za-z0-9]*")]
        //    [StringLength(32)]
        //    public string Xyz { get; set; }
        internal sealed class ProductMetadata
        {

            // Metadata classes are not meant to be instantiated.
            private ProductMetadata()
            {
            }

            public Category Category { get; set; }

            public int CategoryId { get; set; }

            public EntityCollection<OrderDetail> OrderDetails { get; set; }

            public int ProductId { get; set; }

            public string ProductName { get; set; }

            public decimal UnitPrice { get; set; }

            public int UnitsInStock { get; set; }

            public byte[] Version { get; set; }

            public string Weight { get; set; }
        }
    }
}
