using ShoppingCartApp.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShoppingCartApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        void Session_Start(object sender, EventArgs e)
        {
            SessionHelper.RemoveAbandonedSessionsData(); //remove xpired sessions
            string sessionID = System.Web.HttpContext.Current.Session.SessionID;
            SessionHelper.RemoveCurrentSessionData(sessionID); // remove any data with current sessionID
            SessionHelper.StoreNewSessionData();
            // SessionHelper.StoreNewSessionData(); // should maybe call this on the homepage
        }
        // session end seems to never be called
        void Session_End(object sender, EventArgs e)
        {
            string sessionID = Session.SessionID;
            SessionHelper.RemoveCurrentSessionData(sessionID);
        }
    }
}
