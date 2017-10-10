namespace StudentListJS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Students
    {
        [Key]
        public int IDStudent { get; set; }

        [Required]
        [StringLength(20)]
        [DisplayName("Imie")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(20)]
        [DisplayName("Nazwisko")]
        public string LastName { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Indeks")]
        public string IndexNo { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Data urodzin")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDate { get; set; }

        [StringLength(32)]
        [DisplayName("Miasto")]
        public string BirthPlace { get; set; }

        [DisplayName("Grupa")]
        public int IDGroup { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Stamp { get; set; }

        public virtual Groups Groups { get; set; }

        public override bool Equals(object obj)
        {
            Students st = obj as Students;
            if (st == null)
            {
                return false;
            }
            return FirstName == st.FirstName &&
                LastName == st.LastName &&
                BirthPlace == st.BirthPlace &&
                IndexNo == st.IndexNo &&
                BirthDate.Equals(st.BirthDate) &&
                IDGroup.Equals(st.IDGroup);
        }
    }
}
