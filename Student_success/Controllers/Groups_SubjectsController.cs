using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Student_success.Models;

namespace Student_success.Controllers
{
    public class Groups_SubjectsController : Controller
    {
        private StudentSucessContext db = new StudentSucessContext();

        // GET: Groups_Subjects
        public ActionResult Index()
        {
            var groups_Subjects = db.Groups_Subjects.Include(g => g.Group).Include(g => g.Subject);
            return View(groups_Subjects.ToList());
        }

        // GET: Groups_Subjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Groups_Subjects groups_Subjects = db.Groups_Subjects.Find(id);
            if (groups_Subjects == null)
            {
                return HttpNotFound();
            }
            return View(groups_Subjects);
        }

        // GET: Groups_Subjects/Create
        public ActionResult Create()
        {
            ViewBag.GroupsId = new SelectList(db.Groups, "Id", "Name");
            ViewBag.SubjectsId = new SelectList(db.Subjects, "Id", "Name");
            return View();
        }

        // POST: Groups_Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GroupsId,SubjectsId,Groups_SubjectIndex")] Groups_Subjects groups_Subjects)
        {
            if (ModelState.IsValid)
            {
                db.Groups_Subjects.Add(groups_Subjects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GroupsId = new SelectList(db.Groups, "Id", "Name", groups_Subjects.GroupsId);
            ViewBag.SubjectsId = new SelectList(db.Subjects, "Id", "Name", groups_Subjects.SubjectsId);
            return View(groups_Subjects);
        }

        // GET: Groups_Subjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Groups_Subjects groups_Subjects = db.Groups_Subjects.Find(id);
            if (groups_Subjects == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupsId = new SelectList(db.Groups, "Id", "Name", groups_Subjects.GroupsId);
            ViewBag.SubjectsId = new SelectList(db.Subjects, "Id", "Name", groups_Subjects.SubjectsId);
            return View(groups_Subjects);
        }

        // POST: Groups_Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GroupsId,SubjectsId,Groups_SubjectIndex")] Groups_Subjects groups_Subjects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groups_Subjects).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GroupsId = new SelectList(db.Groups, "Id", "Name", groups_Subjects.GroupsId);
            ViewBag.SubjectsId = new SelectList(db.Subjects, "Id", "Name", groups_Subjects.SubjectsId);
            return View(groups_Subjects);
        }

        // GET: Groups_Subjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Groups_Subjects groups_Subjects = db.Groups_Subjects.Find(id);
            if (groups_Subjects == null)
            {
                return HttpNotFound();
            }
            return View(groups_Subjects);
        }

        // POST: Groups_Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Groups_Subjects groups_Subjects = db.Groups_Subjects.Find(id);
            db.Groups_Subjects.Remove(groups_Subjects);
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
