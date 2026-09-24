# BookWorldSystem

Sistema de gestión bibliotecaria desarrollado en C# con .NET 8. El proyecto permite administrar libros, usuarios y préstamos, siguiendo una estructura MVC simplificada con modelos, controlador y capa de vista en consola.

## 1. Objetivo del sistema

BookWorldSystem tiene como finalidad gestionar de manera básica pero ordenada lo siguiente:

- Registro y consulta de libros.
- Registro y actualización de usuarios.
- Control de préstamos y devoluciones.
- Restricción de préstamos según el límite máximo por usuario.
- Reporte de usuarios con préstamos activos.
- Validación de reglas de negocio antes de permitir operaciones.

---

## 2. Requisitos

Para ejecutar el proyecto necesitas:

- .NET SDK 8.0 o superior.
- Sistema operativo compatible con .NET: Windows, Linux o macOS.
- Terminal o consola para ejecutar la aplicación.

Verificar versión instalada:

```bash
dotnet --version
```

---

## 3. Arquitectura del proyecto

El proyecto usa una estructura simple, inspirada en MVC:

- Models: representa las entidades y la lógica propia del dominio.
- Controllers: contiene la lógica de negocio y validaciones.
- Views: permite interactuar con el usuario desde consola.
- Tests: valida comportamientos clave del sistema.

### Estructura principal

```text
BookWorldSystem/
├── Controllers/
│   └── LoanController.cs
├── Models/
│   ├── Book.cs
│   ├── Loan.cs
│   └── User.cs
├── Views/
│   └── ConsoleView.cs
├── BookWorldSystem.Tests/
│   └── UserTests.cs
├── Program.cs
├── BookWorldSystem.csproj
├── README.md
└── .gitignore
```

---

## 4. Modelos del dominio

### 4.1 Book

Representa un libro con los siguientes atributos:

- Isbn: identificador único.
- Title: título.
- Author: autor.
- Year: año de publicación.
- Genre: género.
- IsAvailable: estado de disponibilidad.

Reglas:

- Un libro se crea disponible por defecto.
- No puede haber dos libros con el mismo ISBN.

### 4.2 User

Representa un usuario del sistema:

- Id
- Name
- Email
- Phone
- ActiveLoans

Reglas:

- Un usuario puede tener hasta 3 préstamos activos.
- La validación se realiza mediante `CanBorrow()`.

### 4.3 Loan

Representa un préstamo realizado:

- LoanId
- Borrower
- BorrowedBook
- LoanDate
- ExpectedReturnDate

Reglas:

- El préstamo se crea con una fecha actual.
- La fecha de devolución esperada se calcula en 7 días por defecto.

---

## 5. Controlador: LoanController

El controlador centraliza la lógica de negocio. Sus responsabilidades principales son:

- Inicializar el catálogo de libros y usuarios.
- Registrar, actualizar y eliminar libros.
- Registrar, actualizar y eliminar usuarios.
- Registrar préstamos.
- Registrar devoluciones.
- Generar reportes de préstamos activos.

### Métodos principales

#### Gestión de libros

- AddBook()
- UpdateBook()
- DeleteBook()

#### Gestión de usuarios

- AddUser()
- UpdateUser()
- DeleteUser()

#### Préstamos y devoluciones

- RegisterLoan()
- ReturnBook()

### Reglas de negocio implementadas

1. No se puede registrar un libro con un ISBN duplicado.
2. No se puede eliminar un libro que esté prestado.
3. No se puede registrar un usuario con el mismo Id.
4. No se puede eliminar un usuario con préstamos activos.
5. Un usuario no puede solicitar más de 3 libros prestados a la vez.
6. Un libro no puede prestarse si ya está prestado.
7. Un libro solo puede devolverce si existe un préstamo activo asociado.

---

## 6. Vista de consola

La vista está en [Views/ConsoleView.cs](Views/ConsoleView.cs).

Permite al usuario interactuar desde el menú principal:

- 1. Ver catálogo de libros
- 2. Registrar nuevo libro
- 3. Modificar libro existente
- 4. Eliminar libro
- 5. Ver listado de usuarios
- 6. Registrar nuevo usuario
- 7. Modificar usuario existente
- 8. Eliminar usuario
- 9. Registrar préstamo de libro
- 10. Registrar devolución de libro
- 11. Reporte de usuarios con préstamos activos
- 12. Salir

La consola limpia la pantalla al iniciar cada iteración y luego espera la opción del usuario con `Console.ReadLine()`.

---

## 7. Flujo de ejecución

La aplicación inicia desde [Program.cs](Program.cs):

```csharp
LoanController controller = new LoanController();
ConsoleView view = new ConsoleView(controller);
view.ShowMenu();
```

Esto crea el controlador, carga datos iniciales y levanta el menú principal de la consola.

---

## 8. Datos iniciales por defecto

Al crear `LoanController`, el sistema carga automáticamente:

- 3 libros iniciales.
- 2 usuarios iniciales.

Esto permite probar funcionalidades sin necesidad de registrar todo desde cero.

---

## 9. Cómo ejecutar la aplicación

Desde la raíz del proyecto:

```bash
dotnet restore
dotnet run
```

Ejemplo de flujo de uso:

1. Seleccionar la opción 9.
2. Ingresar un usuario válido, por ejemplo: `11111111-1`.
3. Ingresar un ISBN válido, por ejemplo: `978-1`.
4. Confirmar la operación.
5. Luego usar la opción 10 para devolver el libro.

---

## 10. Cómo ejecutar las pruebas

El proyecto incluye pruebas unitarias en [BookWorldSystem.Tests/UserTests.cs](BookWorldSystem.Tests/UserTests.cs).

Para ejecutarlas:

```bash
dotnet test
```

### Cobertura de pruebas actual

- Validación de `CanBorrow()` cuando el usuario tiene menos de 3 préstamos.
- Validación de `CanBorrow()` cuando ya tiene 3 préstamos.
- Validación de error al intentar registrar un préstamo si el usuario ya alcanzó el límite.

---

## 11. Documentación de usuario

### Menú principal

Cuando se ejecuta la aplicación, el usuario ve este menú:

```text
==================================================
    SISTEMA DE GESTIÓN BIBLIOTECARIA BOOKWORLD
==================================================
--- GESTIÓN DE LIBROS ---
1. Ver Catálogo de Libros
2. Registrar Nuevo Libro
3. Modificar Libro Existente
4. Eliminar Libro

--- GESTIÓN DE USUARIOS ---
5. Ver Listado de Usuarios
6. Registrar Nuevo Usuario
7. Modificar Usuario Existente
8. Eliminar Usuario

--- PRÉSTAMOS Y DEVOLUCIONES ---
9. Registrar Préstamo de Libro
10. Registrar Devolución de Libro
11. Reporte de Usuarios con Préstamos Activos
12. Salir
```

### Casos de uso básicos

#### Registrar préstamo

1. Elegir la opción 9.
2. Ingresar RUT/ID del usuario.
3. Ingresar ISBN del libro.
4. El sistema valida:
   - usuario existente
   - libro existente
   - usuario con capacidad disponible
   - libro disponible
5. Si todo es correcto, se registra el préstamo.

#### Devolver libro

1. Elegir la opción 10.
2. Ingresar ISBN del libro a devolver.
3. El sistema valida que exista un préstamo activo para ese libro.
4. El libro se marca como disponible y se elimina de la lista de préstamos activos del usuario.

#### Ver reportes

1. Elegir la opción 11.
2. Se listan todos los usuarios con préstamos activos.
3. Se muestra el libro prestado y la fecha esperada de devolución.

---

## 12. Validaciones y manejo de errores

El sistema maneja errores con excepciones y mensajes claros para el usuario. Algunas validaciones importantes:

- ISBN vacío.
- Título vacío.
- Autor vacío.
- Año inválido.
- Usuario inexistente.
- Libro inexistente.
- Usuario sin capacidad para préstamo.
- Libro no disponible.
- Usuario con préstamos activos al intentar eliminarlo.
- Libro prestado al intentar eliminarlo.

Los mensajes se presentan en la consola y ayudan a identificar el problema rápidamente.

---

## 13. Depuración y evidencia de debug

Para hacer seguimiento interno del flujo del programa, se pueden agregar mensajes de depuración visibles en la consola con `Console.WriteLine()`.

Ejemplo:

```csharp
Console.WriteLine($"[DEBUG] RegisterLoan solicitado -> userId={userId}, isbn={isbn}");
```

Esto permite ver claramente:

- qué método se invocó,
- qué valores recibió,
- si la validación falló o pasó,
- qué datos quedaron registrados finalmente.

### Observación importante

El proyecto usa `Console.ReadKey()` dentro de la vista, por lo que si la aplicación se ejecuta en un entorno no interactivo o con entrada redirigida, puede lanzar una excepción del tipo:

```text
Cannot read keys when either application does not have a console or when console input has been redirected.
```

Esto no indica un fallo del sistema de préstamos, sino de la interacción de consola en entornos no interactivos.

---

## 14. Limitaciones actuales

El sistema actual es un proyecto académico y presenta limitaciones funcionales, entre ellas:

- No cuenta con persistencia de datos en base de datos ni archivo.
- La información se pierde al cerrar la aplicación.
- No hay autenticación ni roles de usuario.
- La gestión es exclusivamente por consola.
- El manejo de devoluciones no registra historial con fecha real de devolución, solo la esperada.

---

## 15. Mejoras sugeridas

- Agregar persistencia con SQLite o SQL Server.
- Añadir validación de correo y formato de RUT/ID.
- Mejorar la experiencia de usuario con una interfaz web o gráfica.
- Registrar historial completo de préstamos y devoluciones.
- Implementar pruebas de integración.
- Separar aún más la capa de presentación de la lógica de negocio.

---

## 16. Resumen

BookWorldSystem es una aplicación de consola para la gestión de préstamos bibliotecarios. Su objetivo principal es un control sencillo pero ordenado de libros, usuarios y préstamos, aplicando reglas de negocio esenciales para evitar inconsistencias.

Es un proyecto ideal para:

- estudiar programación orientada a objetos,
- practicar validaciones,
- comprender una estructura MVC simple,
- desarrollar lógica de negocio en C#.

---

## 17. Comandos útiles

```bash
dotnet restore
dotnet build
dotnet run
dotnet test
```

---

## 18. Contacto y uso

Este documento sirve como referencia técnica y guía de usuario para el proyecto. Si deseas ampliar funciones o dejar la app lista para producción, se recomienda continuar con persistencia, pruebas e integración con interfaz más robusta.
