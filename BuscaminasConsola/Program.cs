using Buscaminas;
using MenusConsola;

namespace BuscaminasConsola;

class Program
{
    static void Main(string[] args)
    {
        Juego juego = new();
        juego.MostrarMenuInicial();
    }
}
