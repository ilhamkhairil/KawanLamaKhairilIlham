using KawanLamaKhairilIlham.Data;

namespace KawanLamaKhairilIlham.Services.Interfaces
{
    public interface IToDoService
    {
        Task<List<TodoData>> GetToDosForUserAsync(int userId);
        Task CreateToDoAsync(TodoData toDo);
        Task UpdateToDoAsync(TodoData toDo);
        Task DeleteToDoAsync(int toDoId, int userId);
        Task MarkToDoAsync(int toDoId, ToDoStatus status);
    }
}
