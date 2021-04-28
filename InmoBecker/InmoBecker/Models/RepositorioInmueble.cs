using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Models
{
    public class RepositorioInmueble : RepositorioBase
    {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration)
        {

        }

		public List<Inmueble> ObtenerTodos()
		{
			List<Inmueble> res = new List<Inmueble>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdInmueble, Direccion, Ambientes, Superficie, Tipo, Precio, Estado, PropietarioId,"+
					" p.Nombre, p.Apellido" +
					" FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.IdPropietario" ;
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble i = new Inmueble
						{
							IdInmueble = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Ambientes = reader.GetInt32(2),
							Superficie = reader.GetInt32(3),
							Tipo = reader.GetString(4),
							Precio = reader.GetInt32(5),
							Estado = reader.GetInt32(6),
							PropietarioId = reader.GetInt32(7),
							Duenio = new Propietario
							{
								IdPropietario = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),								
							}
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}
		public int Alta(Inmueble i)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Inmuebles (Direccion, Ambientes, Superficie, Tipo, Precio, Estado, PropietarioId) " +
					"VALUES (@direccion, @ambientes, @superficie, @tipo, @Precio, @estado, @propietarioId);" +
					"SELECT SCOPE_IDENTITY();";
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@direccion", i.Direccion);
					command.Parameters.AddWithValue("@ambientes", i.Ambientes);
					command.Parameters.AddWithValue("@superficie", i.Superficie);
					command.Parameters.AddWithValue("@tipo", i.Tipo);
					command.Parameters.AddWithValue("@Precio", i.Precio);
					command.Parameters.AddWithValue("@estado", i.Estado);
					command.Parameters.AddWithValue("@propietarioId", i.PropietarioId);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					i.IdInmueble = res;
					connection.Close();
				}
			}
			return res;
		}
		public Inmueble ObtenerPorId(int id)
		{
			Inmueble i = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdInmueble, Direccion, Ambientes, Superficie, Tipo, Precio, Estado, PropietarioId, p.Nombre, p.Apellido" +
					$" FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.IdPropietario" +
					$" WHERE IdInmueble=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Inmueble
						{
							IdInmueble = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Ambientes = reader.GetInt32(2),
							Superficie = reader.GetInt32(3),
							Tipo = reader.GetString(4),
							Precio = reader.GetInt32(5),
							Estado = reader.GetInt32(6),
							PropietarioId = reader.GetInt32(7),
							Duenio = new Propietario
							{
								IdPropietario = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							}
						};
					}
					connection.Close();
				}
			}
			return i;
		}
		public int Modificacion(Inmueble i)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "UPDATE Inmuebles SET " +
					"Direccion=@direccion, Ambientes=@ambientes, Superficie=@superficie, Tipo=@tipo, Precio=@precio, Estado=@estado, PropietarioId=@propietarioId " +
					"WHERE IdInmueble = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@direccion", i.Direccion);
					command.Parameters.AddWithValue("@ambientes", i.Ambientes);
					command.Parameters.AddWithValue("@superficie", i.Superficie);
					command.Parameters.AddWithValue("@tipo", i.Tipo);
					command.Parameters.AddWithValue("@precio", i.Precio);
					command.Parameters.AddWithValue("@estado", i.Estado);
					command.Parameters.AddWithValue("@propietarioId", i.PropietarioId);
					command.Parameters.AddWithValue("@id", i.IdInmueble);
					
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public int Eliminar(int id)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"DELETE FROM Inmuebles WHERE IdInmueble = {id}";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public List<Inmueble> BuscarPorPropietario(int id)
		{
			List<Inmueble> res = new List<Inmueble>();
			Inmueble e = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdInmueble, Direccion, Ambientes, Superficie, Tipo, Precio, Estado, PropietarioId, p.Nombre, p.Apellido" +
					$" FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.IdPropietario" +
					$" WHERE PropietarioId = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						e = new Inmueble
						{
							IdInmueble = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Ambientes = reader.GetInt32(2),
							Superficie = reader.GetInt32(3),
							Tipo = reader.GetString(4),
							Precio = reader.GetInt32(5),
							Estado = reader.GetInt32(6),
							PropietarioId = reader.GetInt32(7),
							Duenio = new Propietario
							{
								IdPropietario = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							}

						};
						res.Add(e);
					}
					connection.Close();
				}
			}
			return res;
		}
		public IList<Inmueble> BuscarPorFechas(DateTime ingreso, DateTime salida)
		{
			List<Inmueble> res = new List<Inmueble>();
			Inmueble entidad = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdInmueble, Direccion, Ambientes, Superficie, Tipo, Precio, Estado, PropietarioId, p.Nombre, p.Apellido" +
					$" FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.IdPropietario LEFT JOIN Contratos c ON i.IdInmueble = c.InmuebleId " +
					$" WHERE FechaInicio > @salida OR FechaCierre < @ingreso OR c.IdContrato IS NULL AND i.Estado = 1";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@ingreso", SqlDbType.Date).Value = ingreso.Date;
					command.Parameters.Add("@salida", SqlDbType.Date).Value = salida.Date;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						entidad = new Inmueble
						{
							IdInmueble = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Ambientes = reader.GetInt32(2),
							Superficie = reader.GetInt32(3),
							Tipo = reader.GetString(4),
							Precio = reader.GetInt32(5),
							Estado = reader.GetInt32(6),
							PropietarioId = reader.GetInt32(7),
							Duenio = new Propietario
							{
								IdPropietario = reader.GetInt32(7),
								Nombre = reader.GetString(8),
								Apellido = reader.GetString(9),
							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}

	}

}
