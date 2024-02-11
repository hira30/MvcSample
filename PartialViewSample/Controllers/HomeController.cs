using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PartialViewSample.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index(string searchString)
        {
            // ユーザーのリストを作成（本来であればDB等からデータを取得する）
            var userList = new List<UserModel>
            {
                new() { Name = "山田 太郎", Age = 30 },
                new() { Name = "田中 次郎", Age = 20 },
                new() { Name = "山田 花子", Age = 10 },
            };

            // 検索文字列が指定されている場合は条件に一致するユーザーのみを取得する
            if (!string.IsNullOrEmpty(searchString))
            {
                // 名前に検索文字列を含むユーザーを取得
                userList = userList.Where(x => x.Name.Contains(searchString)).ToList();

                // 確認のためわざと3秒間停止させる
                await Task.Delay(3000);
            }

            return View(userList);
        }
    }

    public class UserModel
    {
        [Display(Name = "名前")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "年齢")]
        public int Age { get; set; }
    }
}
