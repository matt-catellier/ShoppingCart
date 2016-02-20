using ShoppingCartApp.Repositories;

namespace ShoppingCartApp.BusinessLogic
{
    public static class SessionHelper
    {
        private static ProductVisitRepo productVistRepo = new ProductVisitRepo(); // interacts with DB
        private static VisitRepo visitRepo = new VisitRepo();

        // used in session start to create a visit entry
        public static void StoreNewSessionData()
        {
            string sessionID = System.Web.HttpContext.Current.Session.SessionID;
            var session = visitRepo.GetVisit(sessionID);
            // if not session not in database than add it 
            if (session == null)
            {
                Visit visit = visitRepo.CreateVisit(sessionID);
                // visitRepo.CreateExpiredVisit(sessionID);
            }
        }

        // used in session end
        public static void RemoveAbandonedSessionsData()
        {
            productVistRepo.RemoveExpiredProductVisits();
            visitRepo.RemoveVisits();
        }
        // used in session end and when click cancel button
        public static void RemoveCurrentSessionData(string sessionID)
        {
            productVistRepo.RemoveProductVisits(sessionID);
            visitRepo.RemoveVisit(sessionID);
        }
    }
}