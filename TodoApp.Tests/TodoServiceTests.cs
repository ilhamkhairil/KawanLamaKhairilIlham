using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KawanLamaKhairilIlham.Data;
using Xunit;
using KhairilKawanLama.Services;
using KawanLamaKhairilIlham.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace TodoApp.Tests
{
    public class TodoServiceTests
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ToDoService> _logger;
        private readonly IUserService _userService;

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
            var service = new ToDoService(_context, _logger, _userService);
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
