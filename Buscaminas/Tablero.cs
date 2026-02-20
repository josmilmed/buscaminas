using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace Buscaminas;
public class Tablero
{
    public int Filas { get; }
    public int Columnas { get; }
    public int NumMinas { get; }

    private Casilla[,]? _casillas = null;
    public Casilla[,] Casillas
    {
        get
        {
            if (_casillas == null) throw new InvalidDataException();
            return _casillas;
        }
    }
    bool esPrimeraJugada = true;

    public Tablero(int filas, int columnas, int numMinas)
    {
        Filas = filas;
        Columnas = columnas;
        NumMinas = numMinas;

        Inicializar();
    }

    public void Reset()
    {
        Inicializar();
    }

    private void Inicializar()
    {
        esPrimeraJugada = true;
        _casillas = new Casilla[Filas, Columnas];
        for (int i = 0; i < Filas; i++)
        {
            for (int j = 0; j < Columnas; j++)
            {
                Casillas[i, j] = new Casilla(this, i, j);
            }
        }
    }

    private void ColocarMinas(int numMinas, int iInicial, int jInicial)
    {
        var rand = new Random();
        for (int i = 0; i < numMinas; i++)
        {
            int fila, columna;
            do
            {
                fila = rand.Next(Filas);
                columna = rand.Next(Columnas);
            } while (
                EsPosicionAdyacente(fila, columna, iInicial, jInicial, 3) ||
                Casillas[fila, columna].TieneMina
            );

            Casillas[fila, columna].TieneMina = true;
        }

        CalcularMinasAdyacentes();
    }

    private void CalcularMinasAdyacentes()
    {
        for (int i = 0; i < Filas; i++)
        {
            for (int j = 0; j < Columnas; j++)
            {
                if (!Casillas[i, j].TieneMina)
                {
                    int minas = 0;
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            int ni = i + x, nj = j + y;
                            if (ni >= 0 && ni < Filas && nj >= 0 && nj < Columnas && Casillas[ni, nj].TieneMina)
                            {
                                minas++;
                            }
                        }
                    }

                    Casillas[i, j].MinasAdyacentes = minas;
                }
            }
        }
    }

    private bool EsPosicionAdyacente(int iPos, int jPos, int iObjetivo, int jObjetivo, int radio = 1) =>
            iObjetivo - radio <= iPos && iPos <= iObjetivo + radio &&
            jObjetivo - radio <= jPos && jPos <= jObjetivo + radio;

    public bool EsPosicionValida(int i, int j) => i >= 0 && i < Filas && j >= 0 && j < Columnas;

    public bool BuscarMina(int fila, int columna)
    {
        if (fila < 0 || fila >= Filas) throw new ArgumentOutOfRangeException(paramName: nameof(fila));

        if (columna < 0 || columna >= Columnas) throw new ArgumentOutOfRangeException(paramName: nameof(columna));

        if (esPrimeraJugada)
        {
            ColocarMinas(NumMinas, fila, columna);
            esPrimeraJugada = false;
        }

        Casilla c = Casillas[fila, columna];

        if (c.Marcada || c.Revelada) return false;

        c.Revelada = true;

        if (c.TieneMina) return true;
        if (!c.EsVacia) return false;

        for (int i = fila - 1; i <= fila + 1; i++)
        {
            for (int j = columna - 1; j <= columna + 1; j++)
            {
                if (EsPosicionValida(i, j))
                {
                    c = Casillas[i, j];
                    if (!c.Revelada)
                    {
                        if (c.EsVacia) BuscarMina(i, j);
                        else if (!c.TieneMina) c.Revelada = true;
                    }
                }
            }
        }
        return false;
    }

    public override string ToString()
    {
        StringBuilder sb = new();

        sb.Append($"    ");
        for (char letraCol = 'A'; letraCol < 'A' + Columnas; letraCol++)
        {
            sb.Append($" {letraCol} ");
        }
        sb.AppendLine();

        for (int i = 0; i < Filas; i++)
        {
            sb.Append($" {i + 1,-2} ");
            for (int j = 0; j < Columnas; j++)
            {
                Casilla c = Casillas[i, j];
                sb.Append($" {c} ");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public void MarcarCasilla(int fila, int columna)
    {
        if (fila < 0 || fila >= Filas) throw new ArgumentOutOfRangeException(paramName: nameof(fila));
        if (columna < 0 || columna >= Columnas) throw new ArgumentOutOfRangeException(paramName: nameof(columna));

        if (EsPosicionValida(fila, columna) && !Casillas[fila, columna].Revelada)
            Casillas[fila, columna].Marcada = !Casillas[fila, columna].Marcada;
    }
}
