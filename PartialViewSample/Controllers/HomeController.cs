using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PartialViewSample.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index(string searchString)
        {
            // ���[�U�[�̃��X�g���쐬�i�{���ł����DB������f�[�^���擾����j
            var userList = new List<UserModel>
            {
                new() { Name = "�R�c ���Y", Age = 30 },
                new() { Name = "�c�� ���Y", Age = 20 },
                new() { Name = "�R�c �Ԏq", Age = 10 },
            };

            // ���������񂪎w�肳��Ă���ꍇ�͏����Ɉ�v���郆�[�U�[�݂̂��擾����
            if (!string.IsNullOrEmpty(searchString))
            {
                // ���O�Ɍ�����������܂ރ��[�U�[���擾
                userList = userList.Where(x => x.Name.Contains(searchString)).ToList();

                // �m�F�̂��߂킴��3�b�Ԓ�~������
                await Task.Delay(3000);
            }

            return View(userList);
        }
    }

    public class UserModel
    {
        [Display(Name = "���O")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "�N��")]
        public int Age { get; set; }
    }
}
