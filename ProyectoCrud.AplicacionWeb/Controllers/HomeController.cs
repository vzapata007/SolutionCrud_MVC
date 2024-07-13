using Microsoft.AspNetCore.Mvc;
using ProyectoCrud.AplicacionWeb.Models;
using ProyectoCrud.AplicacionWeb.Models.ViewModels;
using ProyectoCrud.BLL.Service;
using ProyectoCrud.Models;
using System.Diagnostics;
using System.Globalization;

namespace ProyectoCrud.AplicacionWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IContactoService _contactoService;

        public HomeController(IContactoService contactoServ)
        {
            _contactoService = contactoServ;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Lista()
        {

            IQueryable<Contacto> queryContactoSQL = await _contactoService.ObtenerTodos();

            List<VMContacto> lista = queryContactoSQL
                                                     .Select(c => new VMContacto() { 
                                                        IdContacto = c.IdContacto,
                                                        Nombre = c.Nombre,
                                                        Telefono = c.Telefono,
                                                        FechaNacimiento = c.FechaNacimiento.Value.ToString("dd/MM/yyyy")
                                                     }).ToList();

            return StatusCode(StatusCodes.Status200OK, lista);

        }


        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody] VMContacto modelo)
        {

            Contacto NuevoModelo = new Contacto()
            {
                Nombre = modelo.Nombre,
                Telefono = modelo.Telefono,
                FechaNacimiento = DateTime.ParseExact(modelo.FechaNacimiento,"dd/MM/yyyy",CultureInfo.CreateSpecificCulture("es-PE"))
            };

            bool respuesta = await _contactoService.Insertar(NuevoModelo);

            return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });

        }

        [HttpPut]
        public async Task<IActionResult> Actualizar([FromBody] VMContacto modelo)
        {

            Contacto NuevoModelo = new Contacto()
            {
                IdContacto = modelo.IdContacto,
                Nombre = modelo.Nombre,
                Telefono = modelo.Telefono,
                FechaNacimiento = DateTime.ParseExact(modelo.FechaNacimiento, "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("es-PE"))
            };

            bool respuesta = await _contactoService.Actualizar(NuevoModelo);

            return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });

        }

        [HttpDelete]
        public async Task<IActionResult> Eliminar(int id)
        {

            bool respuesta = await _contactoService.Eliminar(id);

            return StatusCode(StatusCodes.Status200OK, new { valor = respuesta });

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