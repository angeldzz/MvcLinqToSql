using Microsoft.Data.SqlClient;
using MvcLinqToSql.Models;
using System.Data;

namespace MvcLinqToSql.Repositories
{
    public class RepositoryEnfermos
    {
        SqlConnection cn;
        SqlCommand com;
        private DataTable tablaEnfermos;
        public RepositoryEnfermos()
        {
            string connectionString = "Data Source=LOCALHOST\\DEVELOPER;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand(); 
            this.com.Connection = this.cn;
            string sql = "SELECT * FROM ENFERMO";
            //CREAMOS EL ADAPTADOR PUENTE ENTRE SQL SERVER Y LINQ
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            this.tablaEnfermos = new DataTable();
            //TRAEMOS LOS DATOS PARA LINQ
            ad.Fill(this.tablaEnfermos);
        }
        public List<Enfermo> GetEnfermos()
        {
            var consulta = from datos in this.tablaEnfermos.AsEnumerable()
                           select datos;
            List<Enfermo> enfermos = new List<Enfermo>();
            foreach (var row in consulta)
            {
                Enfermo enfermo = new Enfermo();
                enfermo.Inscripcion = row.Field<string>("INSCRIPCION");
                enfermo.Apellido = row.Field<string>("APELLIDO");
                enfermo.Direccion = row.Field<string>("DIRECCION");
                enfermo.FechaNacimiento = row.Field<DateTime>("FECHA_NAC");
                enfermo.sexo = row.Field<string>("S");
                enfermo.NumeroSeguridad = row.Field<string>("NSS");
                enfermos.Add(enfermo);
            }
            return enfermos;
        }
        public Enfermo GetEnfermoInscripcion(string inscripcion)
        {
            var consulta = from datos in this.tablaEnfermos.AsEnumerable()
                           where datos.Field<string>("INSCRIPCION") == inscripcion
                           select datos;
            var row = consulta.First();
            Enfermo enfermo = new Enfermo();
            enfermo.Inscripcion = row.Field<string>("INSCRIPCION");
            enfermo.Apellido = row.Field<string>("APELLIDO");
            enfermo.Direccion = row.Field<string>("DIRECCION");
            enfermo.FechaNacimiento = row.Field<DateTime>("FECHA_NAC");
            enfermo.sexo = row.Field<string>("S");
            enfermo.NumeroSeguridad = row.Field<string>("NSS");
            return enfermo;
        }
        public async Task<int> DeleteEmpleado(string inscripcion)
        {
            string sql = "DELETE FROM ENFERMO WHERE INSCRIPCION=@INSCRIPCION";
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.com.Parameters.AddWithValue("@INSCRIPCION", inscripcion);
            await this.cn.OpenAsync();
            int registros = await this.com.ExecuteNonQueryAsync();
            this.com.Parameters.Clear();
            await this.cn.CloseAsync();
            return registros;
        }
    }
}
