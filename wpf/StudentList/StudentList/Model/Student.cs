using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsList.Model
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public virtual Group Group { get; set; }

        [MaxLength(64)]
        [Required]
        public virtual string FirstName { get; set; }

        [MaxLength(64)]
        [Required]
        public virtual string LastName { get; set; }

        [MaxLength(64)]
        public virtual string City { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(10)]
        [Required]
        public virtual string IndexID { get; set; }

        [Timestamp]
        public byte[] Stamp { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Student st = obj as Student;
            if (st == null)
            {
                return false;
            }
            return (IndexID.Equals(st.IndexID)) &&
                   (FirstName == st.FirstName) &&
                   (LastName == st.LastName) &&
                   (City == st.City) &&
                   (DateOfBirth.Equals(st.DateOfBirth));
        }

        public override string ToString()
        {
            return FirstName + LastName;
        }
    }
}
