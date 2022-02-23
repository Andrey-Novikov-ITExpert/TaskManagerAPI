using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerApp
{
    /// <summary>
    /// Comment data
    /// </summary>
    public class Comment
    {
        public int commentid { get; set; }
        public int taskid { get; set; }
        public DateTime dateadded { get; set; }
        public string commenttext { get; set; }
        public string commenttype { get; set; }
        public DateTime reminderdate { get; set; }
    }
}
