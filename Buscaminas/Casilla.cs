namespace Buscaminas;

public class Casilla
{
    public bool TieneMina { get; set; } = false;
    public bool Revelada { get; set; } = false;
    public bool Marcada { get; set; } = false;
    public int MinasAdyacentes { get; set; } = 0;
    private readonly Tablero _tablero;
    private readonly int _fila, _col;

    public override string ToString()
    {
        if (Marcada) return "X";
        if (!Revelada) return "?";
        if (TieneMina) return "*";
        if (MinasAdyacentes > 0) return $"{MinasAdyacentes}";
        return " ";
    }

    public bool EsVacia => !TieneMina && MinasAdyacentes==0;

    internal Casilla(Tablero tablero, int fila, int col)
    {
        _tablero = tablero;
        _fila = fila;
        _col = col;
    }
}
