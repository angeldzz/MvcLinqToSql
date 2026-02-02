using Microsoft.Data.SqlClient;
using MvcLinqToSql.Models;
using System.Data;

namespace MvcLinqToSql.Repositories
{
    public class RepositoryEmpleado
    {
        private DataTable tablaEmpleados;
        public RepositoryEmpleado()
        {
            string connectionString = "Data Source=LOCALHOST\\DEVELOPER;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            string sql = "SELECT * FROM EMP";
            //CREAMOS EL ADAPTADOR PUENTE ENTRE SQL SERVER Y LINQ
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            this.tablaEmpleados = new DataTable();
            //TRAEMOS LOS DATOS PARA LINQ
            ad.Fill(this.tablaEmpleados);
        }
        public List<Empleado> GetEmpleados()
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           select datos;
            //AHORA MISMO TENEMOS DENTRO DE LA CONSULTA LA INFORMACION DE LOS EMPELADOS
            // DE LOS EMPLEADOS. LOS DATOS VIENEN EN FORMATO TABLA, CADA ELEMENTO
            // ES UNA FILA LLAMADA DATAROW
            //DEBEMOS RECORRER CADA FILA Y CONVERTIRLA A UN OBJETO EMPLEADO
            List<Empleado> empleados = new List<Empleado>();
            foreach (var row in consulta)
            {
                // para extraer los datos de un datarow usamos el nombre de la columna
                //DataRow.Field<TIPODATO>("NOMBRECOLUMNA")
                Empleado empleado = new Empleado();
                empleado.IdEmpleado = row.Field<int>("EMP_NO");
                empleado.Apellido = row.Field<string>("APELLIDO");
                empleado.Oficio = row.Field<string>("OFICIO");
                empleado.Salario = row.Field<int>("SALARIO");
                empleado.IdDepartamento = row.Field<int>("DEPT_NO");
                empleados.Add(empleado);
            }
            return empleados;
        }
        public Empleado FindEmpleado(int idEmpleado)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<int>("EMP_NO") == idEmpleado
                           select datos;
            //Nosotros sabemos que esta consulta nos devuelve una sola fila
            //pero linq nos devuelve un conjunto
            //dentro de este conjunto tenemos metodos LAMBDA
            //para hacer ciertas cosas
            //por ejemplo, podriamos contar, podriamos saber el maximo, minimo, etc
            var row = consulta.First();
            Empleado empleado = new Empleado();
            empleado.IdEmpleado = row.Field<int>("EMP_NO");
            empleado.Apellido = row.Field<string>("APELLIDO");
            empleado.Oficio = row.Field<string>("OFICIO");
            empleado.Salario = row.Field<int>("SALARIO");
            empleado.IdDepartamento = row.Field<int>("DEPT_NO");
            return empleado;
        }
        public List<Empleado> GetEmpleadosOficioSalario(string oficio, int salario)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio &&
                                 datos.Field<int>("SALARIO") >= salario
                                 orderby datos.Field<int>("SALARIO")
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                List<Empleado> empleados = new List<Empleado>();
                foreach (var row in consulta)
                {
                    Empleado emp = new Empleado
                    {
                        IdEmpleado = row.Field<int>("EMP_NO"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Oficio = row.Field<string>("OFICIO"),
                        Salario = row.Field<int>("SALARIO"),
                        IdDepartamento = row.Field<int>("DEPT_NO")
                    };
                    empleados.Add(emp);
                }

                return empleados;
            }
        }
        public ResumenEmpleados GetEmpleadosOficio(string oficio)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            consulta.OrderBy(x => x.Field<int>("SALARIO"));
            int personas = consulta.Count();
            int maximo = consulta.Max(x => x.Field<int>("SALARIO"));
            double media = consulta.Average(x => x.Field<int>("SALARIO"));
            List<Empleado> empleados = new List<Empleado>();
            foreach (var row in consulta)
            {
                Empleado emp = new Empleado
                {
                    IdEmpleado = row.Field<int>("EMP_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Oficio = row.Field<string>("OFICIO"),
                    Salario = row.Field<int>("SALARIO"),
                    IdDepartamento = row.Field<int>("DEPT_NO")
                };
                empleados.Add(emp);
            }
            ResumenEmpleados resumenEmpleados = new ResumenEmpleados();
            resumenEmpleados.Personas = personas;
            resumenEmpleados.MaximoSalario = maximo;
            resumenEmpleados.MediaSalarial = media;
            resumenEmpleados.Empleados = empleados;
            return resumenEmpleados;
        }
        public List<string> GetOficios()
        {
            var consulta = (from datos in 
                               this.tablaEmpleados.AsEnumerable()
                           select datos.Field<string>("OFICIO")).Distinct();
            return consulta.ToList();
        }
    }
}
