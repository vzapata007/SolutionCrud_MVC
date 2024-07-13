using ProyectoCrud.DAL.DataContext;
using ProyectoCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoCrud.DAL.Repositorio
{
    public class ContactoRepository : IGenericRepository<Contacto>
    {

        private readonly DBPRUEBASContext _dbcontext;

        public ContactoRepository(DBPRUEBASContext context)
        {
            _dbcontext = context;
        }

        public async Task<bool> Actualizar(Contacto modelo)
        {
            _dbcontext.Contactos.Update(modelo);
            await _dbcontext.SaveChangesAsync();
            return true;

        }

        public async Task<bool> Eliminar(int id)
        {
            Contacto modelo = _dbcontext.Contactos.First(c => c.IdContacto == id);
            _dbcontext.Contactos.Remove(modelo);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insertar(Contacto modelo)
        {
            _dbcontext.Contactos.Add(modelo);
            await _dbcontext.SaveChangesAsync();
            return true;
        }

        public async Task<Contacto> Obtener(int id)
        {
            return await _dbcontext.Contactos.FindAsync(id);
        }

        public async Task<IQueryable<Contacto>> ObtenerTodos()
        {
            IQueryable<Contacto> queryContactoSQL = _dbcontext.Contactos;
            return queryContactoSQL;
        }
    }
}
