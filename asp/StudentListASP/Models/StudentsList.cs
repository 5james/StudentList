using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentListASP.Models
{
    public class StudentsList
    {
        public virtual IPagedList<Students> PagedStudents { get; set; }
        public int Page { get; set; }
        public SelectList Groups { get; set; }
        public Students CurrentStudent { get; set; }
        public string ErrMsg { get; set; }
        public int? CurrentGroupFilter { get; set; }
        public string CurrentCityFilter { get; set; }
    }
}