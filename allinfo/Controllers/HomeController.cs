using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using allinfo.Models;
using allinfo.Data;
using System.IO;

namespace allinfo.Controllers
{
    public class HomeController : Controller
    {
        private IHomeRepository homeRepository;

        public HomeController(IHomeRepository homeRepository)
        {
            this.homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await homeRepository.GetArticlesAsync();
            ViewData["Scrap"] = await homeRepository.GetPageDataAsync();
            ViewData["Video"] = await homeRepository.GetVideoDataAsync();
            return View(articles);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
