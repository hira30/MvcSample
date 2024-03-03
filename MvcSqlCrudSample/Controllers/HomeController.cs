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
            // appsettings.json�t�@�C������ڑ���������擾
            _connectionString = configuration.GetConnectionString("TodoConnection") 
                ?? throw new ArgumentException("�ڑ������񂪎擾�ł��܂���ł���");
        }

        /// <summary>
        /// ToDo�A�C�e����S���������Č��ʂ�View�ɓn��
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var sample = _sampleRepository.Get();

            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();

            await connection.OpenAsync();
            command.CommandText = "SELECT * FROM TodoItems";

            // �e�[�u���̃f�[�^��S�ēǂݎ���ă��f���̃��X�g�Ɋi�[
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
        /// ToDo�A�C�e����o�^����Index�Ƀ��_�C���N�g����
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

                // �t�H�[���ɓ��͂��ꂽ�^�X�N�����p�����[�^�ɐݒ�
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = todoItem.Name;

                // INSERT���̎��s
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// �u�����v���N���b�N���ꂽ�ꍇ��ToDo�A�C�e�����폜���AIndex�Ƀ��_�C���N�g����
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

                // DELETE���̎��s
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
