using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MvcDapperSample.Models;

namespace MvcDapperSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            // appsettings.jsonファイルから接続文字列を取得
            _connectionString = configuration.GetConnectionString("TodoConnection")
                ?? throw new ArgumentException("接続文字列が取得できませんでした");
        }

        /// <summary>
        /// ToDoアイテムを全件検索する
        /// </summary>
        public async Task<IActionResult> Index()
        {
            using var connection = new SqlConnection(_connectionString);

            // 実行するSQL
            var sql = "SELECT * FROM TodoItems";

            // SELECT文を実行して取得したデータをViewModelにマッピング
            var Items = await connection.QueryAsync<TodoItemViewModel>(sql);

            return View(new TodoItemViewModel { Items = Items.ToList() });
        }

        /// <summary>
        /// ToDoアイテムを登録する
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] TodoItemViewModel todoItem)
        {
            if (ModelState.IsValid)
            {
                using var connection = new SqlConnection(_connectionString);

                var sql = "INSERT INTO TodoItems Values(@Name, 'false');";

                // パラメータに設定する値
                var param = new TodoItemViewModel { Name = todoItem.Name };

                // INSERT文の実行
                await connection.ExecuteAsync(sql, param);
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 「完了」がクリックされた場合にToDoアイテムを削除する
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete([Bind("Id")] TodoItemViewModel todoItem)
        {
            if (ModelState.IsValid)
            {
                using var connection = new SqlConnection(_connectionString);

                var sql = "DELETE FROM TodoItems WHERE Id = @Id";
                var param = new TodoItemViewModel { Id = todoItem.Id };

                // DELETE文の実行
                await connection.ExecuteAsync(sql, param);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}