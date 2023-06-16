using SQLite;

namespace PM2E164.Models
{
    public class LugarVisitado
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; } = 0;

        public double Longitud { get; set; }
        
        public double Latitud { get; set; }

        [MaxLength(200)]
        public string Descripcion { get; set; }

        public byte[] Foto { get; set; }

    }
}
