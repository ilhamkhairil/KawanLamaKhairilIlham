using Microsoft.Extensions.Logging;
using KawanLamaKhairilIlham.Data;
using KawanLamaKhairilIlham.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KhairilKawanLama.Services
{
    public class ToDoService : IToDoService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ToDoService> _logger;
        private readonly IUserService _userService;

        public ToDoService(AppDbContext context, ILogger<ToDoService> logger, IUserService userService)
        {
            _context = context;
            _logger = logger;
            _userService = userService;
        }

        public async Task<List<TodoData>> GetToDosForUserAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Fetching to-dos for user with ID {UserId}", userId);
                return await _context.ToDos
                    .Where(t => t.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching to-dos for user with ID {UserId}", userId);
                throw; // Rethrow the exception to propagate it further
            }
        }

        public async Task CreateToDoAsync(TodoData toDo)
        {
            try
            {
                if (toDo == null)
                {
                    throw new ArgumentNullException(nameof(toDo), "ToDo cannot be null");
                }

                toDo.ActivitiesNo = await GenerateUniqueActivitiesNoAsync();
         
                toDo.CreatedAt = DateTime.UtcNow;
                toDo.Status = ToDoStatus.Unmarked;

                _context.ToDos.Add(toDo);
                await _context.SaveChangesAsync();

                _logger.LogInformation("To-do with ID {Id} created successfully", toDo.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ToDo");
                throw;
            }
        }

        public async Task UpdateToDoAsync(TodoData toDo)
        {
            try
            {
                if (toDo == null)
                {
                    throw new ArgumentNullException(nameof(toDo), "ToDo cannot be null");
                }

                _logger.LogInformation("Updating to-do with ID {Id}", toDo.Id);
                _context.ToDos.Update(toDo);
                await _context.SaveChangesAsync();
                _logger.LogInformation("To-do with ID {Id} updated successfully", toDo.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating to-do with ID {Id}", toDo.Id);
                throw;
            }
        }

        public async Task DeleteToDoAsync(int toDoId, int userId)
        {
            try
            {
                _logger.LogInformation("Deleting to-do with ID {ToDoId} for user with ID {UserId}", toDoId, userId);
                var toDo = await _context.ToDos.FirstOrDefaultAsync(t => t.Id == toDoId && t.UserId == userId);
                if (toDo != null)
                {
                    _context.ToDos.Remove(toDo);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("To-do with ID {ToDoId} deleted successfully", toDoId);
                }
                else
                {
                    _logger.LogWarning("To-do with ID {ToDoId} not found", toDoId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting to-do with ID {ToDoId} for user with ID {UserId}", toDoId, userId);
                throw;
            }
        }

        public async Task MarkToDoAsync(int toDoId, ToDoStatus status)
        {
            try
            {
                _logger.LogInformation("Marking to-do with ID {ToDoId} as {Status}", toDoId, status);
                var toDo = await _context.ToDos.FindAsync(toDoId);
                if (toDo != null)
                {
                    toDo.Status = status;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("To-do with ID {ToDoId} marked as {Status} successfully", toDoId, status);
                }
                else
                {
                    _logger.LogWarning("To-do with ID {ToDoId} not found", toDoId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while marking to-do with ID {ToDoId} as {Status}", toDoId, status);
                throw;
            }
        }

        private async Task<string> GenerateUniqueActivitiesNoAsync()
        {
            try
            {
                // Get the maximum existing number for the current format
                var lastToDo = await _context.ToDos
                    .OrderByDescending(t => t.CreatedAt)
                    .FirstOrDefaultAsync();

                var newNumber = 1;
                if (lastToDo != null)
                {
                    if (int.TryParse(lastToDo.ActivitiesNo.Substring(3), out var lastNumber))
                    {
                        newNumber = lastNumber + 1;
                    }
                    else
                    {
                        _logger.LogWarning("Failed to parse the last number from ActivitiesNo: {ActivitiesNo}", lastToDo.ActivitiesNo);
                    }
                }

                return $"AC-{newNumber:D4}"; // Format as "AC-XXXX"
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating a unique ActivitiesNo");
                throw;
            }
        }
    }
}
