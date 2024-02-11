using System.ComponentModel.DataAnnotations;

namespace MvcDapperSample.Models
{
    public class TodoItemViewModel
    {
        public int Id { get; set; }

        [Display(Name = "タスク")]
        public string? Name { get; set; } = string.Empty;

        [Display(Name = "完了チェック")]
        public bool IsComplete { get; set; }

        public List<TodoItemViewModel>? Items { get; set; }
    }
}