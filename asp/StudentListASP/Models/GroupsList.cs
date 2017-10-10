using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentListASP.Models
{
    public class GroupsList
    {
        public string ErrMsg { get; internal set; }
        public virtual IPagedList<Groups> PagedGroups { get; internal set; }
        public int Page { get; internal set; }
        public Groups CurrentGroup { get; set; }
    }
}