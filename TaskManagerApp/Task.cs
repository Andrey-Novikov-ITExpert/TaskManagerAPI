using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagerApp
{
    /// <summary>
    /// Task data
    /// </summary>
    public class Task
    {
        public int TaskId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime RequiredByDate { get; set; }

        public string TaskDescription { get; set; }

        public string TaskStatus { get; set; }

        public string TaskType { get; set; }

        public string AssignedTo { get; set; }
    }
}
