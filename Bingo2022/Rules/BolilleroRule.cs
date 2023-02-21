namespace Bingo2022.Rules
{
    public class BolilleroRule
    {

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

            Console.WriteLine($"Lengh: {bolillas.Count}");


            return bolillas;
        }
    }
}
