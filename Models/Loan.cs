using System;

namespace BookWorldSystem.Models
{
    // Representa un préstamo realizado por un usuario a un libro.
    // Guarda quién tomó el libro, qué libro recibió y cuándo debe devolverlo.
    public class Loan
    {
        // Identificador único del préstamo.
        public string LoanId { get; set; }

        // Usuario que solicitó el préstamo.
        public User Borrower { get; set; }

        // Libro que fue prestado.
        public Book BorrowedBook { get; set; }

        // Fecha y hora en que se registró el préstamo.
        public DateTime LoanDate { get; set; }

        // Fecha estimada de devolución, calculada a partir de la fecha de préstamo.
        public DateTime ExpectedReturnDate { get; set; }

        // Crea un préstamo con la información del usuario, el libro y la cantidad de días previstos para la devolución.
        public Loan(string loanId, User borrower, Book borrowedBook, int loanDays = 7)
        {
            LoanId = loanId;
            Borrower = borrower;
            BorrowedBook = borrowedBook;
            LoanDate = DateTime.Now;
            ExpectedReturnDate = LoanDate.AddDays(loanDays);
        }
    }
}
