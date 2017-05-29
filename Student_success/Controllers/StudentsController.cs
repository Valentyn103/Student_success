using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Student_success.Models;

namespace Student_success.Controllers
{
    public class StudentsController : Controller
    {
        private Model1 db = new Model1(); //With db, we can work with the database

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
        //Page with info for import
        public ActionResult Import()
        {
            return View();
        }

        //Import data from file
        [HttpPost]
        public ActionResult Import(HttpPostedFileBase file, bool reBuild)
        {
            if (file.ContentLength > 0)
            {

                BinaryReader b = new BinaryReader(file.InputStream);
                byte[] binData = b.ReadBytes((int)file.InputStream.Length);
                string result = Encoding.Default.GetString(binData);

                string[] results = result.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0; i < results.Length - 1; i++)
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
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        //Export data to file
        public FileContentResult Export()
        {
            string export = "";
            var students = db.Students.ToList();
            for (int i = 0; i < students.Count; i++)
            {
                export += students[i].Group.Name + ',' + students[i].Name + ',' + students[i].Surname
                    + ',' + students[i].Email + ',' + students[i].Number + "\r\n";
            }
            return File(new System.Text.UTF8Encoding().GetBytes(export), "text/csv", "Students.csv");
        }
        //Send SMS
        public ActionResult SendSMS(int? id)
        {
            /*if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }*/


            var myDictionary = new Dictionary<string, string>();
            myDictionary["version"] = "3.0";
            myDictionary["action"] = "addAddressbook";
            myDictionary["key"] = "3aa244050f538934d1ada951587cb251";
            myDictionary["name"] = "Testaddressbook";
            myDictionary["description"] = "Testdescription";
            var mylist = myDictionary.ToList();
            mylist.Sort((pair1, pair2) => pair1.Key.CompareTo(pair2.Key));

            string sum = "";
            for (int i = 0; i < mylist.Count; i++)
                sum += mylist[i].Key;
            sum += "e103c7e71d05d2188fa9df4aef2f2f60";

            StringBuilder hash1 = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(sum));

            for (int i = 0; i < bytes.Length; i++)
                hash1.Append(bytes[i].ToString("x2"));
            string res = hash1.ToString();

            HttpWebRequest request = WebRequest
                .Create("http://api.atompark.com/api/sms/3.0/addAddressbook?key=3aa244050f538934d1ada951587cb251&sum=" + res
                + "&name=Testaddressbook&description=Testdescription") as HttpWebRequest;
            request.Method = "Post";
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            ViewData["mes"] = responseFromServer;
            reader.Close();
            response.Close();
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

