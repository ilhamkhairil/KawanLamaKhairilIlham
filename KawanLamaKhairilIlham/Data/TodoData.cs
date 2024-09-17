using Microsoft.AspNetCore.Identity;
using System;

namespace KawanLamaKhairilIlham.Data
{
    public class TodoData
    {
        public int Id { get; set; }
        public string ActivitiesNo { get; set; } // e.g., "AC-0001"
        public string Subject { get; set; }
        public string Description { get; set; }
        public ToDoStatus Status { get; set; } // Enum: Unmarked, Done, Canceled
        public DateTime CreatedAt { get; set; }

        // Foreign Key to User
        public int UserId { get; set; } // Update to string
        public UserData User { get; set; }
    }

    public enum ToDoStatus
    {
        Unmarked,
        Done,
        Canceled
    }
}
