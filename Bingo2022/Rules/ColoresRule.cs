namespace Bingo2022.Rules
{
    public class ColoresRule
    {
        //Método para asignar colores aleatorios a la bolilla.
        public string ColorAleatorio ()
        {
            var genRandom = new Random(DateTime.Now.Millisecond);

            //Opciones de colores.
            string[] colores = { "primary", "secondary", "success", "danger", "warning", "info" };
            
            //Obtengo una posición del array de manera aleatoria.
            var color = genRandom.Next(0, 6);
            
            
            return colores[color];

        }

    }
}
