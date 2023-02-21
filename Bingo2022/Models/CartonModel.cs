namespace Bingo2022.Models
{
    public class CartonModel
    {
        public CartonModel(int numeroDeCarton, NumeroModel[,] numeros)
        {
            NumeroDeCarton= numeroDeCarton;
            Numeros = numeros;

        }
        public DateTime FechaDeCreacion { get; set; } = DateTime.Now;
        public int NumeroDeCarton { get; set; }
        public NumeroModel[,] Numeros { get; set; }
        public bool Ganador { get; set; } = false;
        public int Aciertos { get; set; }

    }
}
