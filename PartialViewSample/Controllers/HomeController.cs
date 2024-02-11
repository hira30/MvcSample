using Microsoft.AspNetCore.Mvc;

namespace PartialViewSample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new SampleViewModel { Name = "‘¾˜Y", Age = 20 });
        }
    }

    public class SampleViewModel
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}
