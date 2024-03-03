using Microsoft.AspNetCore.Mvc;
using MvcSqlCrudSample.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MvcSqlCrudSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;
        private readonly ISampleRepository _sampleRepository;

        public HomeController(IConfiguration configuration, ISampleRepository sampleRepository)
        {
            _sampleRepository = sampleRepository;
            // appsettings.jsonファイルから接続文字列を取得
            _connectionString = configuration.GetConnectionString("TodoConnection") 
                ?? throw new ArgumentException("接続文字列が取得できませんでした");
        }

        /// <summary>
        /// ToDoアイテムを全件検索して結果をViewに渡す
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var sample = _sampleRepository.Get();

            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();

            await connection.OpenAsync();
            command.CommandText = "SELECT * FROM TodoItems";

            // テーブルのデータを全て読み取ってモデルのリストに格納
            var models = new List<TodoItemViewModel>();
            using var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                models.Add(new TodoItemViewModel
                {
                    Id = (int)reader[nameof(TodoItemViewModel.Id)],
                    Name = reader[nameof(TodoItemViewModel.Name)].ToString(),
                    IsComplete = (bool)reader[nameof(TodoItemViewModel.IsComplete)],
                });
            }

            return View(new TodoItemViewModel { Items = models });
        }

        /// <summary>
        /// ToDoアイテムを登録してIndexにリダイレクトする
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] TodoItemViewModel todoItem)
        {
            if (ModelState.IsValid)
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = connection.CreateCommand();

                await connection.OpenAsync();
                command.CommandText = "INSERT INTO TodoItems Values(@Name, 'false');";

                // フォームに入力されたタスク名をパラメータに設定
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = todoItem.Name;

                // INSERT文の実行
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 「完了」がクリックされた場合にToDoアイテムを削除し、Indexにリダイレクトする
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete([Bind("Id")] TodoItemViewModel todoItem)
        {
            if (ModelState.IsValid)
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = connection.CreateCommand();

                await connection.OpenAsync();
                command.CommandText = "DELETE FROM TodoItems WHERE Id = @Id";
                command.Parameters.Add("@Id", SqlDbType.Int).Value = todoItem.Id;

                // DELETE文の実行
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
