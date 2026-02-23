using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public ProjectStatus Status { get; set; }

        public decimal Budget { get; set; }

        public int? ProjectManagerId { get; set; }
        public User? ProjectManager { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
