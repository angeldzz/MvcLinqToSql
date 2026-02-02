using Microsoft.AspNetCore.Mvc;
using MvcLinqToSql.Models;
using MvcLinqToSql.Repositories;
using System.Threading.Tasks;

namespace MvcLinqToSql.Controllers
{
    public class EnfermosController : Controller
    {
        RepositoryEnfermos repo;
        public EnfermosController()
        {
            this.repo = new RepositoryEnfermos();
        }
        public IActionResult Index()
        {
            List<Enfermo> enfermos = this.repo.GetEnfermos();
            return View(enfermos);
        }
        public IActionResult Details(string inscripcion)
        {
            Enfermo enfermo = this.repo.GetEnfermoInscripcion(inscripcion);
            if (enfermo == null)
            {
                return NotFound();
            }
            return View(enfermo);
        }
        public async Task<IActionResult> Delete(string inscripcion)
        {
            int registros = await this.repo.DeleteEmpleado(inscripcion);
            return RedirectToAction("Index");
        }
        
    }
}
