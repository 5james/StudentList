using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using StudentListJS;
using StudentListJS.Models;

namespace StudentListJS.Controllers
{

    public class JsonStudentsGetResponse
    {
        public string errormsg;
        public Students[] studentslist;
        public Groups[] groupslist;
    }
    public class StudentsController : ApiController
    {
        //private StorageContext db = new StorageContext();
        private Storage storage = new Storage();
        public virtual Storage Storage
        {
            get { return storage; }
            set { storage = value; }
        }
        ILogger logger = new Logger(typeof(Storage));

        private IHttpActionResult DefaultResponse(string errMsg = null)
        {
            try
            {
                var students = from s in storage.GetStudents()
                               select new Students
                               {
                                   BirthDate = s.BirthDate,
                                   BirthPlace = s.BirthPlace,
                                   FirstName = s.FirstName,
                                   IDGroup = s.IDGroup,
                                   IDStudent = s.IDStudent,
                                   IndexNo = s.IndexNo,
                                   LastName = s.LastName,
                                   Stamp = s.Stamp
                               };

                var groups = from g in storage.GetGroups()
                             select new Groups
                             {
                                 IDGroup = g.IDGroup,
                                 Name = g.Name,
                                 Stamp = g.Stamp
                             };

                return Json(new JsonStudentsGetResponse { errormsg = errMsg, studentslist = students.ToArray(), groupslist = groups.ToArray() });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                responseMsg.Content = new StringContent("Wewnętrzny błąd strony. Proszę spróbować ponownie później.");
                return ResponseMessage(responseMsg) as IHttpActionResult;
            }
        }


        // GET: api/Students
        public IHttpActionResult Get()
        {
            return DefaultResponse();
        }

        // POST: api/Students
        public IHttpActionResult Post([FromBody()] Students student, [FromUri()] string operation)
        {
            string msg = null;
            try
            {
                if (operation == "PUT")
                {
                    storage.UpdateStudent(student);
                }
                else if (operation == "POST")
                {
                    storage.CreateStudent(student.FirstName, student.LastName, student.IndexNo, student.IDGroup, student.BirthPlace, student.BirthDate);
                }

                else if (operation == "DELETE")
                {
                    storage.DeleteStudent(student);
                }
                else
                {
                    msg = "Wewnętrzny błąd strony. Proszę spróbować ponownie.";
                }
            }
            catch (DBConcurrencyException cex)
            {
                logger.LogError(" - " + operation + " - " + cex.Message);
                msg = cex.Message;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                msg = ex.Message;
            }
            return DefaultResponse(msg);
        }


        //// GET: api/Students
        //public IQueryable<Students> GetStudents()
        //{
        //    return db.Students;
        //}

        //// GET: api/Students/5
        //[ResponseType(typeof(Students))]
        //public IHttpActionResult GetStudents(int id)
        //{
        //    Students students = db.Students.Find(id);
        //    if (students == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(students);
        //}

        //// PUT: api/Students/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutStudents(int id, Students students)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != students.IDStudent)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(students).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!StudentsExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Students
        //[ResponseType(typeof(Students))]
        //public IHttpActionResult PostStudents(Students students)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Students.Add(students);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = students.IDStudent }, students);
        //}

        //// DELETE: api/Students/5
        //[ResponseType(typeof(Students))]
        //public IHttpActionResult DeleteStudents(int id)
        //{
        //    Students students = db.Students.Find(id);
        //    if (students == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Students.Remove(students);
        //    db.SaveChanges();

        //    return Ok(students);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool StudentsExists(int id)
        //{
        //    return db.Students.Count(e => e.IDStudent == id) > 0;
        //}
    }
}