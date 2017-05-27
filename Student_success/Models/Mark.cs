namespace Student_success.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Mark
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public int SubjectsId { get; set; }

        public DateTime? MarkDate { get; set; }

        [Column("Mark")]
        public int? Mark1 { get; set; }

        public virtual Student Student { get; set; }

        public virtual Subject Subject { get; set; }
    }
}
