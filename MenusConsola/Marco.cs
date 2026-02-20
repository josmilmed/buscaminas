using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

namespace MenusConsola;

/// <summary>
/// Define los tipos de trazo disponibles para el marco.
/// </summary>
public enum TipoTrazo
{
    /// <summary>
    /// Trazo simple para los bordes del marco.
    /// </summary>
    Simple,

    /// <summary>
    /// Trazo doble para los bordes del marco.
    /// </summary>
    Doble
}

/// <summary>
/// Representa un marco decorativo que se puede dibujar en la consola.
/// </summary>
public class Marco
{
    // Estilos de bordes
    static readonly char[] B1L = { '─', '│', '┌', '┐', '└', '┘' };
    static readonly char[] B2L = { '═', '║', '╔', '╗', '╚', '╝' };
    const int H = 0, V = 1, TL = 2, TR = 3, BL = 4, BR = 5;

    // Propiedades privadas
    private int _x1;
    private int _y1;

    /// <summary>
    /// Obtiene o establece la coordenada X del borde superior izquierdo del marco.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Se lanza si el valor es menor que cero.</exception>
    public int X1
    {
        get => _x1;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(X1), "El valor no puede ser menor que cero");
            _x1 = value;
        }
    }

    /// <summary>
    /// Obtiene o establece la coordenada Y del borde superior izquierdo del marco.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Se lanza si el valor es menor que cero.</exception>
    public int Y1
    {
        get => _y1;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(Y1), "El valor no puede ser menor que cero");
            _y1 = value;
        }
    }

    /// <summary>
    /// Obtiene o establece el ancho del marco.
    /// </summary>
    public int Ancho { get; set; }

    /// <summary>
    /// Obtiene o establece el alto del marco.
    /// </summary>
    public int Alto { get; set; }

    /// <summary>
    /// Obtiene o establece el tipo de trazo del marco.
    /// </summary>
    public TipoTrazo TipoTrazo { get; set; }

    /// <summary>
    /// Obtiene o establece el color de los bordes del marco.
    /// </summary>
    public ConsoleColor ColorMarco { get; set; }

    /// <summary>
    /// Obtiene o establece el color de fondo del marco.
    /// </summary>
    public ConsoleColor ColorFondo { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Marco"/>.
    /// </summary>
    /// <param name="x1">Coordenada X del borde superior izquierdo.</param>
    /// <param name="y1">Coordenada Y del borde superior izquierdo.</param>
    /// <param name="ancho">Ancho del marco.</param>
    /// <param name="alto">Alto del marco.</param>
    /// <param name="tipoTrazo">Tipo de trazo a utilizar (opcional).</param>
    /// <param name="colorMarco">Color de los bordes del marco (opcional).</param>
    /// <param name="colorFondo">Color de fondo del marco (opcional).</param>
    public Marco(
        int x1,
        int y1,
        int ancho,
        int alto,
        TipoTrazo tipoTrazo = TipoTrazo.Simple,
        ConsoleColor colorMarco = ConsoleColor.Gray,
        ConsoleColor colorFondo = ConsoleColor.Black
    )
    {
        X1 = x1;
        Y1 = y1;
        Ancho = ancho;
        Alto = alto;
        TipoTrazo = tipoTrazo;
        ColorMarco = colorMarco;
        ColorFondo = colorFondo;
    }

    /// <summary>
    /// Crea un marco centrado en la ventana de la consola.
    /// </summary>
    /// <param name="ancho">Ancho del marco.</param>
    /// <param name="alto">Alto del marco.</param>
    /// <param name="tipoTrazo">Tipo de trazo a utilizar (opcional).</param>
    /// <param name="colorMarco">Color de los bordes del marco (opcional).</param>
    /// <param name="colorFondo">Color de fondo del marco (opcional).</param>
    /// <returns>Una instancia de <see cref="Marco"/> centrada en la consola.</returns>
    public static Marco Centrado(
        int ancho,
        int alto,
        TipoTrazo tipoTrazo = TipoTrazo.Simple,
        ConsoleColor colorMarco = ConsoleColor.Gray,
        ConsoleColor colorFondo = ConsoleColor.Black
    )
    {
        int x1 = (int)((double)Console.WindowWidth / 2 - (double)ancho / 2);
        int y1 = (int)((double)Console.WindowHeight / 2 - (double)alto / 2);
        return new Marco(
            x1: x1 > 0 ? x1 : 0,
            y1: y1 > 0 ? y1 : 0,
            ancho: ancho,
            alto: alto,
            tipoTrazo: tipoTrazo,
            colorMarco: colorMarco,
            colorFondo: colorFondo
        );
    }

    /// <summary>
    /// Dibuja el marco en la consola.
    /// </summary>
    public void Dibujar()
    {
        // Control de límites de pantalla
        int limiteX = Console.WindowWidth - 1;
        int limiteY = Console.WindowHeight - 1;

        int x2 = Math.Min(X1 + Ancho - 1, limiteX);
        int y2 = Math.Min(Y1 + Alto - 1, limiteY);

        char[] borde = TipoTrazo == TipoTrazo.Simple ? B1L : B2L;

        Console.ForegroundColor = ColorMarco;
        Console.BackgroundColor = ColorFondo;

        // Borde superior
        Console.SetCursorPosition(X1, Y1);
        Console.Write(borde[TL] + new string(borde[H], x2 - X1 - 1) + borde[TR]);

        // Bordes verticales
        for (int y = Y1 + 1; y < y2; y++)
        {
            Console.SetCursorPosition(X1, y);
            Console.Write(borde[V]);
            for (int x = X1 + 1; x < x2; x++)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(" ");
            }
            Console.SetCursorPosition(x2, y);
            Console.Write(borde[V]);
        }

        // Borde inferior
        Console.SetCursorPosition(X1, y2);
        Console.Write(borde[BL] + new string(borde[H], x2 - X1 - 1) + borde[BR]);

        Console.ResetColor();
    }
}
