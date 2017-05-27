using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Student_success.Models
{
    public class Marks
    {
        public virtual int Id { get; set; }
        public virtual int Mark { get; set; }
        public virtual DateTime MarkDate { get; set; }
        public virtual Student Student { get; set; }

        public virtual Subjects Subjects { get; set; }
    }
}