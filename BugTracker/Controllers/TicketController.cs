using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;
using System.Linq;
using System.Linq.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using System.Data.Entity.Validation;

namespace BugTracker.Controllers
{
    //  Test Code for Authorize override for entire Ticket Class - all methods in here
    //  [Authorize(Roles="Administrator,Developer")]
    [Authorize(Roles="Administrator,Developer,Submitter")]
    public class TicketController : Controller
    {
        private BugTrackerEntities db = new BugTrackerEntities();

        // GET: Ticket ViewOnly
    //  Test code for Authorize override that is just for ViewOnly
    //    [Authorize(Roles = "Administrator")]
    //
        [Authorize(Roles = "Administrator,Submitter,Developer")]
        public ActionResult ViewOnly()
        {
            // Below line matches the ViewOnly with the user that signed in so he only sees his Tickets
            var tickets = db.Tickets.Where(x => x.AspNetUser1.UserName == User.Identity.Name).ToList();
            // Jamie helped me put above line in, and we commented out below
            //
            //var tickets = db.Tickets.Include(t => t.AspNetUser).Include(t => t.AspNetUser1).Include(t => t.Project)
            //    .Include(t => t.TicketPriority).Include(t => t.TicketStatu)
            //    .Include(t => t.TicketType).Where(m => m.AssignToUserId == User.Identity.Name); 
            ViewBag.OwnerId = db.AspNetUsers;
            
            return View(tickets.ToList());
        }
        
        // GET: Ticket
       
        [Authorize(Roles = "Administrator,Submitter,Developer")]
        public ActionResult Index(int? page, string search="", string sortOrder="Tickets",bool asc=false)
        {
            ViewBag.Search = search;
            ViewBag.ascending = asc;
            ViewBag.SortOrder = sortOrder;
            ViewBag.ModelSortParm = String.IsNullOrEmpty(sortOrder) ? "Ticket" : "";
            var pageNumber = page ?? 1;
            var query = from q in db.Tickets select q;
            if (!String.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.Title == search);
            }

            switch (sortOrder)
            {
                case "Title":
                    if (asc)
                        query = query.OrderBy(s => s.Title);
                    else
                        query = query.OrderByDescending(s => s.Title);
                    break;
                case "Created":
                    if (asc)
                        query = query.OrderBy(s => s.Created );
                    else
                        query = query.OrderByDescending(s => s.Created);
                    break;
                case "Updated":
                    if (asc)
                        query = query.OrderBy(s => s.Updated);
                    else
                        query = query.OrderByDescending(s => s.Updated);
                    break;
                case "PriorityId":
                    if (asc)
                        query = query.OrderBy(s => s.PriorityId);
                    else
                        query = query.OrderByDescending(s => s.PriorityId);
                    break;
                case "StatusId":
                    if (asc)
                        query = query.OrderBy(s => s.StatusId);
                    else
                        query = query.OrderByDescending(s => s.StatusId);
                    break;
                default:
                    query = query.OrderBy(s => s.Title);
                    break;
            }
            var results = query.ToPagedList(pageNumber, 10);
            return View(results);

        }

        // GET: Ticket/Details/5
        [Authorize(Roles = "Administrator,Submitter,Developer")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Ticket/Comment
        [Authorize(Roles = "Administrator,Submitter,Developer")]
        public ActionResult Comment(int? id)
        {
            /* var com = new AspNetUser(); */
 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
      /*      AspNetUser aspnetu = db.AspNetUsers.Find(id); */
            /*
            if (aspnetu == null)
            {
                    return HttpNotFound();
            }
             */

            TicketComment tc = new TicketComment();
            
            AspNetUser user = db.AspNetUsers.FirstOrDefault(u => u.UserName == User.Identity.Name);
            tc.UserId = user.Id;
            //ViewBag.Comment = new SelectList(db.TicketComments, "Comment", "Comment");
            tc.TicketId = (int)id;
            tc.Created = DateTime.Now;
          
            return View(tc);
        }

        // POST: Comment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Submitter,Developer")]
        public ActionResult Comment(TicketComment ticketcomment)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                //ticket.Id = db.Tickets.Max(x => x.Id) + 1;
                
                
                db.TicketComments.Add(ticketcomment);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
         
            return View(ticketcomment);
        }

        // GET: Ticket/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.OwnerId = new SelectList(db.AspNetUsers, "Id", "FirstName");
            ViewBag.AssignToUserId = new SelectList(db.AspNetUsers, "Id", "FirstName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.PriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.StatusId = new SelectList(db.TicketStatus, "ID", "Name");
            ViewBag.TypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View();
        }
        
        // POST: Ticket/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "OwnerId,Title,Description,Created,Updated,AssignToUserId,ProjectId,TypeId,PriorityId,StatusId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //ticket.Id = db.Tickets.Max(x => x.Id) + 1;
                ticket.Created = DateTime.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            ViewBag.OwnerId = new SelectList(db.AspNetUsers, "Id", "FirstName", ticket.OwnerId);
            ViewBag.AssignToUserId = new SelectList(db.AspNetUsers, "Id", "FirstName", ticket.AssignToUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.PriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.PriorityId);
            ViewBag.StatusId = new SelectList(db.TicketStatus, "ID", "Name", ticket.StatusId);
            ViewBag.TypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TypeId);
            return View(ticket);
        }

        // GET: Ticket/Edit/5
        [Authorize(Roles = "Administrator,Developer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerId = new SelectList(db.AspNetUsers, "Id", "FirstName", ticket.OwnerId);
            ViewBag.AssignToUserId = new SelectList(db.AspNetUsers, "Id", "FirstName", ticket.AssignToUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.PriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.PriorityId);
            ViewBag.StatusId = new SelectList(db.TicketStatus, "ID", "Name", ticket.StatusId);
            ViewBag.TypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TypeId);
            return View(ticket);
        }

        // POST: Ticket/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Developer")]
        public ActionResult Edit([Bind(Include = "Id,OwnerId,Title,Description,Created,Updated,AssignToUserId,ProjectId,TypeId,PriorityId,StatusId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Updated = DateTime.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerId = new SelectList(db.AspNetUsers, "Id", "FirstName", ticket.OwnerId);
            ViewBag.AssignToUserId = new SelectList(db.AspNetUsers, "Id", "FirstName", ticket.AssignToUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.PriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.PriorityId);
            ViewBag.StatusId = new SelectList(db.TicketStatus, "ID", "Name", ticket.StatusId);
            ViewBag.TypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TypeId);
            return View(ticket);
        }

        // GET: Ticket/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Ticket/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
