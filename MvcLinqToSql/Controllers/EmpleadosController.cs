using Microsoft.AspNetCore.Mvc;
using MvcLinqToSql.Models;
using MvcLinqToSql.Repositories;

namespace MvcLinqToSql.Controllers
{
    public class EmpleadosController : Controller
    {
        RepositoryEmpleado repo;
        public EmpleadosController()
        {
            this.repo = new RepositoryEmpleado();
        }

        public IActionResult Index()
        {
            List<Empleado> empleados = this.repo.GetEmpleados();
            return View(empleados);
        }
        public IActionResult Details(int idempleado)
        {
            Empleado empleado = this.repo.FindEmpleado(idempleado);
            return View(empleado);
        }
    }
}
