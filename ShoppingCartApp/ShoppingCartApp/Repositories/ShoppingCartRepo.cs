using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCartApp.Repositories
{
    public class ShoppingCartRepo
    {
        // FOR CREATING AND UPDATING SHOPPING CART ITEMS
        public void AddCartItem(string sessionID, Product product, int? quantity)
        {
            // need to update the expiry date of the session aswell?
            // maybe sliding does this by default
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();

            ProductVisit item = db.ProductVisits
                                    .Where(pv => pv.productID == product.productID && pv.sessionID == sessionID)
                                        .FirstOrDefault();

            if (item == null) // if null make one
            {
                ProductVisit cartItem = new ProductVisit();
                cartItem.sessionID = sessionID; // primary
                cartItem.productID = product.productID; // primary
                cartItem.qtyOrdered = quantity;
                cartItem.updated = DateTime.Now;

                db.ProductVisits.Add(cartItem);
                db.SaveChanges();
            }
            else // otherwise update quantity and updated
            {
                
                item.qtyOrdered = quantity;
                item.updated = DateTime.Now;                
                db.SaveChanges();
            }
        }

       
        public ProductVisit GetCartItem(string sessionID, int productID)
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();
            ProductVisit cartItem = db.ProductVisits
                                                    .Where(pv => pv.sessionID == sessionID && pv.productID == productID)
                                                        .FirstOrDefault();
            return cartItem;
        }
        /// <summary>
        ///  Returns a list of items product visits with  a certain sessionID
        /// </summary>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        public IEnumerable<ProductVisit> GetCartItems(string sessionID)
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();
            // get all cart items from the database
            IEnumerable<ProductVisit> carItems = db.ProductVisits.Where(pv => pv.sessionID == sessionID).Select(pv => pv);

            // how to get price aswell?
            return carItems;
        }
    }
}