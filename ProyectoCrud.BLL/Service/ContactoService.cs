using ProyectoCrud.DAL.Repositorio;
using ProyectoCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCrud.BLL.Service
{
    public class ContactoService : IContactoService
    {
        private readonly IGenericRepository<Contacto> _contacRepo;

        public ContactoService(IGenericRepository<Contacto> contactRepo)
        {
            _contacRepo = contactRepo;
        }

        public async Task<bool> Actualizar(Contacto modelo)
        {
            return await _contacRepo.Actualizar(modelo);
        }

        public async Task<bool> Eliminar(int id)
        {
            return await _contacRepo.Eliminar(id);
        }

        public async Task<bool> Insertar(Contacto modelo)
        {
            return await _contacRepo.Insertar(modelo);
        }

        public async Task<Contacto> Obtener(int id)
        {
            return await _contacRepo.Obtener(id);
        }

        public async Task<Contacto> ObtenerPorNombre(string nombreContacto)
        {
            IQueryable<Contacto> queryContactoSQL = await _contacRepo.ObtenerTodos();
            Contacto contacto = queryContactoSQL.Where(c => c.Nombre == nombreContacto ).FirstOrDefault();
            return contacto;
        }

        public async Task<IQueryable<Contacto>> ObtenerTodos()
        {
            return await _contacRepo.ObtenerTodos();
        }
    }
}
