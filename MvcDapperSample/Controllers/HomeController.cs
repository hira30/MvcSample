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
            // appsettings.json�t�@�C������ڑ���������擾
            _connectionString = configuration.GetConnectionString("TodoConnection")
                ?? throw new ArgumentException("�ڑ������񂪎擾�ł��܂���ł���");
        }

        /// <summary>
        /// ToDo�A�C�e����S����������
        /// </summary>
        public async Task<IActionResult> Index()
        {
            using var connection = new SqlConnection(_connectionString);

            // ���s����SQL
            var sql = "SELECT * FROM TodoItems";

            // SELECT�������s���Ď擾�����f�[�^��ViewModel�Ƀ}�b�s���O
            var Items = await connection.QueryAsync<TodoItemViewModel>(sql);

            return View(new TodoItemViewModel { Items = Items.ToList() });
        }

        /// <summary>
        /// ToDo�A�C�e����o�^����
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] TodoItemViewModel todoItem)
        {
            if (ModelState.IsValid)
            {
                using var connection = new SqlConnection(_connectionString);

                var sql = "INSERT INTO TodoItems Values(@Name, 'false');";

                // �p�����[�^�ɐݒ肷��l
                var param = new TodoItemViewModel { Name = todoItem.Name };

                // INSERT���̎��s
                await connection.ExecuteAsync(sql, param);
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// �u�����v���N���b�N���ꂽ�ꍇ��ToDo�A�C�e�����폜����
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete([Bind("Id")] TodoItemViewModel todoItem)
        {
            if (ModelState.IsValid)
            {
                using var connection = new SqlConnection(_connectionString);

                var sql = "DELETE FROM TodoItems WHERE Id = @Id";
                var param = new TodoItemViewModel { Id = todoItem.Id };

                // DELETE���̎��s
                await connection.ExecuteAsync(sql, param);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}