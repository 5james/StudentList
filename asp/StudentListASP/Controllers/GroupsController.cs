using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentListASP;
using StudentListASP.Models;
using System.Configuration;
using PagedList;

namespace StudentListASP.Controllers
{
    public class GroupsController : Controller
    {
        private Storage storage = new Storage();
        public virtual Storage Storage
        {
            get
            {
                return storage;
            }
            set
            {
                storage = value;
            }
        }
        private int groupsPerPage = Int32.Parse(ConfigurationManager.AppSettings["GroupsPerPage"]);

        // GET: PagedGroups
        public ActionResult Index(int? page, string errMsg)
        {
            try
            {
                int currentPage = (page ?? 1);

                var storageGroupList = storage.GetGroups();

                return View(new GroupsList() { ErrMsg = errMsg, PagedGroups = storageGroupList.ToPagedList(currentPage == 0 ? 1 : currentPage, groupsPerPage) });
            }
            catch (Exception ex)
            {
                return View(new GroupsList() { ErrMsg = ex.Message, PagedGroups = storage.GetGroups().ToPagedList(1, groupsPerPage) });
            }
        }



        // GET: PagedGroups/Details/5
        public ActionResult FilterGroups(GroupsList groupList, string request)
        {
            try { 

            if (request.Equals("Create"))
            {
                    storage.CreateGroup(groupList.CurrentGroup.Name);
            }
            else if (request.Equals("Edit"))
            {
                storage.UpdateGroup(groupList.CurrentGroup);
            }
            else if (request.Equals("Delete"))
            {
                storage.DeleteGroup(groupList.CurrentGroup);
            }
            return RedirectToAction("Index", new { page = groupList.Page });
        }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { page = 1, errMsg = ex.Message} );
            }
        }

        //// GET: PagedGroups/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: PagedGroups/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "IDGroup,Name,Stamp")] PagedGroups groups)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.PagedGroups.Add(groups);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(groups);
        //}

        //// GET: PagedGroups/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PagedGroups groups = db.PagedGroups.Find(id);
        //    if (groups == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(groups);
        //}

        //// POST: PagedGroups/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "IDGroup,Name,Stamp")] PagedGroups groups)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(groups).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(groups);
        //}

        //// GET: PagedGroups/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PagedGroups groups = db.PagedGroups.Find(id);
        //    if (groups == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(groups);
        //}

        //// POST: PagedGroups/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    PagedGroups groups = db.PagedGroups.Find(id);
        //    db.PagedGroups.Remove(groups);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
