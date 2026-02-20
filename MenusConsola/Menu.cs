using System;
using System.Numerics;

namespace MenusConsola;

/// <summary>
/// Representa un menú interactivo para la consola.
/// </summary>
public class Menu
{
    const int H_PADDING = 3;
    const int V_PADDING = 1;
    const int OPCION_TAB = 4;

    private string _titulo = "";

    /// <summary>
    /// Obtiene o establece el título del menú.
    /// </summary>
    /// <exception cref="ArgumentException">Se proporciona un valor nulo o vacío.</exception>
    public string Titulo
    {
        get => _titulo; 
        set
        {
            if (string.IsNullOrWhiteSpace(value)) 
                throw new ArgumentException("Se requiere un valor", paramName: nameof(Titulo));
            _titulo = value;
        }
    }

    /// <summary>
    /// Número de opciones en el menú.
    /// </summary>
    public int NumOpciones => _opciones.Count;

    private string _mensaje = "";

    /// <summary>
    /// Obtiene o establece el mensaje descriptivo del menú.
    /// </summary>
    /// <exception cref="ArgumentException">Se proporciona un valor nulo o vacío.</exception>
    public string Mensaje
    {
        get => _mensaje; 
        set
        {
            if (string.IsNullOrWhiteSpace(value)) 
                throw new ArgumentException("Se requiere un valor", paramName: nameof(Mensaje));
            _mensaje = value;
        }
    }

    private List<OpcionMenu> _opciones;

    private Vector2 _pos;

    /// <summary>
    /// Obtiene o establece la posición del menú en la consola.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Las coordenadas son negativas.</exception>
    public Vector2 Pos
    {
        get => _pos; 
        set
        {
            if (value.X < 0 || value.Y < 0) 
                throw new ArgumentOutOfRangeException(nameof(Pos), "No se permite coordenadas negativas");
            _pos = value;
        }
    }

    /// <summary>
    /// Obtiene o establece el color del título del menú.
    /// </summary>
    public ConsoleColor ColorTitulo { get; set; }

    /// <summary>
    /// Obtiene o establece el color de las opciones del menú.
    /// </summary>
    public ConsoleColor ColorOpciones { get; set; }

    /// <summary>
    /// Obtiene o establece el color de fondo del menú.
    /// </summary>
    public ConsoleColor ColorFondo { get; set; }

    /// <summary>
    /// Crea una nueva instancia de la clase <see cref="Menu"/>.
    /// </summary>
    /// <param name="titulo">Título del menú.</param>
    /// <param name="mensaje">Mensaje descriptivo.</param>
    /// <param name="pos">Posición inicial del menú (opcional).</param>
    /// <param name="colorTitulo">Color del título (opcional).</param>
    /// <param name="colorOpciones">Color de las opciones (opcional).</param>
    /// <param name="colorFondo">Color de fondo (opcional).</param>
    public Menu(
        string titulo,
        string mensaje,
        Vector2? pos = null,
        ConsoleColor colorTitulo = ConsoleColor.White,
        ConsoleColor colorOpciones = ConsoleColor.White,
        ConsoleColor colorFondo = ConsoleColor.Black
    )
    {
        Titulo = titulo;
        Mensaje = mensaje;
        Pos = pos ?? new Vector2(0, 0);
        ColorTitulo = colorTitulo;
        ColorOpciones = colorOpciones;
        ColorFondo = colorFondo;
        _opciones = [];
    }

    /// <summary>
    /// Agrega una nueva opción al menú.
    /// </summary>
    /// <param name="texto">Texto que describe la opción.</param>
    /// <param name="accion">Acción que se ejecutará cuando la opción sea seleccionada.</param>
    /// <returns>El índice de la opción agregada.</returns>
    public int IncorporarOpcion(string texto, Action accion)
    {
        _opciones.Add(new OpcionMenu(texto, accion));
        return _opciones.Count - 1;
    }

    /// <summary>
    /// Muestra el menú y permite al usuario seleccionar una opción.
    /// </summary>
    /// <returns>El índice de la opción seleccionada.</returns>
    public int MostrarMenu()
    {
        ConsoleKeyInfo k;
        int iOpcion = 0;
        do
        {
            Dibujar(iOpcion);
            k = Console.ReadKey();
            switch (k.Key)
            {
                case ConsoleKey.UpArrow:
                    if (iOpcion > 0) iOpcion--;
                    break;

                case ConsoleKey.DownArrow:
                    if (iOpcion < _opciones.Count - 1) iOpcion++;
                    break;
            }
        } while (k.Key != ConsoleKey.Enter);

        return iOpcion;
    }

    /// <summary>
    /// Ejecuta la acción asociada a una opción específica.
    /// </summary>
    /// <param name="iOpcion">Índice de la opción a ejecutar.</param>
    /// <exception cref="ArgumentOutOfRangeException">Se lanza si el índice está fuera de rango.</exception>
    public void EjecutarOpcion(int iOpcion)
    {
        if (iOpcion < 0 || iOpcion >= _opciones.Count) 
            throw new ArgumentOutOfRangeException(paramName: nameof(iOpcion));
        _opciones[iOpcion].Accion();
    }

    /// <summary>
    /// Dibuja el menú en la consola.
    /// </summary>
    /// <param name="iOpcion">Índice de la opción actualmente seleccionada (opcional).</param>
    public void Dibujar(int? iOpcion = null)
    {
        const string SELECCIONA_OPCION = "Selecciona una opción:";
        const string AYUDA = "Usa los cursores para navegar por el menú.";

        int alto = _opciones.Count + 4 + V_PADDING * 2 + 1;
        int ancho = Math.Max(Titulo.Length, Mensaje.Length);
        ancho = Math.Max(ancho, SELECCIONA_OPCION.Length);
        ancho = Math.Max(ancho, AYUDA.Length);
        ancho += H_PADDING * 2;

        foreach (OpcionMenu op in _opciones)
        {
            if (op.Texto.Length + OPCION_TAB > ancho) ancho = op.Texto.Length + OPCION_TAB;
        }

        Console.Clear();

        Marco m = new((int)Pos.X, (int)Pos.Y, ancho + 4, alto + 2);

        m.Dibujar();

        double xCentro = m.X1 + (double)m.Ancho / 2;

        int x = (int)(xCentro - (double)$" {Titulo} ".Length / 2);
        int y = m.Y1;

        Console.SetCursorPosition(x, y);
        Console.Write($" {Titulo} ");
        y += 1;

        x = (int)(xCentro - (double)Mensaje.Length / 2);
        Console.SetCursorPosition(x, y);
        Console.Write(Mensaje);
        y += 1 + V_PADDING;

        x = m.X1 + 2 + H_PADDING;
        Console.SetCursorPosition(x, y);
        Console.Write(SELECCIONA_OPCION);
        y += 2;

        for (int i = 0; i < _opciones.Count; i++)
        {
            OpcionMenu op = _opciones[i];
            Console.SetCursorPosition(x + OPCION_TAB, y);
            Console.Write($"{i + 1}. {op.Texto}.");
            Console.SetCursorPosition(x + OPCION_TAB, y);

            if (iOpcion == i)
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write($"{i + 1}.");
                Console.ResetColor();
            }
            y += 1;
        }
        y += 1;

        Console.SetCursorPosition(x, y);
        Console.Write(AYUDA);

        Console.SetCursorPosition(0, m.Y1 + m.Alto + 1);
    }
}
