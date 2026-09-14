using PracticaAgenda.Logica.Repositories;
using Serilog;
using PracticaAgenda.Logica.Models;
Console.WriteLine("Hello, World!");

UserRepository userRepository = new UserRepository();
userRepository.NombreTqblq();


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // mostrar logs por consola
    .WriteTo.File(Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName, "logs", "app.log")) // guardar logs en un archivo
    .CreateLogger();// crear logger
/*
 * userRepository.Create(new User
{
    Alias = "canterano",
    Name = "paco",
    Phone = "600000001",
    Email = "pacoo@example.com"
});
 */



Log.CloseAndFlush();
