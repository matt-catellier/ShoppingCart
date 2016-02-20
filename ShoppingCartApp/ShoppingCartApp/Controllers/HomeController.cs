using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ShoppingCartApp.Repositories;
using ShoppingCartApp.Models;
using ShoppingCartApp.BusinessLogic;
using PagedList;

namespace ShoppingCartApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(bool? cancelOrder, string sortOrder, string searchString, string currentFilter, int? page)
        {
            if(searchString != null)
            {
                page = 1;
            } else
            {
                searchString = currentFilter;
            }

            ProductVMRepo productRepo = new ProductVMRepo();
            IEnumerable<ProductVM> products = productRepo.GetAll(sortOrder,searchString);

            // Store current sort filter parameter.
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentFilter = searchString;
            ViewBag.SearchResults = products.Count();

            ViewBag.Product_ASC = ProductVMRepo.PRODUCT;
            ViewBag.Product_DESC = ProductVMRepo.PRODUCT_DESC;
            ViewBag.Price_ASC = ProductVMRepo.PRICE;
            ViewBag.Price_DESC = ProductVMRepo.PRICE_DESC;

            if (cancelOrder != null)
            {
                // clear cart and end session... 
                string sessionID = System.Web.HttpContext.Current.Session.SessionID;
                SessionHelper.RemoveCurrentSessionData(sessionID);
                // start again so can keep shopping
                SessionHelper.StoreNewSessionData();
            }

            const int PAGE_SIZE = 6;
            int pageNumber = (page ?? 1);
            products = products.ToPagedList(pageNumber, PAGE_SIZE);

            return View(products);
        }

        [HttpGet]
        public ActionResult Add(int? productID)
        {
            // then didnt come from products page
            if(productID == null)
            {
                return RedirectToAction("Index"); // send them there
            }                  
            // used to send current qunatity to the page
            string sessionID = System.Web.HttpContext.Current.Session.SessionID;
            ShoppingCartRepo cartRepo = new ShoppingCartRepo();
            ProductVisit cartItem = cartRepo.GetCartItem(sessionID, (int)productID);

            ProductVM productVM;
            if (cartItem == null)
            {
                ProductRepo pRepo = new ProductRepo();
                Product product = pRepo.GetProduct(productID);

                productVM = new ProductVM(product);
                productVM.SetTotalCost(); // quntity times price  
                productVM.image = product.Image.imageTitle;
                productVM.quantity = 1;
            }
            else
            {
                productVM = new ProductVM(cartItem.Product);
                productVM.SetTotalCost(); // quntity times price  
                productVM.image = cartItem.Product.Image.imageTitle;
                productVM.quantity = cartItem.qtyOrdered;
            }

            return View(productVM);
        }

        [HttpPost]
        public ActionResult Add(ProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                return View(productVM);
            } 

            string sessionID = System.Web.HttpContext.Current.Session.SessionID;
            Product product = productVM.CreateProductEntity();
            int? quantity = productVM.quantity;


            ShoppingCartRepo cartRepo = new ShoppingCartRepo();
            cartRepo.AddCartItem(sessionID, product, quantity);
            ViewBag.Quantity = quantity;

            // Session.Timeout = 1; // necessary or does i automatically do this?
            return RedirectToAction("ViewCart");
        }

        //  should pass in a nullable cart items?
        [HttpGet]
        public ActionResult ViewCart(int? id)
        {
            string sessionID = System.Web.HttpContext.Current.Session.SessionID;
            if (id != null)
            {
                // remove product visit
                ProductVisitRepo productVisitRepo = new ProductVisitRepo();
                productVisitRepo.RemoveProductVisit(sessionID, (int)id);
            }
            
            ShoppingCartRepo cartRepo = new ShoppingCartRepo();
            // get all product visit entries
            IEnumerable<ProductVisit> productVisits = cartRepo.GetCartItems(sessionID);

            // if have product visit can create a cart item out of it via the naviagtion properties
            List<ProductVM> products = new List<ProductVM>();
            foreach(ProductVisit item in productVisits)
            {
                ProductVM product = new ProductVM(item.Product, (int)item.qtyOrdered);
                product.SetTotalCost();
                product.image = item.Product.Image.imageTitle;
                products.Add(product);
            }

            ShoppingCartVM cart = new ShoppingCartVM(products);

            if (products.Count() > 0)
            {
                return View(cart);
            }
            else
            {
                ViewBag.Message = "No cart items selected";
                return View(cart);
            }
        }

        [HttpPost]
        public ActionResult ViewCart(ShoppingCartVM cart)
        {
            if(!ModelState.IsValid)
            {
                return View(cart);
            }

            string sessionID = System.Web.HttpContext.Current.Session.SessionID;    
            ShoppingCartRepo cartRepo = new ShoppingCartRepo();
            foreach (ProductVM productVM in cart.CartItems)
            {
                // update record in database
                Product product = productVM.CreateProductEntity();
                cartRepo.AddCartItem(sessionID, product, productVM.quantity);

                // calculate new product totals
                productVM.SetTotalCost();
            }
            // calcualte new cart totals
            cart.SubTotal = cart.CalculateSubTotal();
            cart.Tax = cart.CalculateTax();
            cart.Total = cart.CalulateTotal();
            return View(cart);
        }
    }
}