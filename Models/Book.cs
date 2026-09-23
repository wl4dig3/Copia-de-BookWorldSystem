namespace BookWorldSystem.Models
{
    // Representa un libro dentro del catálogo del sistema.
    // Contiene los datos bibliográficos del título y su estado actual de disponibilidad.
    public class Book
    {
        // Código ISBN único que identifica al libro.
        public string Isbn { get; set; }

        // Título del libro.
        public string Title { get; set; }

        // Autor o autores del libro.
        public string Author { get; set; }

        // Año de publicación.
        public int Year { get; set; }

        // Género o categoría del libro.
        public string Genre { get; set; }

        // Indica si el libro puede ser prestado en este momento.
        public bool IsAvailable { get; set; }

        // Crea un libro con su información principal y lo deja disponible por defecto.
        public Book(string isbn, string title, string author, int year, string genre)
        {
            Isbn = isbn;
            Title = title;
            Author = author;
            Year = year;
            Genre = genre;
            IsAvailable = true;
        }
    }
}
