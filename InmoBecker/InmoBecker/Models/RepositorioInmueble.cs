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
				string sql = $"SELECT IdInmueble, Direccion, Ambientes, Superficie, PropietarioId,"+
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
							PropietarioId = reader.GetInt32(4),
							Duenio = new Propietario
							{
								IdPropietario = reader.GetInt32(4),
								Nombre = reader.GetString(5),
								Apellido = reader.GetString(6),
								
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
				string sql = $"INSERT INTO Inmuebles (Direccion, Ambientes, Superficie, PropietarioId) " +
					"VALUES (@direccion, @ambientes, @superficie, @propietarioId);" +
					"SELECT SCOPE_IDENTITY();";
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@direccion", i.Direccion);
					command.Parameters.AddWithValue("@ambientes", i.Ambientes);
					command.Parameters.AddWithValue("@superficie", i.Superficie);
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
				string sql = $"SELECT IdInmueble, Direccion, Ambientes, Superficie, PropietarioId, p.Nombre, p.Apellido" +
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
							PropietarioId = reader.GetInt32(4),
							Duenio = new Propietario
							{
								IdPropietario = reader.GetInt32(4),
								Nombre = reader.GetString(5),
								Apellido = reader.GetString(6),
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
					"Direccion=@direccion, Ambientes=@ambientes, Superficie=@superficie, PropietarioId=@propietarioId " +
					"WHERE IdInmueble = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@direccion", i.Direccion);
					command.Parameters.AddWithValue("@ambientes", i.Ambientes);
					command.Parameters.AddWithValue("@superficie", i.Superficie);
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

	}

}
