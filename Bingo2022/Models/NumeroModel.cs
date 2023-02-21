namespace Bingo2022.Models
{
    public class NumeroModel
    {
        public NumeroModel(int n)
        {
            Numero = n;
            Salio = false;
        }
        public int Numero { get; set; }
        public bool Salio { get; set; }

    }

}
