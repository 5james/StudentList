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
    public class JsonGroupsGetResponse
    {
        public string errormsg;
        public Groups[] groupslist;
    }
    public class GroupsController : ApiController
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
                var groups = from g in storage.GetGroups()
                             select new Groups
                             {
                                 IDGroup = g.IDGroup,
                                 Name = g.Name,
                                 Stamp = g.Stamp
                             };

                return Json(new JsonGroupsGetResponse { errormsg = errMsg, groupslist = groups.ToArray() });
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                responseMsg.Content = new StringContent("Wewnętrzny błąd strony. Proszę spróbować ponownie później.");
                return ResponseMessage(responseMsg) as IHttpActionResult;
            }
        }


        // GET: api/Groups
        public IHttpActionResult Get()
        {
            return DefaultResponse();
        }

        // POST: api/Students
        public IHttpActionResult Post([FromBody()] Groups groups, [FromUri()] string operation)
        {
            string msg = null;
            try
            {
                if (operation == "PUT")
                {
                    storage.UpdateGroup(groups);
                }
                else if (operation == "POST")
                {
                    storage.CreateGroup(groups.Name);
                }
               
                else if (operation == "DELETE")
                {
                    storage.DeleteGroup(groups);
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
        //// GET: api/Groups
        //public IQueryable<Groups> GetGroups()
        //{
        //    return db.Groups;
        //}

        //// GET: api/Groups/5
        //[ResponseType(typeof(Groups))]
        //public IHttpActionResult GetGroups(int id)
        //{
        //    Groups groups = db.Groups.Find(id);
        //    if (groups == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(groups);
        //}

        //// PUT: api/Groups/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutGroups(int id, Groups groups)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != groups.IDGroup)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(groups).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GroupsExists(id))
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

        //// POST: api/Groups
        //[ResponseType(typeof(Groups))]
        //public IHttpActionResult PostGroups(Groups groups)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Groups.Add(groups);
        //    db.SaveChanges();

        //    return CreatedAtRoute("DefaultApi", new { id = groups.IDGroup }, groups);
        //}

        //// DELETE: api/Groups/5
        //[ResponseType(typeof(Groups))]
        //public IHttpActionResult DeleteGroups(int id)
        //{
        //    Groups groups = db.Groups.Find(id);
        //    if (groups == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Groups.Remove(groups);
        //    db.SaveChanges();

        //    return Ok(groups);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool GroupsExists(int id)
        //{
        //    return db.Groups.Count(e => e.IDGroup == id) > 0;
        //}
    }
}