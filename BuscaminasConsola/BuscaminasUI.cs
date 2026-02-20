using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Buscaminas;

namespace BuscaminasConsola;

public class BuscaminasUI(Tablero tablero)
{
    Tablero Tablero { get; init; } = tablero;

    public void MostrarTablero()
    {
        Console.Clear();

        Console.Write($"    ");
        for (char letraCol = 'A'; letraCol < 'A' + Tablero.Columnas; letraCol++)
        {
            Console.Write($" {letraCol} ");
        }
        Console.WriteLine();

        for (int i = 0; i < Tablero.Filas; i++)
        {
            Console.Write($" {i + 1,-2} ");
            for (int j = 0; j < Tablero.Columnas; j++)
            {
                Casilla c = Tablero.Casillas[i, j];
                Console.BackgroundColor = ColorTraseroPara(c);
                Console.ForegroundColor = ColorFrontalPara(c);
                Console.Write($" {c} ");
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        Console.WriteLine();
    }

    ConsoleColor ColorFrontalPara(Casilla c)
    {
        if (c.Marcada) return ConsoleColor.Red;
        if (!c.Revelada) return ConsoleColor.White;
        if (c.TieneMina) return ConsoleColor.Yellow;
        if (c.MinasAdyacentes > 0) return ConsoleColor.Green;
        return ConsoleColor.Green;
    }

    ConsoleColor ColorTraseroPara(Casilla c)
    {
        if (c.Marcada) return ConsoleColor.DarkGray;
        if (!c.Revelada) return ConsoleColor.Green;
        if (c.TieneMina) return ConsoleColor.Red;
        if (c.MinasAdyacentes > 0) return ConsoleColor.Black;
        return ConsoleColor.Black;
    }

    public (int i, int j, bool marcar, bool salir) PedirComando()
    {
        int i = 0;
        int j = 0;
        bool marcar = false;

        bool esLecturaCorrecta = false, salir = false;
        Regex patronEntrada = new(@"^([mM]?)(\d*)([a-zA-Z]+)$");

        do
        {
            Console.Write("Indica fila y columna, M delante para marcar (Q para salir): ");
            String rawInput = (Console.ReadLine() ?? "").Trim();

            if (rawInput.ToUpper() != "Q")
            {
                try
                {
                    if (!patronEntrada.IsMatch(rawInput)) throw new InvalidDataException("Formato de entrada erróneo.");
                    Match match = patronEntrada.Match(rawInput);

                    marcar = match.Groups[1].Value.ToUpper() == "M";

                    i = int.Parse(match.Groups[2].Value) - 1;
                    if (i < 0 || i >= Tablero.Filas) throw new InvalidDataException("Fila fuera de rango");

                    j = match.Groups[3].Value.ToUpper()[0] - 'A';
                    if (j < 0 || j >= Tablero.Columnas) throw new InvalidDataException("Columna fuera de rango");

                    esLecturaCorrecta = true;
                }
                catch (InvalidDataException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception)
                {
                    Console.WriteLine("Comando inválido.");
                }
            }
            else
            {
                salir = true;
            }
        } while (!salir && !esLecturaCorrecta);

        return (i, j, marcar, salir);
    }
}
