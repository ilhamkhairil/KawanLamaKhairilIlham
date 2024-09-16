using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KawanLamaKhairilIlham.Data;
using Xunit;
using KhairilKawanLama.Services;

namespace TodoApp.Tests
{
    public class TodoServiceTests
    {
        private readonly AppDbContext _context;

        public TodoServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TodoTestDb")
                .Options;

            _context = new AppDbContext(options);
        }

        [Fact]
        public async Task CanAddTodo()
        {
            // Arrange
            var service = new ToDoService(_context);
            var todo = new TodoData { Subject = "Test", Description = "Test Desc" };

            // Act
            await service.CreateToDoAsync(todo);
            var todos = await _context.ToDos.ToListAsync();

            // Assert
            Assert.Single(todos);
            Assert.Equal("Test", todos[0].Subject);
        }
    }
}
