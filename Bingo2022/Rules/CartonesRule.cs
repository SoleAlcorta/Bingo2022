using Bingo2022.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Bingo2022.Rules
{
    public class CartonesRule
    {
        public List<NumeroModel[,]> ObtenerNumeros()
        {
            var genRandom = new Random(DateTime.Now.Millisecond);
            List<int[,]> cartones = new List<int[,]>();

            //Hago una lista donde guardarlos como Model?
            List<NumeroModel[,]> numerosModel = new List<NumeroModel[,]>();

            for (int vuelta = 0; vuelta < 4; vuelta++)
            {
                var carton = new int[3, 9];

                var convertidos = new NumeroModel[3, 9];

                Console.WriteLine(convertidos[0,0]);

                //Obtengo los números del cartón
                for (int c = 0; c < 9; c++)
                {
                    for (int f = 0; f < 3; f++)
                    {
                        int nuevoNumero = 0;
                        bool encontreUnoNuevo = false;
                        while (!encontreUnoNuevo)
                        {
                            if (c == 0)
                            {
                                nuevoNumero = genRandom.Next(1, 10);

                            }
                            else if (c == 8)
                            {
                                nuevoNumero = genRandom.Next(80, 91);

                            }
                            else
                            {
                                nuevoNumero = genRandom.Next(c * 10, c * 10 + 10);

                            }

                            //Buscamos si el nvo numero existe en la columna
                            encontreUnoNuevo = true;
                            for (int f2 = 0; f2 < 3; f2++)
                            {
                                if (carton[f2, c] == nuevoNumero)
                                {
                                    encontreUnoNuevo = false;
                                    break;
                                }
                            }

                        }
                        carton[f, c] = nuevoNumero;
                    }
                }

                //Ordenamos las columnas
                for (int c = 0; c < 9; c++)
                {
                    for (int f = 0; f < 3; f++)
                    {
                        for (int k = f + 1; k < 3; k++)
                        {
                            if (carton[f, c] > carton[k, c])
                            {
                                int aux = carton[f, c];
                                carton[f, c] = carton[k, c];
                                carton[k, c] = aux;
                            }
                        }
                    }
                }

                //Generar los espacios vacios
                var borrados = 0;
                while (borrados < 12)
                {
                    var filaABorrar = genRandom.Next(0, 3);
                    var columnaABorrar = genRandom.Next(0, 9);


                    if (carton[filaABorrar, columnaABorrar] == 0)
                    {
                        continue;

                    }

                    var cerosEnFila = 0;
                    for (int c = 0; c < 9; c++)
                    {
                        if (carton[filaABorrar, c] == 0)
                        {
                            cerosEnFila++;
                        }
                    }

                    var cerosEnColumna = 0;
                    for (int f = 0; f < 3; f++)
                    {
                        if (carton[f, columnaABorrar] == 0)
                        {
                            cerosEnColumna++;
                        }
                    }

                    var itemsPorColumna = new int[9];
                    for (int c = 0; c < 9; c++)
                    {
                        for (int f = 0; f < 3; f++)
                        {
                            if (carton[f, c] != 0)
                            {
                                itemsPorColumna[c]++;
                            }
                        }
                    }

                    var columnasConUnSoloNumero = 0;
                    for (int c = 0; c < 9; c++)
                    {
                        if (itemsPorColumna[c] == 1)
                        {
                            columnasConUnSoloNumero++;
                        }
                    }

                    if (cerosEnFila == 4 || cerosEnColumna == 2)
                    {
                        continue;
                    }

                    if (columnasConUnSoloNumero == 3 && itemsPorColumna[columnaABorrar] != 3)
                    {
                        continue;
                    }

                    carton[filaABorrar, columnaABorrar] = 0;
                    borrados++;

                }

                //cartones.Add(carton);

                //Recorro los int y los convierto en Model
                for (int fil = 0; fil < 3; fil++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        var nuevoNModel = new NumeroModel(carton[fil, col]);
                        convertidos[fil, col] = nuevoNModel;
                    }
                }

                numerosModel.Add(convertidos);


            }
            return numerosModel;
        }

        //Completar estos datos y leeeesto.
        public void GuardarHistorialCartones(HistorialCartones data)
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
