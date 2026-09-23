using System;
using System.Collections.Generic;
using System.Linq;
using BookWorldSystem.Models;

namespace BookWorldSystem.Controllers
{
    // Controlador principal del sistema de préstamos.
    // Aquí se gestionan los libros, usuarios y préstamos activos, así como las reglas de negocio.
    public class LoanController
    {
        // Catálogo de libros disponibles en el sistema.
        public List<Book> Books { get; private set; }

        // Registro de usuarios del sistema.
        public List<User> Users { get; private set; }

        // Historial de préstamos registrados.
        public List<Loan> Loans { get; private set; }

        // Inicializa las colecciones y carga datos de prueba para empezar con un estado funcional.
        public LoanController()
        {
            Books = new List<Book>();
            Users = new List<User>();
            Loans = new List<Loan>();
            SeedData();
        }

        // Carga datos iniciales para que la aplicación cuente con ejemplos de libros y usuarios.
        private void SeedData()
        {
            Books.Add(new Book("978-1", "C# y .NET Core", "Microsoft Press", 2022, "Tecnología"));
            Books.Add(new Book("978-2", "Patrones de Diseño", "Erich Gamma", 1994, "Ingeniería de Software"));
            Books.Add(new Book("978-3", "Clean Code", "Robert C. Martin", 2008, "Programación"));

            Users.Add(new User("11111111-1", "Juan Pérez", "juan@email.com", "+56911112222"));
            Users.Add(new User("22222222-2", "María González", "maria@email.com", "+56933334444"));
        }

        // --- GESTIÓN DE LIBROS ---
        // Agrega un nuevo libro al catálogo si no existe un ISBN duplicado.
        public string AddBook(string isbn, string title, string author, int year, string genre)
        {
            if (Books.Any(b => b.Isbn == isbn))
                throw new InvalidOperationException($"Ya existe un libro registrado con el ISBN {isbn}.");

            Book newBook = new Book(isbn, title, author, year, genre);
            Books.Add(newBook);
            return $"Libro '{title}' ingresado exitosamente al catálogo.";
        }

        // Actualiza los datos de un libro existente.
        public string UpdateBook(string isbn, string newTitle, string newAuthor, int newYear, string newGenre)
        {
            var book = Books.FirstOrDefault(b => b.Isbn == isbn);
            if (book == null)
                throw new Exception("El libro con el ISBN especificado no existe.");

            book.Title = newTitle;
            book.Author = newAuthor;
            book.Year = newYear;
            book.Genre = newGenre;

            return $"Libro con ISBN {isbn} modificado exitosamente.";
        }

        // Elimina un libro solo si no está prestado actualmente.
        public string DeleteBook(string isbn)
        {
            var book = Books.FirstOrDefault(b => b.Isbn == isbn);
            if (book == null)
                throw new Exception("El libro con el ISBN especificado no existe.");

            if (!book.IsAvailable)
                throw new InvalidOperationException("No se puede eliminar el libro porque se encuentra actualmente prestado.");

            Books.Remove(book);
            return $"Libro '{book.Title}' eliminado del catálogo exitosamente.";
        }

        // --- GESTIÓN DE USUARIOS ---
        // Registra un nuevo usuario en el sistema si aún no existe.
        public string AddUser(string id, string name, string email, string phone)
        {
            if (Users.Any(u => u.Id == id))
                throw new InvalidOperationException($"Ya existe un usuario registrado con el RUT/ID {id}.");

            User newUser = new User(id, name, email, phone);
            Users.Add(newUser);
            return $"Usuario '{name}' registrado exitosamente.";
        }

        // Modifica la información personal de un usuario registrado.
        public string UpdateUser(string id, string newName, string newEmail, string newPhone)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                throw new Exception("El usuario ingresado no existe.");

            user.Name = newName;
            user.Email = newEmail;
            user.Phone = newPhone;

            return $"Usuario con RUT/ID {id} modificado exitosamente.";
        }

        // Elimina un usuario solo cuando no tiene libros prestados activos.
        public string DeleteUser(string id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                throw new Exception("El usuario ingresado no existe.");

            if (user.ActiveLoans.Count > 0)
                throw new InvalidOperationException("No se puede eliminar el usuario porque tiene préstamos de libros activos.");

            Users.Remove(user);
            return $"Usuario '{user.Name}' eliminado exitosamente.";
        }

        // --- PRÉSTAMOS Y DEVOLUCIONES ---
        // Registra un préstamo si el usuario puede pedirlo y el libro está disponible.
        public string RegisterLoan(string userId, string isbn)
        {
            var user = Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new Exception("El usuario ingresado no existe.");

            if (!user.CanBorrow())
                throw new InvalidOperationException($"El usuario {user.Name} ya posee el límite máximo de 3 libros prestados.");

            var book = Books.FirstOrDefault(b => b.Isbn == isbn);
            if (book == null)
                throw new Exception("El libro con el ISBN especificado no existe.");

            if (!book.IsAvailable)
                throw new InvalidOperationException($"El libro '{book.Title}' no se encuentra disponible actualmente.");

            book.IsAvailable = false;
            string loanId = "L-" + (Loans.Count + 1).ToString("D3");
            Loan newLoan = new Loan(loanId, user, book);

            Loans.Add(newLoan);
            user.ActiveLoans.Add(newLoan);

            return $"Préstamo {loanId} registrado exitosamente a {user.Name}. Devolución esperada: {newLoan.ExpectedReturnDate:dd/MM/yyyy}.";
        }

        // Marca un libro como devuelto y elimina la relación activa del usuario con ese préstamo.
        public string ReturnBook(string isbn)
        {
            var loan = Loans.FirstOrDefault(l => l.BorrowedBook.Isbn == isbn && !l.BorrowedBook.IsAvailable);
            if (loan == null)
                throw new Exception("No se encontró un préstamo activo para el libro especificado.");

            loan.BorrowedBook.IsAvailable = true;
            loan.Borrower.ActiveLoans.Remove(loan);

            return $"El libro '{loan.BorrowedBook.Title}' ha sido devuelto con éxito por {loan.Borrower.Name}.";
        }
    }
}