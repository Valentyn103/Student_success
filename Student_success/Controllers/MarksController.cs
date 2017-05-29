using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Student_success.Models;

namespace Student_success.Controllers
{
    public class MarksController : Controller
    {
        private Model1 db = new Model1();

        // GET: Marks
        public ActionResult Index()
        {
            var marks = db.Marks.Include(m => m.Student).Include(m => m.Subject);
            return View(marks.ToList());
        }

        // GET: Marks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mark mark = db.Marks.Find(id);
            if (mark == null)
            {
                return HttpNotFound();
            }
            return View(mark);
        }

        // GET: Marks/Create
        public ActionResult Create()
        {
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name");
            ViewBag.SubjectsId = new SelectList(db.Subjects, "Id", "Name");
            return View();
        }

        // POST: Marks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StudentId,SubjectsId,MarkDate,Mark1,MarkIndex")] Mark mark)
        {
            if (ModelState.IsValid)
            {
                mark.MarkDate = DateTime.Now;
                db.Marks.Add(mark);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", mark.StudentId);
            ViewBag.SubjectsId = new SelectList(db.Subjects, "Id", "Name", mark.SubjectsId);
            return View(mark);
        }

        // GET: Marks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mark mark = db.Marks.Find(id);
            if (mark == null)
            {
                return HttpNotFound();
            }
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", mark.StudentId);
            ViewBag.SubjectsId = new SelectList(db.Subjects, "Id", "Name", mark.SubjectsId);
            return View(mark);
        }

        // POST: Marks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StudentId,SubjectsId,MarkDate,Mark1,MarkIndex")] Mark mark)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mark).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StudentId = new SelectList(db.Students, "Id", "Name", mark.StudentId);
            ViewBag.SubjectsId = new SelectList(db.Subjects, "Id", "Name", mark.SubjectsId);
            return View(mark);
        }

        // GET: Marks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mark mark = db.Marks.Find(id);
            if (mark == null)
            {
                return HttpNotFound();
            }
            return View(mark);
        }

        // POST: Marks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mark mark = db.Marks.Find(id);
            db.Marks.Remove(mark);
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
