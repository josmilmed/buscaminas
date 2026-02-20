using System;

namespace MenusConsola;

/// <summary>
/// Representa una opción dentro de un menú interactivo de consola.
/// Cada opción consta de un texto descriptivo y una acción que se ejecuta al seleccionarla.
/// </summary>
class OpcionMenu
{
    /// <summary>
    /// Obtiene el texto que describe la opción del menú.
    /// Este texto se muestra al usuario cuando el menú es renderizado en la consola.
    /// </summary>
    public string Texto { get; init; }

    /// <summary>
    /// Obtiene la acción asociada a esta opción del menú.
    /// La acción es un delegado <see cref="Action"/> que se ejecuta cuando el usuario selecciona esta opción.
    /// </summary>
    public Action Accion { get; init; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="OpcionMenu"/> con el texto y la acción especificados.
    /// </summary>
    /// <param name="texto">El texto que describe la opción del menú. No debe ser nulo ni vacío.</param>
    /// <param name="accion">La acción que se ejecutará al seleccionar esta opción. No debe ser nula.</param>
    /// <exception cref="ArgumentNullException">Se lanza si <paramref name="texto"/> o <paramref name="accion"/> son nulos.</exception>
    /// <exception cref="ArgumentException">Se lanza si <paramref name="texto"/> es una cadena vacía o solo contiene espacios en blanco.</exception>
    public OpcionMenu(string texto, Action accion)
    {
        if (string.IsNullOrWhiteSpace(texto))
            throw new ArgumentException("El texto de la opción del menú no puede estar vacío o contener solo espacios en blanco.", nameof(texto));
        
        Accion = accion ?? throw new ArgumentNullException(nameof(accion), "La acción de la opción del menú no puede ser nula.");
        Texto = texto;
    }
}
