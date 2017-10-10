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
using System.Web.Configuration;

namespace StudentListASP.Controllers
{
    public class StudentsController : Controller
    {
        private Storage storage = new Storage();
        public virtual Storage Storage
        {
            get { return storage; }
            set { storage = value; }
        }
        private int studentsPerPage = Int32.Parse(ConfigurationManager.AppSettings["StudentsPerPage"]);
        ILogger logger = new Logger(typeof(StudentsController));
        // GET: Students
        public ActionResult Index(int? page, string cityFilter, string groupFilter, string errMsg, string request)
        {
            int currentPage = (page ?? 1);
            var storageStudList = storage.GetStudents();
            var storageGroupList = storage.GetGroups();

            if (!String.IsNullOrEmpty(request))
            {
                if (request.Equals("Clear"))
                {
                    groupFilter = "";
                    cityFilter = "";
                }
                else if (request.Equals("Filter"))
                {
                    return FilterStudents(groupFilter, cityFilter);
                }
            }

            return View(new StudentsList { PagedStudents = storageStudList.ToPagedList(currentPage == 0 ? 1 : currentPage, studentsPerPage), Groups = new SelectList(storageGroupList, "IDGroup"), ErrMsg = errMsg });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StudentForms(StudentsList studentsList, string request, string formGroup)
        {
            try
            {
                var student = studentsList.CurrentStudent;
                studentsList.CurrentStudent.IDGroup = storage.GetGroups(formGroup).First().IDGroup;

                if (request.Equals("Create"))
                {
                    storage.CreateStudent(student.FirstName, student.LastName, student.IndexNo, student.IDGroup, student.BirthPlace, student.BirthDate);
                }
                else if (request.Equals("Edit"))
                {
                    storage.UpdateStudent(student);
                }
                else if (request.Equals("Delete"))
                {
                    storage.DeleteStudent(student);
                }
                return RedirectToAction("Index", new { page = studentsList.Page });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { page = 1, errMsg = ex.Message });
            }
        }

        public virtual ActionResult FilterStudents(string groupFilter, string cityFilter)
        {

            if (!String.IsNullOrEmpty(groupFilter) && String.IsNullOrEmpty(cityFilter))
            {
                return View(new StudentsList { PagedStudents = storage.GetStudents().ToPagedList(1, studentsPerPage), Groups = new SelectList(storage.GetGroups(), "IDGroup") });
            }

            List<Students> students = storage.GetStudents();

            if (!String.IsNullOrEmpty(groupFilter))
            {
                if (!String.IsNullOrEmpty(cityFilter))
                {
                    Groups gr = storage.GetGroups(groupFilter).First();
                    students = storage.GetStudents(gr, cityFilter);
                }
                else
                {
                    Groups gr = storage.GetGroups(groupFilter).First();
                    students = storage.GetStudents(gr);
                }
            }
            else
            {
                students = storage.GetStudents(cityFilter);
            }

            return View(new StudentsList { PagedStudents = students.ToPagedList(1, studentsPerPage), Groups = new SelectList(storage.GetGroups(), "IDGroup") });
        }


        //// GET: Students/Details/5
        //public ActionResult Details(int? id)
        //{
        // if (id == null)
        // {
        // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        // }
        // Students students = db.Students.Find(id);
        // if (students == null)
        // {
        // return HttpNotFound();
        // }
        // return View(students);
        //}

        //// GET: Students/Create
        //public ActionResult Create()
        //{
        // ViewBag.IDGroup = new SelectList(db.PagedGroups, "IDGroup", "Name");
        // return View();
        //}

        //// POST: Students/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "IDStudent,FirstName,LastName,IndexNo,BirthDate,BirthPlace,IDGroup,Stamp")] Students students)
        //{
        // if (ModelState.IsValid)
        // {
        // db.Students.Add(students);
        // db.SaveChanges();
        // return RedirectToAction("Index");
        // }

        // ViewBag.IDGroup = new SelectList(db.PagedGroups, "IDGroup", "Name", students.IDGroup);
        // return View(students);
        //}

        //// GET: Students/Edit/5
        //public ActionResult Edit(int? id)
        //{
        // if (id == null)
        // {
        // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        // }
        // Students students = db.Students.Find(id);
        // if (students == null)
        // {
        // return HttpNotFound();
        // }
        // ViewBag.IDGroup = new SelectList(db.PagedGroups, "IDGroup", "Name", students.IDGroup);
        // return View(students);
        //}

        //// POST: Students/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "IDStudent,FirstName,LastName,IndexNo,BirthDate,BirthPlace,IDGroup,Stamp")] Students student)
        //{
        // if (ModelState.IsValid)
        // {
        // db.Entry(student).State = EntityState.Modified;
        // db.SaveChanges();
        // return RedirectToAction("Index");
        // }
        // ViewBag.IDGroup = new SelectList(db.PagedGroups, "IDGroup", "Name", student.IDGroup);
        // return View(student);
        //}

        //// GET: Students/Delete/5
        //public ActionResult Delete(int? id)
        //{
        // if (id == null)
        // {
        // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        // }
        // Students students = db.Students.Find(id);
        // if (students == null)
        // {
        // return HttpNotFound();
        // }
        // return View(students);
        //}

        //// POST: Students/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        // Students students = db.Students.Find(id);
        // db.Students.Remove(students);
        // db.SaveChanges();
        // return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        // if (disposing)
        // {
        // db.Dispose();
        // }
        // base.Dispose(disposing);
        //}
    }
}
