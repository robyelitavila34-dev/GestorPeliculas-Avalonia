
using Avalonia.Controls;
using Avalonia.Interactivity;
using GestorPeliculas.Datos;
using GestorPeliculas.Modelos;
using System;
using System.Collections.Generic;

namespace GestorPeliculas;

public partial class MainWindow : Window
{
    private readonly PeliculaDAO dao;
    private List<Pelicula> peliculas = new();
    private int idSeleccionado = 0;

    public MainWindow()
    {
        InitializeComponent();

        dao = new PeliculaDAO();
        CargarPeliculas();
    }

    // Mostrar películas guardadas
    private void CargarPeliculas()
    {
        peliculas = dao.Consultar();

        ListaPeliculas.ItemsSource = null;

        ListaPeliculas.ItemsSource =
            peliculas.ConvertAll(p => p.MostrarInformacion());
    }

    // Guardar una película
    private void BtnGuardar_Click(
        object? sender, RoutedEventArgs e)
    {
        try
        {
            Pelicula pelicula = ObtenerDatos();

            dao.Guardar(pelicula);

            TxtMensaje.Text = "Película guardada correctamente.";

            CargarPeliculas();
            LimpiarCampos();
        }
        catch (Exception ex)
        {
            TxtMensaje.Text = "Error: " + ex.Message;
        }
    }

    // Actualizar una película
    private void BtnActualizar_Click(
        object? sender, RoutedEventArgs e)
    {
        if (idSeleccionado == 0)
        {
            TxtMensaje.Text = "Selecciona una película.";
            return;
        }

        try
        {
            Pelicula pelicula = ObtenerDatos();
            pelicula.Id = idSeleccionado;

            dao.Actualizar(pelicula);

            TxtMensaje.Text = "Película actualizada.";

            CargarPeliculas();
            LimpiarCampos();
        }
        catch (Exception ex)
        {
            TxtMensaje.Text = "Error: " + ex.Message;
        }
    }

    // Eliminar una película
    private void BtnEliminar_Click(
        object? sender, RoutedEventArgs e)
    {
        if (idSeleccionado == 0)
        {
            TxtMensaje.Text = "Selecciona una película.";
            return;
        }

        try
        {
            dao.Eliminar(idSeleccionado);

            TxtMensaje.Text = "Película eliminada.";

            CargarPeliculas();
            LimpiarCampos();
        }
        catch (Exception ex)
        {
            TxtMensaje.Text = "Error: " + ex.Message;
        }
    }

    // Limpiar formulario
    private void BtnLimpiar_Click(
        object? sender, RoutedEventArgs e)
    {
        LimpiarCampos();
        TxtMensaje.Text = "Formulario limpio.";
    }

    // Seleccionar película de la lista
    private void ListaPeliculas_SelectionChanged(
        object? sender, SelectionChangedEventArgs e)
    {
        int indice = ListaPeliculas.SelectedIndex;

        if (indice < 0 || indice >= peliculas.Count)
            return;

        Pelicula pelicula = peliculas[indice];

        idSeleccionado = pelicula.Id;

        TxtTitulo.Text = pelicula.Titulo;
        TxtGenero.Text = pelicula.Genero;

        CmbTipo.SelectedIndex =
            pelicula.Tipo == "Película" ? 0 : 1;

        CmbEstado.SelectedIndex =
            pelicula.Estado == "Pendiente" ? 0 : 1;
    }

    // Obtener datos del formulario
    private Pelicula ObtenerDatos()
    {
        string titulo = TxtTitulo.Text?.Trim() ?? "";
        string genero = TxtGenero.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(titulo))
            throw new Exception("Debes ingresar un título.");

        if (string.IsNullOrWhiteSpace(genero))
            throw new Exception("Debes ingresar un género.");

        string tipo = CmbTipo.SelectedIndex == 0
            ? "Película" : "Anime";

        string estado = CmbEstado.SelectedIndex == 0
            ? "Pendiente" : "Vista";

        return new Pelicula(0, titulo, genero, tipo, estado);
    }

    // Limpiar los campos
    private void LimpiarCampos()
    {
        idSeleccionado = 0;

        ListaPeliculas.SelectedIndex = -1;

        TxtTitulo.Text = "";
        TxtGenero.Text = "";

        CmbTipo.SelectedIndex = 0;
        CmbEstado.SelectedIndex = 0;
    }
}
