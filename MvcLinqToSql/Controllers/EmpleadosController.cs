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
        public IActionResult BuscadorEmpleados()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BuscadorEmpleados(string oficio, int salario)
        {
            List<Empleado> empleados = this.repo.GetEmpleadosOficioSalario(oficio, salario);
            //if (empleados.Count == 0)
            //{
            //    ViewData["MENSAJE"] = "No existen empleados con ese oficio y salario";
            //}
            return View(empleados);
        }
        public IActionResult DatosEmpleados()
        {
            ViewBag.OFICIO = this.repo.GetOficios();
            return View();
        }
        [HttpPost]
        public IActionResult DatosEmpleados(string oficio)
        {
            ResumenEmpleados resumen = this.repo.GetEmpleadosOficio(oficio);
            ViewBag.OFICIO = this.repo.GetOficios();
            return View(resumen);
        }
    }
}
