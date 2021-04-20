using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Models
{
    public class RepositorioPago : RepositorioBase
    {
        public RepositorioPago(IConfiguration configuration) : base(configuration)
        {

        }

		public List<Pago> ObtenerTodos()
		{
			List<Pago> res = new List<Pago>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdPago, NroPago, Fecha, Importe, ContratoId, c.Monto, c.InmuebleId, c.InquilinoId" +
					$" FROM Pagos p INNER JOIN Contratos c ON p.ContratoId = c.IdContrato";

				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago p = new Pago
						{
							IdPago = reader.GetInt32(0),
							NroPago = reader.GetInt32(1),
							Fecha = reader.GetDateTime(2),
							Importe = reader.GetDecimal(3),
							ContratoId = reader.GetInt32(4),
							Alquiler = new Contrato
							{
								IdContrato = reader.GetInt32(4),
								Monto = reader.GetInt32(5),
								InmuebleId = reader.GetInt32(6),
								InquilinoId = reader.GetInt32(7),
							}
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}
		public int Alta(Pago p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"INSERT INTO Pagos (NroPago, Fecha, Importe, ContratoId) " +
					"VALUES (@nroPago, @fecha, @importe, @contratoId);" +
					"SELECT SCOPE_IDENTITY();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
				using (var command = new SqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nroPago", p.NroPago);
					command.Parameters.AddWithValue("@fecha", p.Fecha);
					command.Parameters.AddWithValue("@importe", p.Importe);
					command.Parameters.AddWithValue("@contratoId", p.ContratoId);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					p.IdPago = res;
					connection.Close();
				}
			}
			return res;
		}
		public Pago ObtenerPorId(int id)
		{
			Pago p = null;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = $"SELECT IdPago, NroPago, Fecha, Importe, ContratoId, c.Monto, c.InmuebleId, c.InquilinoId" +
					$" FROM Pagos p INNER JOIN Contratos c ON p.ContratoId = c.IdContrato" +
					$" WHERE p.IdPago=@id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", SqlDbType.Int).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Pago
						{
							IdPago = reader.GetInt32(0),
							NroPago = reader.GetInt32(1),
							Fecha = reader.GetDateTime(2),
							Importe = reader.GetDecimal(3),
							ContratoId = reader.GetInt32(4),
							Alquiler = new Contrato
							{
								IdContrato = reader.GetInt32(4),
								Monto = reader.GetInt32(5),
								InmuebleId = reader.GetInt32(6),
								InquilinoId = reader.GetInt32(7),
							}
						};
					}
					connection.Close();
				}
			}
			return p;
		}
		public int Modificacion(Pago p)
		{
			int res = -1;
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				string sql = "UPDATE Pagos SET " +
					"NroPago=@pago, Fecha=@fecha, Importe=@importe, ContratoId=@contratoId " +
					"WHERE IdPago = @id";
				using (SqlCommand command = new SqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@pago", p.NroPago);
					command.Parameters.AddWithValue("@fecha", p.Fecha);
					command.Parameters.AddWithValue("@importe", p.Importe);
					command.Parameters.AddWithValue("@contratoId", p.ContratoId);
					command.Parameters.AddWithValue("@id", p.IdPago);
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
				string sql = $"DELETE FROM Pagos WHERE Idpago = {id}";
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
