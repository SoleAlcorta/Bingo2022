using Bingo2022.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Bingo2022.Data
{
    public class BingoData
    {
        private readonly IConfiguration _configuration;
        public BingoData(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    
        //Método para guardar la bolilla en el historial
        public void GuardarHistorialBolillero(HistorialBolillero data)
        {
            var connectionString = _configuration.GetConnectionString("BingoDatabase");
            using var connection = new SqlConnection(connectionString);
            {
                connection.Open();

                var queryInsert = "INSERT INTO HistorialBolillero(fecha, numBolilla) Values(@fecha, @numBolilla)";
                var result = connection.Execute(queryInsert, new
                {
                    fecha = data.Fecha,
                    numBolilla = data.NumBolilla,
                });
            }
        }

    }
}
