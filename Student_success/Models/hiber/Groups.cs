using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_success.Models
{
    public class Groups
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime CreatedDate { get; set; }
        public virtual IList<Student> Students { get; set; }
        public virtual IList<Groups_Subjects> GroupsSubjectses { get; set; }
    }
}