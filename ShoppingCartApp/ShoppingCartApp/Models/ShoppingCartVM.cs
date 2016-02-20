using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ShoppingCartApp.Models
{
    public class ShoppingCartVM
    {
        public List<ProductVM> CartItems { get; set; }
        [DisplayName("Sub-total:")]
        public decimal SubTotal { get; set; }
        [DisplayName("+ 7% Tax:")]
        public decimal Tax { get; set; }
        [DisplayName("Total:")]
        public decimal Total { get; set; }

        public ShoppingCartVM() {}
        public ShoppingCartVM(List<ProductVM> cartItems) {
            this.CartItems = cartItems;
            this.SubTotal = CalculateSubTotal();
            this.Tax = CalculateTax();
            this.Total = CalulateTotal();
            // CalculateItemCost(); // will update the total cost field of VM
        }

        public decimal CalculateSubTotal()
        {
            decimal? total = 0.00m;
            foreach(var item in this.CartItems)
            {
                total += item.totalCost;
            }
            total = Math.Round((decimal)total, 2);
            return (decimal)total;
        }

        public decimal CalculateTax()
        {
            decimal tax = this.SubTotal * 0.07m;
            tax = Math.Round(tax, 2);
            return tax;
        }

        public decimal CalulateTotal()
        {
            decimal total = this.SubTotal + this.Tax;
            total = Math.Round(total, 2);
            return total;
        }       
    }
}