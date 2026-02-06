using Microsoft.AspNetCore.Mvc;
using MvcSegundaPracticaJST.Models;
using MvcSegundaPracticaJST.Repositories;

namespace MvcSegundaPracticaJST.Controllers
{
    public class ComicsController : Controller
    {
        private RepositoryComics repo;
        public ComicsController()
        {
            this.repo = new RepositoryComics();
        }

        public IActionResult Index()
        {
            List<Comic> comics = this.repo.GetComics();
            return View(comics);
        }

        public IActionResult Details(int id)
        {
            Comic cmc = this.repo.GetDetailsComic(id);
            return View(cmc);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Comic cmc)
        {
            await this.repo.CreateComics(cmc);
            return RedirectToAction("Index");
        }
    }
}
