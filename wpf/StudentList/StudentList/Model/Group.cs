using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsList.Model
{
    public class Group
    {
        [Required]
        public int GroupId { get; set; }

        [MaxLength(16)]
        [Required]
        public string Name { get; set; }

        [Timestamp]
        public byte[] Stamp { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Group gr = obj as Group;
            if (gr == null)
            {
                return false;
            }
            return 
                   (Name == gr.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() & GroupId.GetHashCode();
        }
    }
}