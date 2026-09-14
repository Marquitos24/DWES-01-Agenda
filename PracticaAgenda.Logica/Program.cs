using PracticaAgenda.Logica.Repositories;
using PracticaAgenda.Logica.Models;
Console.WriteLine("Hello, World!");

UserRepository userRepository = new UserRepository();
userRepository.NombreTqblq();

userRepository.Create(new User
{
    Alias = "canterano",
    Name = "paco",
    Phone = "600000001",
    Email = "pacoo@example.com"
});

