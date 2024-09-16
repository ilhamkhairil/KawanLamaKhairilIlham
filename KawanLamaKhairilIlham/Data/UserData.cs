namespace KawanLamaKhairilIlham.Data
{
    public class UserData
    {
        public int Id { get; set; }
        public string UserName { get; set; } // User Id
        public string PasswordHash { get; set; } // Store hashed password
        public string FullName { get; set; }
        public List<TodoData> ToDos { get; set; } // Navigation property for ToDo items
    }
}
