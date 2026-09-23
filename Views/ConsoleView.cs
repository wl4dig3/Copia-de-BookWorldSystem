using System;
using BookWorldSystem.Controllers;

namespace BookWorldSystem.Views
{
    public class ConsoleView
    {
        private readonly LoanController _controller;

        public ConsoleView(LoanController controller)
        {
            _controller = controller;
        }

        public void ShowMenu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("==================================================");
                Console.WriteLine("    SISTEMA DE GESTIÓN BIBLIOTECARIA BOOKWORLD   ");
                Console.WriteLine("==================================================");
                Console.WriteLine("--- GESTIÓN DE LIBROS ---");
                Console.WriteLine("1. Ver Catálogo de Libros");
                Console.WriteLine("2. Registrar Nuevo Libro");
                Console.WriteLine("3. Modificar Libro Existente");
                Console.WriteLine("4. Eliminar Libro");
                Console.WriteLine("\n--- GESTIÓN DE USUARIOS ---");
                Console.WriteLine("5. Ver Listado de Usuarios");
                Console.WriteLine("6. Registrar Nuevo Usuario");
                Console.WriteLine("7. Modificar Usuario Existente");
                Console.WriteLine("8. Eliminar Usuario");
                Console.WriteLine("\n--- PRÉSTAMOS Y DEVOLUCIONES ---");
                Console.WriteLine("9. Registrar Préstamo de Libro");
                Console.WriteLine("10. Registrar Devolución de Libro");
                Console.WriteLine("11. Reporte de Usuarios con Préstamos Activos");
                Console.WriteLine("12. Salir");
                Console.Write("\nSeleccione una opción (1-12): ");

                try
                {
                    string choice = Console.ReadLine() ?? "";

                    if (string.IsNullOrWhiteSpace(choice))
                    {
                        throw new ArgumentException("No ingresó ninguna opción. Debe escribir un número del 1 al 12.");
                    }

                    Console.WriteLine();

                    switch (choice.Trim())
                    {
                        case "1": ShowCatalog(); break;
                        case "2": ProcessAddBook(); break;
                        case "3": ProcessUpdateBook(); break;
                        case "4": ProcessDeleteBook(); break;
                        case "5": ShowUsers(); break;
                        case "6": ProcessAddUser(); break;
                        case "7": ProcessUpdateUser(); break;
                        case "8": ProcessDeleteUser(); break;
                        case "9": ProcessLoan(); break;
                        case "10": ProcessReturn(); break;
                        case "11": ShowActiveLoansReport(); break;
                        case "12":
                            running = false;
                            Console.WriteLine("Gracias por utilizar el sistema BookWorld.");
                            break;
                        default:
                            throw new FormatException($"La opción '{choice}' no es válida. Debe ingresar un número entre 1 y 12.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[ERROR PREVENIDO]: {ex.Message}");
                }

                if (running)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        // --- MÉTODOS DE LIBROS ---
        private void ShowCatalog()
        {
            Console.WriteLine("--- CATÁLOGO DE LIBROS ---");
            foreach (var b in _controller.Books)
            {
                string status = b.IsAvailable ? "DISPONIBLE" : "PRESTADO";
                Console.WriteLine($"ISBN: {b.Isbn} | Título: {b.Title} | Autor: {b.Author} | Año: {b.Year} | Género: {b.Genre} | Estado: {status}");
            }
        }

        private void ProcessAddBook()
        {
            Console.WriteLine("--- REGISTRAR NUEVO LIBRO ---");
            Console.Write("Ingrese ISBN: "); string isbn = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(isbn)) throw new ArgumentException("El ISBN no puede estar vacío.");

            Console.Write("Ingrese Título: "); string title = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("El Título no puede estar vacío.");

            Console.Write("Ingrese Autor: "); string author = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("El Autor no puede estar vacío.");

            Console.Write("Ingrese Año de publicación: ");
            if (!int.TryParse(Console.ReadLine(), out int year)) throw new FormatException("El año debe ser un número entero válido.");

            Console.Write("Ingrese Género: "); string genre = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(genre)) throw new ArgumentException("El Género no puede estar vacío.");

            Console.WriteLine($"\n{_controller.AddBook(isbn.Trim(), title.Trim(), author.Trim(), year, genre.Trim())}");
        }

        private void ProcessUpdateBook()
        {
            Console.WriteLine("--- MODIFICAR LIBRO EXISTENTE ---");
            Console.Write("Ingrese el ISBN del libro a modificar: "); string isbn = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(isbn)) throw new ArgumentException("El ISBN no puede estar vacío.");

            Console.Write("Ingrese Nuevo Título: "); string title = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("El Título no puede estar vacío.");

            Console.Write("Ingrese Nuevo Autor: "); string author = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("El Autor no puede estar vacío.");

            Console.Write("Ingrese Nuevo Año de publicación: ");
            if (!int.TryParse(Console.ReadLine(), out int year)) throw new FormatException("El año debe ser un número entero válido.");

            Console.Write("Ingrese Nuevo Género: "); string genre = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(genre)) throw new ArgumentException("El Género no puede estar vacío.");

            Console.WriteLine($"\n{_controller.UpdateBook(isbn.Trim(), title.Trim(), author.Trim(), year, genre.Trim())}");
        }

        private void ProcessDeleteBook()
        {
            Console.WriteLine("--- ELIMINAR LIBRO ---");
            Console.Write("Ingrese el ISBN del libro a eliminar: "); string isbn = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(isbn)) throw new ArgumentException("El ISBN no puede estar vacío.");

            Console.WriteLine($"\n{_controller.DeleteBook(isbn.Trim())}");
        }

        // --- MÉTODOS DE USUARIOS ---
        private void ShowUsers()
        {
            Console.WriteLine("--- LISTADO DE USUARIOS REGISTRADOS ---");
            foreach (var u in _controller.Users)
            {
                Console.WriteLine($"RUT/ID: {u.Id} | Nombre: {u.Name} | Correo: {u.Email} | Teléfono: {u.Phone} | Libros Prestados: {u.ActiveLoans.Count}/3");
            }
        }

        private void ProcessAddUser()
        {
            Console.WriteLine("--- REGISTRAR NUEVO USUARIO ---");
            Console.Write("Ingrese RUT/ID: "); string id = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("El RUT/ID no puede estar vacío.");

            Console.Write("Ingrese Nombre Completo: "); string name = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El Nombre no puede estar vacío.");

            Console.Write("Ingrese Correo Electrónico: "); string email = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("El Correo no puede estar vacío.");

            Console.Write("Ingrese Teléfono: "); string phone = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("El Teléfono no puede estar vacío.");

            Console.WriteLine($"\n{_controller.AddUser(id.Trim(), name.Trim(), email.Trim(), phone.Trim())}");
        }

        private void ProcessUpdateUser()
        {
            Console.WriteLine("--- MODIFICAR USUARIO EXISTENTE ---");
            Console.Write("Ingrese RUT/ID del usuario a modificar: "); string id = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("El RUT/ID no puede estar vacío.");

            Console.Write("Ingrese Nuevo Nombre Completo: "); string name = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El Nombre no puede estar vacío.");

            Console.Write("Ingrese Nuevo Correo Electrónico: "); string email = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("El Correo no puede estar vacío.");

            Console.Write("Ingrese Nuevo Teléfono: "); string phone = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(phone)) throw new ArgumentException("El Teléfono no puede estar vacío.");

            Console.WriteLine($"\n{_controller.UpdateUser(id.Trim(), name.Trim(), email.Trim(), phone.Trim())}");
        }

        private void ProcessDeleteUser()
        {
            Console.WriteLine("--- ELIMINAR USUARIO ---");
            Console.Write("Ingrese RUT/ID del usuario a eliminar: "); string id = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("El RUT/ID no puede estar vacío.");

            Console.WriteLine($"\n{_controller.DeleteUser(id.Trim())}");
        }

        // --- PRÉSTAMOS Y REPORTES ---
        private void ProcessLoan()
        {
            Console.WriteLine("--- REGISTRAR PRÉSTAMO ---");
            Console.Write("Ingrese RUT/ID del usuario: "); string userId = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("El RUT/ID del usuario no puede quedar vacío.");

            Console.Write("Ingrese ISBN del libro: "); string isbn = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(isbn)) throw new ArgumentException("El ISBN del libro no puede quedar vacío.");

            Console.WriteLine($"\n{_controller.RegisterLoan(userId.Trim(), isbn.Trim())}");
        }

        private void ProcessReturn()
        {
            Console.WriteLine("--- DEVOLUCIÓN DE LIBRO ---");
            Console.Write("Ingrese ISBN del libro a devolver: "); string isbn = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(isbn)) throw new ArgumentException("El ISBN del libro no puede quedar vacío.");

            Console.WriteLine($"\n{_controller.ReturnBook(isbn.Trim())}");
        }

        private void ShowActiveLoansReport()
        {
            Console.WriteLine("--- REPORTE DE USUARIOS CON LIBROS PRESTADOS ---");
            bool hasLoans = false;

            foreach (var u in _controller.Users)
            {
                if (u.ActiveLoans.Count > 0)
                {
                    hasLoans = true;
                    Console.WriteLine($"\nUsuario: {u.Name} (ID: {u.Id} | Correo: {u.Email}) - Libros prestados ({u.ActiveLoans.Count}/3):");
                    foreach (var l in u.ActiveLoans)
                    {
                        Console.WriteLine($"  - Libro: {l.BorrowedBook.Title} | Devolución Esperada: {l.ExpectedReturnDate:dd/MM/yyyy}");
                    }
                }
            }

            if (!hasLoans)
            {
                Console.WriteLine("Actualmente no existen usuarios con préstamos activos.");
            }
        }
    }
}
