using Microsoft.Data.SqlClient;
using MvcSegundaPracticaJST.Models;
using System.Data;

namespace MvcSegundaPracticaJST.Repositories
{
    public class RepositoryComics
    {
        private SqlConnection cn;
        private SqlCommand com;
        private DataTable tablaComics;

        public RepositoryComics()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=COMICS;Persist Security Info=True;User ID=SA;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;

            string sql = "select * from COMICS";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            this.tablaComics = new DataTable();
            ad.Fill(this.tablaComics);
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            List<Comic> comics = new List<Comic>();
            foreach(var row in consulta)
            {
                Comic cmc = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")
                };
                comics.Add(cmc);
            }
            return comics;
        }

        public async Task CreateComics(Comic cmc)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            var row = consulta.FirstOrDefault();
            int maximoId = consulta.Max(x => x.Field<int>("IDCOMIC"));
            int idMayor = maximoId + 1;

            string sql = "insert into COMICS (IDCOMIC, NOMBRE, IMAGEN, DESCRIPCION) values(@idcomic, @nombre, @imagen, @descripcion)";
            this.com.Parameters.AddWithValue("@idcomic", idMayor);
            this.com.Parameters.AddWithValue("@nombre", cmc.Nombre);
            this.com.Parameters.AddWithValue("@imagen", cmc.Imagen);
            this.com.Parameters.AddWithValue("@descripcion", cmc.Descripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }


        public Comic GetDetailsComic(int id)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == id
                           select datos;
            var row = consulta.First();
            Comic cmc = new Comic
            {
                IdComic = row.Field<int>("IDCOMIC"),
                Nombre = row.Field<string>("NOMBRE"),
                Imagen = row.Field<string>("IMAGEN"),
                Descripcion = row.Field<string>("DESCRIPCION")
            };
            return cmc;
        }
    }

}
