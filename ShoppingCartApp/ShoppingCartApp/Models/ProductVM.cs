using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShoppingCartApp.Models
{
    public class ProductVM
    {
        [DisplayName("Product ID")]
        public int productID { get; set; }
        [DisplayName("Product")]
        public string productName { get; set; }
        [DisplayName("Price:")]
        public Nullable<decimal> price { get; set; }
        [DisplayName("Image")]
        public string image { get; set; }
        [DisplayName("Quantity:")]
        [Required(ErrorMessage = "Please enter a quantity")]
        [Range(1, int.MaxValue, ErrorMessage = "Must be greater than 0")]
        public Nullable<int> quantity { get; set; }
        [DisplayName("Total Cost")]
        public Nullable<decimal> totalCost { get; set; }

        public ProductVM() { }
        public ProductVM(Product product)
        {
            this.productID = product.productID;
            this.productName = product.productName;
            this.price = product.price;
            this.quantity = 1; // 1 by default causing issues... :(
        }
        public ProductVM(Product product, int quantity)
        {
            this.productID = product.productID;
            this.productName = product.productName;
            this.price = product.price;
            this.quantity = quantity; // 1 by default causing issues... :(
        }

        public Product CreateProductEntity()
        {
            Product product = new Product();
            product.productID = this.productID;
            product.productName = this.productName;
            product.price = this.price;

            return product;
        }

        public void SetTotalCost()
        {
            this.totalCost = this.price * this.quantity;
        }

        public void SetImage(string title)
        {
            string imagePath = "~/Content/Images/" + title;
            this.image = imagePath;
        }
    }
}