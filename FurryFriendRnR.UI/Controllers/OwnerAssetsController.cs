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

namespace FurryFriendRnR.UI.Controllers
{
    public class OwnerAssetsController : Controller
    {
        private FurryFriendRnREntities db = new FurryFriendRnREntities();

        // GET: OwnerAssets
        [Authorize]
        public ActionResult Index()
        {
            string currentUserID = User.Identity.GetUserId();
            if (User.IsInRole("Admin") || User.IsInRole("Employee"))
            {
                var ownerAssets = db.OwnerAssets.Include(o => o.UserDetail);
                return View(ownerAssets.ToList());
            }
            else
            {
                var ownerAssets = db.OwnerAssets.Where(x => x.OwnerId == currentUserID).Include(a => a.UserDetail);
                return View(ownerAssets.ToList());
            }

            
        }

        // GET: OwnerAssets/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);
            if (ownerAsset == null)
            {
                return HttpNotFound();
            }
            return View(ownerAsset);
        }

        // GET: OwnerAssets/Create
        [Authorize(Roles = "Admin,Family")]
        public ActionResult Create()
        {
            ViewBag.OwnerId = new SelectList(db.UserDetails, "UserId", "FirstName");
            return View();
        }

        // POST: OwnerAssets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OwnerAssetId,PetName,OwnerId,AssetPhoto,SpecialNotes,IsActive,DateAdded,PetBreed,RecordFile,PetBirthdate,Weight,Color,IsFixed,PetGender")]
        OwnerAsset ownerAsset, HttpPostedFileBase assetPhoto)
        {
            if (ModelState.IsValid)
            {
                #region File Upload
                string imgName = "noimg.jpg";
                if (assetPhoto != null)
                {
                    imgName = assetPhoto.FileName;
                    string ext = imgName.Substring(imgName.LastIndexOf('.'));
                    string[] goodExts = { ".jpeg", ".jpg", ".gif", ".png" };
                    if (goodExts.Contains(ext.ToLower()) && (assetPhoto.ContentLength <= 4194304))
                    {
                        imgName = Guid.NewGuid() + ext;
                        assetPhoto.SaveAs(Server.MapPath("~/Content/images/" + imgName));
                    }
                    else
                    {
                        imgName = "noimg.jpg";
                    }
                }

                ownerAsset.AssetPhoto = imgName;
                #endregion

                ownerAsset.OwnerId = User.Identity.GetUserId();
                db.OwnerAssets.Add(ownerAsset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerId = new SelectList(db.UserDetails, "UserId", "FirstName", ownerAsset.OwnerId);
            return View(ownerAsset);
        }

        // GET: OwnerAssets/Edit/5
        [Authorize(Roles = "Admin,Family")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);
            if (ownerAsset == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerId = new SelectList(db.UserDetails, "UserId", "FirstName", ownerAsset.OwnerId);
            return View(ownerAsset);
        }

        // POST: OwnerAssets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OwnerAssetId,PetName,OwnerId,AssetPhoto,SpecialNotes,IsActive,DateAdded,PetBreed,RecordFile,PetBirthdate,Weight,Color,IsFixed,PetGender")] OwnerAsset ownerAsset, HttpPostedFileBase assetPhoto)
        {
            if (ModelState.IsValid)
            {
                #region File Upload
                if (assetPhoto != null)
                {
                    string imgName = assetPhoto.FileName;
                    string ext = imgName.Substring(imgName.LastIndexOf("."));
                    string[] goodExts = { ".jpeg", ".jpg", ".gif", ".png" };
                    if (goodExts.Contains(ext.ToLower()) && (assetPhoto.ContentLength <= 4194304))
                    {
                        imgName = Guid.NewGuid() + ext;
                        assetPhoto.SaveAs(Server.MapPath("~/Content/images/" + imgName));
                        if (ownerAsset.AssetPhoto != null && ownerAsset.AssetPhoto != "noimg.jpg")
                        {
                            System.IO.File.Delete(Server.MapPath("~/Content/images/" + Session["currentImage"].ToString()));
                        }
                        ownerAsset.AssetPhoto = imgName;
                    }
                }          
                
                #endregion

                db.Entry(ownerAsset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ownerAsset);
        }

        // GET: OwnerAssets/Delete/5
        [Authorize(Roles = "Admin,Family")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);
            if (ownerAsset == null)
            {
                return HttpNotFound();
            }
            return View(ownerAsset);
        }

        // POST: OwnerAssets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);

            if (ownerAsset.AssetPhoto != null && ownerAsset.AssetPhoto != "noimg.jpg")
            {
                System.IO.File.Delete(Server.MapPath("~/Content/images/" + Session["currentImage"].ToString()));
            }

            db.OwnerAssets.Remove(ownerAsset);
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
