using PracticaAgenda.Logica.Models;

namespace PracticaAgenda.Logica.Repositories;

public interface IUserRepository
{
    // Buscar por ID del usuario
    User? FindById(int id);
    
    // Buscar por Alias del usuario
    User? FindByAlias(string? alias);
    
    // Buscar usuarios por paginación
    List<User> FindByPagination(int page);   
    
    // Funciones principales: Crear, actualizar y borrar
    User Create(User user);
    User Update(User user);
    void Delete(int id);
    
}