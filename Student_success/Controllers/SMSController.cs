using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using AtomPark;
using Student_success.Models;

namespace Student_success.Controllers
{
    public class SMSController : Controller
    {
        private StudentSucessContext db = new StudentSucessContext();
        // GET: SMS
        public ActionResult Index()
        {
            ViewData["Balance"] = GetBalance(); //Display Balance, Price, Status
            ViewData["Price"] = GetPrice();
            ViewData["Status"] = GetStatus();
            return View();
        }

        public string GetBalance()
        {
            var XML = "XML=<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +      //Making XML for getting balance
                      "<SMS>\n" +
                      "<operations>\n" +
                      "<operation>BALANCE</operation>\n" +
                      "</operations>\n" +
                      "<authentification>\n" +
                      "<username>" + Class1.username + "</username>\n" +
                      "<password>" + Class1.password + "</password>\n" +
                      "</authentification>\n" +
                      "</SMS>\n";
            return GetResponse(XML);
        }

        public string GetPrice()
        {
            var XML = "XML=<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +      //Making XML for getting Price
                      "<SMS>\n" +
                      "<operations>\n" +
                      "<operation>GETPRICE</operation>\n" +
                      "</operations>\n" +
                      "<authentification>\n" +
                      "<username>" + Class1.username + "</username>\n" +
                      "<password>" + Class1.password + "</password>\n" +
                      "</authentification>\n" +
                      "<message>\n" +
                      "<sender>SMS</sender>\n" +
                      "<text>Test message [UTF-8]</text>\n" +
                      "</message>\n" +
                      "<numbers>\n" +
                      "<number messageID=\"msg11\">380972920000</number>\n" +
                      "</numbers>\n" +
                      "</SMS>\n";
            return GetResponse(XML);
        }

        public ActionResult Send(IList<string> StudentsId)
        {
            if (StudentsId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            string res = "";                                            //Create messages
            List<Student> students = new List<Student>();
            List<Message> messages = new List<Message>();
            for (int i = 0; i < StudentsId.Count; i++)
            {
                students.Add(db.Students.Find(Int32.Parse(StudentsId[i])));
                messages.Add(new Message());
                messages[i].Student = students[i];
                db.Messages.Add(messages[i]);
            }
            db.SaveChanges();
            string marks;
            for (int i = 0; i < messages.Count; i++)
            {
                marks = "";
                for (int j = 0; j < students[i].Marks.Count; j++)
                {
                    var listmark =students.ElementAt(j).Marks;
                    marks += listmark.ElementAt(j).Subject.Name + ":" + listmark.ElementAt(j).Mark1 + ";";

                }
                res += "<number messageID=\"" + messages[i].Id + "\" variables=\"" + students[i].Name + "; "+ marks +" \">";
                res += students[i].Number + "</number>\n";
            }
            var XML = "XML=<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +  //Making XML for send sms
                  "<SMS>\n" +
                  "<operations>\n" +
                  "<operation>SEND</operation>\n" +
                  "</operations>\n" +
                  "<authentification>\n" +
                      "<username>" + Class1.username + "</username>\n" +
                      "<password>" + Class1.password + "</password>\n" +
                  "</authentification>\n" +
                  "<message>\n" +
                  "<sender>Valentyn</sender>\n" +
                  "<text><![CDATA[Hello %1%, your marks : %2%]]></text>\n" +
                  "</message>\n" +
                  "<numbers>\n" +
                      res +
                  "</numbers>\n" +
                  "</SMS>\n";
            ViewData["Status"] = GetResponse(XML);
            return View();
        }

        public string GetStatus()
        {
            var mes = db.Messages.ToList();
            string messageid = "";
            for (int i = 0; i < mes.Count; i++)
                messageid += "<messageid>" + mes[i].Id + "</messageid>\n";
            var XML = "XML=<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +      //Making XML for getting Status
                      "<SMS>\n" +
                      "<operations>\n" +
                      "<operation>GETSTATUS</operation>\n" +
                      "</operations>\n" +
                      "<authentification>\n" +
                      "<username>" + Class1.username + "</username>\n" +
                      "<password>" + Class1.password + "</password>\n" +
                      "</authentification>\n" +
                      "<statistics>\n" +
                      messageid +
                      "</statistics>\n" +
                      "</SMS>\n";
            string res = GetResponse(XML);
            res = res.Replace(">", ">\n");
            return res;
        }

        public string GetResponse(String XML)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create("http://api.atompark.com/members/sms/xml.php") as HttpWebRequest;
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded";
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = encoding.GetBytes(XML);
                request.ContentLength = data.Length;
                Stream dataStream = request.GetRequestStream();     //Requesting to the server and recive
                dataStream.Write(data, 0, data.Length);
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format(
                            "Server error (HTTP {0}: {1}).",
                            response.StatusCode,
                            response.StatusDescription));
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    return reader.ReadToEnd();              //Return data from server
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
