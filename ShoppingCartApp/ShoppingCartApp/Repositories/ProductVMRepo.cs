using ShoppingCartApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCartApp.Repositories
{
    public class ProductVMRepo
    {
        public const string PRODUCT = "Product";
        public const string PRODUCT_DESC = "Product_desc";
        public const string PRICE = "Price";
        public const string PRICE_DESC = "Price_desc";

        public IEnumerable<ProductVM> GetAll(string sortOrder, string searchString)
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();

            IEnumerable<ProductVM> products = db.Products.Select(p => new ProductVM
                                                                {
                                                                    productID = p.productID,
                                                                    productName = p.productName,
                                                                    price = p.price,
                                                                    quantity = null,
                                                                    image = p.Image.imageTitle
                                                                });

            products = SortProducts(products, sortOrder);
            products = FilterProducts(products, searchString);

            return products;
        }

        public ProductVM GetProductVM(int? id)
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();

            ProductVM product = db.Products.Where(p => p.productID == id)
                                                .Select(p => new ProductVM
                                                    {
                                                        productID = p.productID,
                                                        productName = p.productName,
                                                        price = p.price,
                                                        quantity = null
                                                    }).FirstOrDefault();
            return product;
        }

        IEnumerable<ProductVM> SortProducts(IEnumerable<ProductVM> products, string sortOrder)
        {
            switch (sortOrder)
            {
                case PRODUCT_DESC:
                    products = products.OrderByDescending(p => p.productName);
                    break;
                case PRICE:
                    products = products.OrderBy(p => p.price);
                    break;
                case PRICE_DESC:
                    products = products.OrderByDescending(p => p.price);
                    break;
                default: // order by product name
                    products = products.OrderBy(p => p.productName);
                    break;
            }
            return products;
        }

        IEnumerable<ProductVM> FilterProducts(IEnumerable<ProductVM> products, string searchString)
        {
            // Filter results based on search.
            if (!String.IsNullOrEmpty(searchString))
                products = products.Where(
                                        p => p.productName.ToUpper().Contains(searchString.ToUpper()));
            return products;
        }



    }
}