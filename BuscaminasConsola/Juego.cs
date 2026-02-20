using System;
using Buscaminas;
using MenusConsola;

namespace BuscaminasConsola;

public class Juego
{
    public Juego(){}

    //--------------------------------------------------------------------------------------
    public void MostrarMenuInicial()
    {
        Menu menuPrincipal = new("¡¡BUSCAMINAS!!", "Versión consola");

        menuPrincipal.IncorporarOpcion("Jugar partida", () => MostrarMenuDificultad());
        menuPrincipal.IncorporarOpcion("Salir", () => { });

        int iOpcion;
        do
        {
            iOpcion = menuPrincipal.MostrarMenu();
            if (iOpcion < menuPrincipal.NumOpciones-1) menuPrincipal.EjecutarOpcion(iOpcion);
        }
        while (iOpcion < menuPrincipal.NumOpciones-1);

        Console.WriteLine("Bye!");
    }

    //--------------------------------------------------------------------------------------
    void MostrarMenuDificultad()
    {
        Menu menuDificultad = new("Buscaminas", "Nivel de dificultad");

        menuDificultad.IncorporarOpcion("Fácil", () => EjecutarPartida(7, 9, 10));
        menuDificultad.IncorporarOpcion("Normal", () => EjecutarPartida(10, 13, 20));
        menuDificultad.IncorporarOpcion("Difícil", () => EjecutarPartida(14, 18, 40));
        menuDificultad.IncorporarOpcion("Volver", () => { });

        int iOpcion = menuDificultad.MostrarMenu();
        if (iOpcion < menuDificultad.NumOpciones-1) menuDificultad.EjecutarOpcion(iOpcion);
    }

    //--------------------------------------------------------------------------------------
    void EjecutarPartida(int filas = 14, int columnas = 18, int numMinas = 40)
    {
        Tablero tablero = new(
            filas: filas,
            columnas: columnas,
            numMinas: numMinas
        );

        BuscaminasUI ui = new(tablero);
        int i, j;

        bool hasMuerto = false, juegoAbortado = false, marcar;

        do
        {
            ui.MostrarTablero();
            (i, j, marcar, juegoAbortado) = ui.PedirComando();
            if (!juegoAbortado)
            {
                if (marcar) tablero.MarcarCasilla(i, j);
                else hasMuerto = tablero.BuscarMina(i, j);
            }
        } while (!juegoAbortado && !hasMuerto);

        if (juegoAbortado)
        {
            Console.WriteLine("Partida abortada, presiona una tecla para volver al menú principal...");
        }
        else // Has muerto
        {
            ui.MostrarTablero();
            if (hasMuerto) Console.WriteLine("Has muerto!");
            Console.WriteLine("Presiona una tecla para volver al menú principal...");
        }

        Console.ReadKey();
    }
}
