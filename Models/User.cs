using System.Collections.Generic;

namespace BookWorldSystem.Models
{
    // Representa a un usuario del sistema de préstamos de libros.
    // Guarda la información básica del usuario y el estado de sus préstamos activos.
    public class User
    {
        // Identificador único del usuario dentro del sistema.
        public string Id { get; set; }

        // Nombre completo del usuario.
        public string Name { get; set; }

        // Correo electrónico de contacto del usuario.
        public string Email { get; set; }

        // Número telefónico del usuario.
        public string Phone { get; set; }

        // Lista de préstamos actuales que el usuario tiene vigentes.
        public List<Loan> ActiveLoans { get; set; }

        // Crea un nuevo usuario con los datos básicos y una lista vacía de préstamos activos.
        public User(string id, string name, string email, string phone)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
            ActiveLoans = new List<Loan>();
        }

        // Determina si el usuario puede solicitar otro préstamo.
        // En este caso, solo permite hasta 3 préstamos activos.
        public bool CanBorrow()
        {
            return ActiveLoans.Count < 3;
        }
    }
}
