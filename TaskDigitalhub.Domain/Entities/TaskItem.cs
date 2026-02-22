using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskDigitalhub.Domain.Enums;

namespace TaskDigitalhub.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int AssignedToUserId { get; set; }

        public TaskPriority Priority { get; set; }

        public DateTime DueDate { get; set; }

        public Enums.TaskStatus Status { get; set; }

        public Project Project { get; set; } = null!;

        public User AssignedToUser { get; set; } = null!;
    }
}
