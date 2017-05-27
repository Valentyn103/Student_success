namespace Student_success.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Groups_Subjects
    {
        public int Id { get; set; }

        public int GroupsId { get; set; }

        public int SubjectsId { get; set; }

        public virtual Group Group { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
