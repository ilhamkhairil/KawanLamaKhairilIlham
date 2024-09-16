using Microsoft.EntityFrameworkCore;

namespace KawanLamaKhairilIlham.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserData> Users { get; set; }
        public DbSet<TodoData> ToDos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the default value for Status
            modelBuilder.Entity<TodoData>()
                .Property(t => t.Status)
                .HasDefaultValue(ToDoStatus.Unmarked);

            // Configure the primary key and relationships if necessary
            //modelBuilder.Entity<TodoData>()
            //    .HasOne(t => t.User)
            //    .WithMany(u => u.Todo)
            //    .HasForeignKey(t => t.UserId);

            //// Configure the unique constraint for ActivitiesNo if needed
            //modelBuilder.Entity<TodoData>()
            //    .HasIndex(t => t.ActivitiesNo)
            //    .IsUnique();
        }
    }
}
