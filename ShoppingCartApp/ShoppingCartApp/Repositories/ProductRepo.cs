using System.Collections.Generic;
using System.Linq;

namespace ShoppingCartApp.Repositories
{
    // not being used currently
    public class ProductRepo
    {
        public IEnumerable<Product> GetAll()
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();

            var products = db.Products.ToList();
            return products;
        }

        public Product GetProduct(int? id)
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();

            var product = db.Products.Where(p => p.productID == id).FirstOrDefault();
            return product;
        }
    }
}