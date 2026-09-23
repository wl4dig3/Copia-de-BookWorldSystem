using BookWorldSystem.Controllers;
using BookWorldSystem.Models;
using Xunit;

namespace BookWorldSystem.Tests;

public class UserTests
{
    [Fact]
    public void CanBorrow_ReturnsTrue_WhenUserHasLessThanThreeActiveLoans()
    {
        var user = new User("1", "Juan", "juan@test.com", "+56900000000");

        user.ActiveLoans.Add(new Loan("L-001", user, new Book("978-1", "Libro 1", "Autor", 2024, "Ficción")));
        user.ActiveLoans.Add(new Loan("L-002", user, new Book("978-2", "Libro 2", "Autor", 2024, "Ficción")));

        Assert.True(user.CanBorrow());
    }

    [Fact]
    public void CanBorrow_ReturnsFalse_WhenUserHasThreeActiveLoans()
    {
        var user = new User("2", "María", "maria@test.com", "+56911111111");

        user.ActiveLoans.Add(new Loan("L-001", user, new Book("978-1", "Libro 1", "Autor", 2024, "Ficción")));
        user.ActiveLoans.Add(new Loan("L-002", user, new Book("978-2", "Libro 2", "Autor", 2024, "Ficción")));
        user.ActiveLoans.Add(new Loan("L-003", user, new Book("978-3", "Libro 3", "Autor", 2024, "Ficción")));

        Assert.False(user.CanBorrow());
    }
}

public class LoanControllerTests
{
    [Fact]
    public void RegisterLoan_Throws_WhenUserHasThreeActiveLoans()
    {
        var controller = new LoanController();
        var user = new User("999", "Ana", "ana@test.com", "+56922222222");
        controller.Users.Add(user);

        for (int i = 1; i <= 3; i++)
        {
            var book = new Book($"978-{i}", $"Libro {i}", "Autor", 2024, "Ficción");
            controller.Books.Add(book);
            user.ActiveLoans.Add(new Loan($"L-{i}", user, book));
        }

        var availableBook = new Book("978-10", "Libro disponible", "Autor", 2024, "Ficción");
        controller.Books.Add(availableBook);

        var exception = Assert.Throws<InvalidOperationException>(() => controller.RegisterLoan("999", "978-10"));

        Assert.Equal("El usuario Ana ya posee el límite máximo de 3 libros prestados.", exception.Message);
    }
}
