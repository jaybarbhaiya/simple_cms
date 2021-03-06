﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using simple_cms.Models;

namespace simple_cms.Controllers
{
    public class PageController : Controller
    {
        private CMSContext db = new CMSContext();

        // GET: Page
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Pages.ToList());
        }

        // GET: Page/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // GET: Page/Create
        [Authorize]
        public ActionResult Create(int? id)
        {
            Page page = new Page();
            page.SubjectId = id.Value;
            return View(page);
        }

        // POST: Page/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PageId,SubjectId,PageName,PagePosition,PageVisible,PageContent")] Page page)
        {
            if (ModelState.IsValid)
            {
                db.Pages.Add(page);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(page);
        }

        // GET: Page/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // POST: Page/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PageId,SubjectId,PageName,PagePosition,PageVisible,PageContent")] Page page)
        {
            if (ModelState.IsValid)
            {
                db.Entry(page).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(page);
        }

        // GET: Page/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // POST: Page/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Page page = db.Pages.Find(id);
            db.Pages.Remove(page);
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

        [Authorize]
        public ActionResult RelatedPages(int? id)
        {
            PublicData pd = new PublicData();
            pd.subject = db.Subjects.Find(id);
            pd.pg = new List<Page>();
            List<Page> page = db.Pages.ToList();
            pd.pageObj = new Page();
            pd.pageObj.SubjectId = id.Value;
            if (pd.subject.SubjectId == pd.pageObj.SubjectId)
            {
                foreach (var p in page)
                {
                    if (pd.subject.SubjectId == p.SubjectId)
                    {
                        pd.pg.Add(p);
                        pd.pageObj = p;
                    }
                }
            }
            return View(pd);
        }
    }
}
