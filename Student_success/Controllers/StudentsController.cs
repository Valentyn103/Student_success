using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using NHibernate.Mapping;
using Student_success.Models;

namespace Student_success.Controllers
{
    public class StudentsController : Controller
    {
        private Model1 db = new Model1();

        // GET: Students
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Group);
            return View(students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        public ActionResult Import()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file, bool reBuild)
        {
            if (file.ContentLength > 0)
            {
                
                BinaryReader b = new BinaryReader(file.InputStream);
                byte[] binData = b.ReadBytes((int) file.InputStream.Length);
                string result = Encoding.Default.GetString(binData);

                string[]results =  result.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < results.Length-1; i++)
                {
                    string[] parse = results[i].Split(',');
                    string tmp = parse[0];
                    Group group = db.Groups.FirstOrDefault(k => k.Name == tmp);
                    Student student = new Student();
                    student.Group = group;
                    student.Name = parse[1];
                    student.Surname = parse[2];
                    student.Email = parse[3];
                    student.Number = parse[4];
                    db.Students.Add(student);
                }
                if (reBuild)
                {
                    var students = from c in db.Students select c;
                    var marks = from c in db.Marks select c;
                    db.Marks.RemoveRange(marks);
                    db.Students.RemoveRange(students);
                    db.SaveChanges();
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult SendSMS()
        {
            try
            {
                HttpWebRequest request = WebRequest
                    .Create("http://api.atompark.com/api/sms/3.0/getUserBalance?key=3aa244050f538934d1ada951587cb251&sum=control_sum&cy=UAH") as HttpWebRequest;
                request.Method = "Post";
                WebResponse response = request.GetResponse();
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                Console.WriteLine(responseFromServer);
                reader.Close();
                response.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            return View();
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.GroupsId = new SelectList(db.Groups, "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GroupsId,StudentIndex,Name,Surname,Email,Number")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GroupsId = new SelectList(db.Groups, "Id", "Name", student.GroupsId);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupsId = new SelectList(db.Groups, "Id", "Name", student.GroupsId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GroupsId,StudentIndex,Name,Surname,Email,Number")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GroupsId = new SelectList(db.Groups, "Id", "Name", student.GroupsId);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
