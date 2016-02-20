using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCartApp.Repositories
{
    public class ProductVisitRepo
    { 
        // used to get all cart items in DB regardless of session
        public IEnumerable<ProductVisit> GetAll()
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();
            IEnumerable<ProductVisit> pvs = db.ProductVisits.ToList();
            return pvs;
        }

        // used to get a cart item from DB, not in use
        public ProductVisit GetProductVisit(string sessionID)
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();
            ProductVisit sessionData = db.ProductVisits.Where(pv => pv.sessionID == sessionID).FirstOrDefault();
            return sessionData;
        }

        // used for testing
        public ProductVisit CreateProductVisit(string sessionID)
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();

            ProductVisit pv = new ProductVisit();
            pv.sessionID = sessionID; // primary
            pv.productID = 107; // primary
            pv.updated = DateTime.Now;

            db.ProductVisits.Add(pv);
            db.SaveChanges();
            return pv;
        }
        // used for testing
        public void CreateExpiredProductVisit(string sessionID)
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();

            ProductVisit pv = new ProductVisit();
            pv.sessionID = sessionID + "-expired"; // primary
            pv.productID = 107; // primary
            pv.updated = DateTime.Now.AddHours(-2);

            db.ProductVisits.Add(pv);
            db.SaveChanges();
        }

        // used when click remove button
        // need to pass session and product ID
        // this is causing error when called from session start and there is not any data yet
        public void RemoveProductVisit(string sessionID, int productID)
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();

            ProductVisit productVisit = db.ProductVisits.Where(pv => pv.sessionID == sessionID && pv.productID == productID).FirstOrDefault();
            if (productVisit != null)
            {
                db.ProductVisits.Remove(productVisit);
                db.SaveChanges();
            }
        }
        // this is causing error when called from session start and there is not any data yet
        // used by cacnel order button only called when there is product visits
        public void RemoveProductVisits(string sessionID)
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();
            IEnumerable<ProductVisit> pvs = db.ProductVisits.Where(pv => pv.sessionID == sessionID).ToList();

            if (pvs != null) // if any entries in product visit
            {
                foreach (ProductVisit pv in pvs)
                {
                    db.ProductVisits.Remove(pv); // remove ones with current sessioID
                }
                db.SaveChanges();
            }
        }
        // used in session start
        public void RemoveExpiredProductVisits()
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();
            IEnumerable<ProductVisit> pvs = db.ProductVisits.ToList();

            if(pvs != null) // if there are old product visits 
            {
                foreach (ProductVisit pv in pvs)
                {
                    if (pv.updated.Value < DateTime.Now.AddHours(-1)) // remove the old ones
                    {
                        db.ProductVisits.Remove(pv);
                    }
                }
                db.SaveChanges();
            }

            
        }
    }
}
