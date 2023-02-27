using Bingo2022.Data;
using Bingo2022.Models;
using Dapper;
using System.Data.SqlClient;

namespace Bingo2022.Rules
{
    public class BolilleroRule
    {
        //Conexión a la BD.
        private readonly IConfiguration _configuration;
        public BolilleroRule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //Obtengo aleatoriamente los numeros del bolillero. 
        public List<int> GenerarBolillero ()
        {

            var genRandom = new Random(DateTime.Now.Millisecond);
            var bolillas = new List<int>();
            var bolillaNva = 0;
            var contador = 0;

            while (contador <= 89)
            {
                bolillaNva = genRandom.Next(1, 91);
                if (!bolillas.Contains(bolillaNva))
                {
                    contador++;
                    bolillas.Add(bolillaNva);
                }
            }

            //Console.WriteLine($"Lengh: {bolillas.Count}");


            return bolillas;
        }

        //Guardar en el historial de bolillero.
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
