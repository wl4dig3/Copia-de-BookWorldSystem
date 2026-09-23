using BookWorldSystem.Controllers;
using BookWorldSystem.Views;

namespace BookWorldSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            LoanController controller = new LoanController();
            ConsoleView view = new ConsoleView(controller);
            view.ShowMenu();
        }
    }
}
