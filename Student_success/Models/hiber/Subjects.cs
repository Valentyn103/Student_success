using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_success.Models
{
    public class Subjects
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Groups_Subjects> GroupsSubjectses{ get; set; }
        public virtual IList<Marks> Marks{ get; set; }
    }
}