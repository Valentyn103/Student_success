namespace Student_success.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Student")]
    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            Marks = new HashSet<Mark>();
        }

        public int Id { get; set; }

        public int GroupsId { get; set; }

        [StringLength(64)]
        public string Name { get; set; }

        [StringLength(64)]
        public string Surname { get; set; }

        [StringLength(64)]
        public string Email { get; set; }

        [StringLength(64)]
        public string Number { get; set; }

        public virtual Group Group { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Mark> Marks { get; set; }
    }
}
