using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FurryFriendRnR.DATA.EF;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;

namespace FurryFriendRnR.UI.Controllers
{
    public class ReservationsController : Controller
    {
        private FurryFriendRnREntities db = new FurryFriendRnREntities();

        // GET: Reservations
        [Authorize]
        public ViewResult Index(string searchString, string currentFilter, int page = 1)
        {
            int pageSize = 3;

            var reserve = db.Reservations.ToList();
            string currentUserID = User.Identity.GetUserId();
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFiler = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
                    reserve = (from r in reserve
                               where (r.Location.LocationName.ToString().ToLower().Contains(searchString.ToLower()) || r.ReservationDate.ToString().Contains(searchString))
                               select r).ToList();
                }
                else
                {
                    reserve = (from r in reserve
                               where r.OwnerAsset.OwnerId == currentUserID && (r.Location.LocationName.ToString().ToLower().Contains(searchString.ToLower()) || r.ReservationDate.ToString().Contains(searchString))
                               select r).ToList();
                }
                return View(reserve.ToPagedList(page,pageSize));
            }
            else
            {
                if (User.IsInRole("Admin") || User.IsInRole("Employee"))
                {
                    reserve = (from r in reserve
                               select r).ToList();
                }
                else
                {
                    reserve = (from r in reserve
                               where r.OwnerAsset.OwnerId == currentUserID
                               select r).ToList();
                }
                ViewBag.currentFilter = searchString;
                return View(reserve.ToPagedList(page,pageSize));
            }
            
        }

        // GET: Reservations/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        [Authorize(Roles = "Admin,Family")]
        public ActionResult Create()
        {
            string currentUserID = User.Identity.GetUserId();
            if (User.IsInRole("Family"))
            {
                ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName");
                ViewBag.OwnerAssetId = new SelectList(db.OwnerAssets.Where(x => x.UserDetail.UserId == currentUserID), "OwnerAssetId", "PetName");
                return View();

            }
            else
            {
                ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName");
                ViewBag.OwnerAssetId = new SelectList(db.OwnerAssets, "OwnerAssetId", "PetName");
                return View();
            }
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Family")]
        public ActionResult Create([Bind(Include = "ReservationId,OwnerAssetId,LocationId,ReservationDate")] Reservation reservation)
        {
            
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", reservation.LocationId);
            ViewBag.OwnerAssetId = new SelectList(db.OwnerAssets, "OwnerAssetId", "PetName", reservation.OwnerAssetId);


            if (ModelState.IsValid)
            {
                if (User.IsInRole("Family") || User.IsInRole("Admin"))
                {


                    int reservationLimit = 0;
                    reservationLimit = db.Reservations.Where(x => x.ReservationDate == reservation.ReservationDate && x.LocationId == reservation.LocationId).Count();

                    if (reservationLimit < db.Locations.Find(reservation.LocationId).ReservationLimit)
                    {
                        db.Reservations.Add(reservation);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.ClassIsFullMessage = "Sorry, the selected date has no openings. Please select another date.";
                    }
                }
                else
                {
                    db.Reservations.Add(reservation);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(reservation);
        }

        // GET: Reservations/Edit/5
        [Authorize(Roles = "Admin,Family")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", reservation.LocationId);
            ViewBag.OwnerAssetId = new SelectList(db.OwnerAssets, "OwnerAssetId", "PetName", reservation.OwnerAssetId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservationId,OwnerAssetId,LocationId,ReservationDate")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", reservation.LocationId);
            ViewBag.OwnerAssetId = new SelectList(db.OwnerAssets, "OwnerAssetId", "PetName", reservation.OwnerAssetId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "Admin,Family")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
