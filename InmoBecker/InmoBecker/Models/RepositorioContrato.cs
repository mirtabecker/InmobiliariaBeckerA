using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Models
{
    public class RepositorioContrato : RepositorioBase
    {
        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {

        }

		public List<Contrato> ObtenerTodos()
		{
			List<Contrato> res = new List<Contrato>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT IdContrato, Monto, FechaInicio, FechaCierre, InquilinoId, InmuebleId, i.Nombre, i.Apellido, n.Direccion " +
					" FROM Contratos c INNER JOIN Inquilinos i ON c.InquilinoId = i.IdInquilino INNER JOIN Inmuebles n ON c.InmuebleId = n.IdInmueble";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
                    var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato c = new Contrato
						{
							IdContrato = reader.GetInt32(0),
							Monto = reader.GetInt32(1),
							FechaInicio = reader.GetDateTime(2),
							FechaCierre = reader.GetDateTime(3),
							InquilinoId = reader.GetInt32(4),
							InmuebleId = reader.GetInt32(5),

							Inquilino = new Inquilino
							{
								Nombre = reader.GetString(6),
								Apellido = reader.GetString(7),
							},
							Inmueble = new Inmueble
							{
								Direccion = reader.GetString(8),
								
							}

						};	
						res.Add(c);
					}
					connection.Close();
				}
			}
			return res;
		}

		public int Alta(Contrato c)
        {
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
            {
				string sql = $"INSERT INTO Contratos (Monto, FechaInicio, FechaCierre, InquilinoId, InmuebleId)" +
							 "VALUES (@monto, @inicio, @fin, @inquilinoId, @inmuebleId);" +
							 "SELECT SCOPE_IDENTITY();";
				using (var command = new SqlCommand(sql, connection))
                {
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@monto", c.Monto);
					command.Parameters.AddWithValue("@inicio", c.FechaInicio);
					command.Parameters.AddWithValue("@fin", c.FechaCierre);
					command.Parameters.AddWithValue("@inquilinoId", c.InquilinoId);
					command.Parameters.AddWithValue("@inmuebleId", c.InmuebleId);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					c.IdContrato = res;
					connection.Close();
				}
            }
			return res;
        }
		public Contrato ObtenerPorId(int id)
		{
			Contrato c = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdContrato, Monto, FechaInicio, FechaCierre, InquilinoId, InmuebleId, i.Nombre, i.Apellido, n.Direccion " +
					" FROM Contratos c INNER JOIN Inquilinos i ON c.InquilinoId = i.IdInquilino INNER JOIN Inmuebles n ON c.InmuebleId = n.IdInmueble" +
					$" WHERE IdContrato=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						c = new Contrato
						{
							IdContrato = reader.GetInt32(0),
							Monto = reader.GetInt32(1),
							FechaInicio = reader.GetDateTime(2),
							FechaCierre = reader.GetDateTime(3),
							InquilinoId = reader.GetInt32(4),
							InmuebleId = reader.GetInt32(5),

							Inquilino = new Inquilino
							{
								Nombre = reader.GetString(6),
								Apellido = reader.GetString(7),
							},
							Inmueble = new Inmueble
							{
								Direccion = reader.GetString(8),		
							}

						};

					}
					connection.Close();
				}
			}
			return c;
		}
		public int Modificacion(Contrato c)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "UPDATE Contratos SET " +
					 "Monto=@monto, FechaInicio=@inicio, FechaCierre=@fin, InquilinoId=@inquilinoId, InmuebleId=@inmuebleId "+
					"WHERE IdContrato = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					
					command.Parameters.AddWithValue("@monto", c.Monto);
					command.Parameters.AddWithValue("@inicio", c.FechaInicio);
					command.Parameters.AddWithValue("@fin", c.FechaCierre);
					command.Parameters.AddWithValue("@inquilinoId", c.InquilinoId);
					command.Parameters.AddWithValue("@inmuebleId", c.InmuebleId);
					command.Parameters.AddWithValue("@id", c.IdContrato);
					command.CommandType = CommandType.Text;
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
				string sql = $"DELETE FROM Contratos WHERE IdContrato = {id}";
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
		public List<Contrato> BuscarPorInmueble(int id)
		{
			List<Contrato> res = new List<Contrato>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT IdContrato, Monto, FechaInicio, FechaCierre, InquilinoId, InmuebleId, i.Nombre, i.Apellido, p.Direccion " +
					" FROM Contratos c INNER JOIN Inmuebles p ON c.InmuebleId = p.IdInmueble INNER JOIN Inquilinos i ON c.InquilinoId = i.IdInquilino " +
					" WHERE InmuebleId=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato entidad = new Contrato
						{
							IdContrato = reader.GetInt32(0),
							Monto = reader.GetInt32(1),
							FechaInicio = reader.GetDateTime(2),
							FechaCierre = reader.GetDateTime(3),
							InquilinoId = reader.GetInt32(4),
							InmuebleId = reader.GetInt32(5),

							Inquilino = new Inquilino
							{
								Nombre = reader.GetString(6),
								Apellido = reader.GetString(7),
							},
							Inmueble = new Inmueble
							{
								Direccion = reader.GetString(8),


							}
						};
						res.Add(entidad);
					}
					connection.Close();
				}
			}
			return res;
		}
		public List<Contrato> ContradosVigentes(DateTime fecha)
		{
			List<Contrato> res = new List<Contrato>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "SELECT IdContrato, Monto, FechaInicio, FechaCierre, InquilinoId, i.Nombre, i.Apellido, InmuebleId, p.Direccion" +
					" FROM Contratos c INNER JOIN Inquilinos i ON c.InquilinoId = i.IdInquilino INNER JOIN Inmuebles p ON c.InmuebleId = p.IdInmueble" +
					" WHERE @fecha BETWEEN FechaInicio AND FechaCierre ";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@fecha", SqlDbType.Date).Value = fecha.Date;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato c = new Contrato
						{
							IdContrato = reader.GetInt32(0),
							Monto = reader.GetInt32(1),
							FechaInicio = reader.GetDateTime(2),
							FechaCierre = reader.GetDateTime(3),
							InquilinoId = reader.GetInt32(4),
							InmuebleId = reader.GetInt32(7),

							Inquilino = new Inquilino
							{
								Nombre = reader.GetString(5),
								Apellido = reader.GetString(6),
							},
							Inmueble = new Inmueble
							{
								Direccion = reader.GetString(8),

							}
						};
						res.Add(c);
					}
					connection.Close();
				}
			}
			return res;
		}
	}
}
 