using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_success.Models
{
    public class Groups_Subjects
    {
        public virtual int Id { get; set; }
        public virtual Groups Groups { get; set; }

        public virtual Subjects Subjects { get; set; }
    }
}