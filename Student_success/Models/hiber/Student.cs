using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_success.Models
{
    public class Student
    {
        public virtual int Id { get; set; }
        public virtual Groups Groups { get; set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Email { get; set; }
        public virtual string Number { get; set; }
        public virtual IList<Marks> Marks { get; set; }
    }
}