using PM2E164.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PM2E164.Utiles
{

    public class LugaresRepositorio
    {
        readonly SQLiteAsyncConnection _connection;

        public LugaresRepositorio(string dbpath)
        {
            _connection = new SQLiteAsyncConnection(dbpath);
            /* Crear todos mis objetos de base de datos : tablas */
            _connection.CreateTableAsync<LugarVisitado>().Wait();
        }

        public Task<int> Add(LugarVisitado LugarVisitado)
        {
            if (LugarVisitado.Id == 0)
            {
                return _connection.InsertAsync(LugarVisitado);
            }
            else
            {
                return _connection.UpdateAsync(LugarVisitado);
            }
        }

        public Task<List<LugarVisitado>> GetAll()
        {
            return _connection.Table<LugarVisitado>().ToListAsync();
        }

        public Task<LugarVisitado> GetById(int id)
        {
            return _connection.Table<LugarVisitado>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

        public Task<int> Delete(LugarVisitado LugarVisitado)
        {
            return _connection.Table<LugarVisitado>().DeleteAsync(c => c.Id == LugarVisitado.Id);
        }

    }

}
