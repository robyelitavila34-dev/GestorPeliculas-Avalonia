
using Microsoft.Data.Sqlite;
using GestorPeliculas.Modelos;
using System;
using System.Collections.Generic;
using System.IO;

namespace GestorPeliculas.Datos
{
    public class PeliculaDAO
    {
        private readonly string conexion;

        public PeliculaDAO()
        {
            string ruta = Path.Combine(
                AppContext.BaseDirectory,
                "peliculas.db");

            conexion = $"Data Source={ruta}";
            CrearTabla();
        }

        private void CrearTabla()
        {
            using var con = new SqliteConnection(conexion);
            con.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS Peliculas (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Titulo TEXT NOT NULL,
                    Genero TEXT NOT NULL,
                    Tipo TEXT NOT NULL,
                    Estado TEXT NOT NULL
                )";

            using var cmd = new SqliteCommand(sql, con);
            cmd.ExecuteNonQuery();
        }

        // CREATE: Guardar película
        public void Guardar(Pelicula pelicula)
        {
            using var con = new SqliteConnection(conexion);
            con.Open();

            string sql = @"INSERT INTO Peliculas
                (Titulo, Genero, Tipo, Estado)
                VALUES (@titulo, @genero, @tipo, @estado)";

            using var cmd = new SqliteCommand(sql, con);

            cmd.Parameters.AddWithValue("@titulo", pelicula.Titulo);
            cmd.Parameters.AddWithValue("@genero", pelicula.Genero);
            cmd.Parameters.AddWithValue("@tipo", pelicula.Tipo);
            cmd.Parameters.AddWithValue("@estado", pelicula.Estado);

            cmd.ExecuteNonQuery();
        }

        // READ: Consultar películas
        public List<Pelicula> Consultar()
        {
            var peliculas = new List<Pelicula>();

            using var con = new SqliteConnection(conexion);
            con.Open();

            string sql = "SELECT * FROM Peliculas ORDER BY Id";

            using var cmd = new SqliteCommand(sql, con);
            using var lector = cmd.ExecuteReader();

            while (lector.Read())
            {
                peliculas.Add(new Pelicula(
                    lector.GetInt32(0),
                    lector.GetString(1),
                    lector.GetString(2),
                    lector.GetString(3),
                    lector.GetString(4)
                ));
            }

            return peliculas;
        }

        // UPDATE: Actualizar película
        public void Actualizar(Pelicula pelicula)
        {
            using var con = new SqliteConnection(conexion);
            con.Open();

            string sql = @"UPDATE Peliculas SET
                Titulo = @titulo,
                Genero = @genero,
                Tipo = @tipo,
                Estado = @estado
                WHERE Id = @id";

            using var cmd = new SqliteCommand(sql, con);

            cmd.Parameters.AddWithValue("@id", pelicula.Id);
            cmd.Parameters.AddWithValue("@titulo", pelicula.Titulo);
            cmd.Parameters.AddWithValue("@genero", pelicula.Genero);
            cmd.Parameters.AddWithValue("@tipo", pelicula.Tipo);
            cmd.Parameters.AddWithValue("@estado", pelicula.Estado);

            cmd.ExecuteNonQuery();
        }

        // DELETE: Eliminar película
        public void Eliminar(int id)
        {
            using var con = new SqliteConnection(conexion);
            con.Open();

            string sql = "DELETE FROM Peliculas WHERE Id = @id";

            using var cmd = new SqliteCommand(sql, con);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
