using KawanLamaKhairilIlham.Data;
using KawanLamaKhairilIlham.Services.Interfaces;
using KawanLamaKhairilIlham.Data;
using KawanLamaKhairilIlham.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhairilKawanLama.Services
{
    public class ToDoService : IToDoService
    {
        private readonly AppDbContext _context;

        public ToDoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TodoData>> GetToDosForUserAsync(int userId)
        {
            return await _context.ToDos
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task CreateToDoAsync(TodoData toDo)
        {
            _context.ToDos.Add(toDo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateToDoAsync(TodoData toDo)
        {
            _context.ToDos.Update(toDo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteToDoAsync(int toDoId, int userId)
        {
            var toDo = await _context.ToDos.FirstOrDefaultAsync(t => t.Id == toDoId && t.UserId == userId);
            if (toDo != null)
            {
                _context.ToDos.Remove(toDo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task MarkToDoAsync(int toDoId, ToDoStatus status)
        {
            var toDo = await _context.ToDos.FindAsync(toDoId);
            if (toDo != null)
            {
                toDo.Status = status;
                await _context.SaveChangesAsync();
            }
        }
    }
}
