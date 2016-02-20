using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingCartApp.Repositories
{
    public class VisitRepo
    {
        public IEnumerable<Visit> GetAll()
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();
            var visits = db.Visits.ToList();
            return visits;
        }

        public Visit GetVisit(string sessionID)
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();

            Visit visit = db.Visits.Where(v => v.sessionID == sessionID).FirstOrDefault();
            return visit;
        }

        public Visit CreateVisit(string sessionID)
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();

            Visit visit = new Visit();
            visit.sessionID = sessionID;
            visit.started = DateTime.Now;

            db.Visits.Add(visit);
            db.SaveChanges();
            return visit;
        }

        public void CreateExpiredVisit(string sessionID)
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();

            Visit visit = new Visit();
            visit.sessionID = sessionID + "-expired";
            visit.started = DateTime.Now.AddHours(-2);

            db.Visits.Add(visit);
            db.SaveChanges();
        }

        public void RemoveVisit(string sessionID)
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();
            Visit visit = db.Visits.Where(v => v.sessionID == sessionID).FirstOrDefault();

            if (visit != null)
            {
                db.Visits.Remove(visit);
                db.SaveChanges();
            }
        }

        public void RemoveVisits()
        {
            MC_ShoppingCartEntities db = new MC_ShoppingCartEntities();
            IEnumerable<Visit> vs = db.Visits.ToList();

            foreach (Visit v in vs)
            {
                if (v.started.Value < DateTime.Now.AddHours(-1))
                {
                    db.Visits.Remove(v);
                }
            }
            db.SaveChanges();
        }

    }
}