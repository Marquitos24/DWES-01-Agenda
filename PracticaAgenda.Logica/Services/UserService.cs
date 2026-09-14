using PracticaAgenda.Logica.Models;
using PracticaAgenda.Logica.Repositories;
using PracticaAgenda.Logica.DTOs;
using PracticaAgenda.Logica.Mapper;

namespace PracticaAgenda.Logica.Services;

public class UserService : IUserService
{
    /*
     * 1- inyectar Repositorio
     * 2- Metodo crear usuario (recibir UserDTOREquest transformado en User y pasarlo al repository para crearlo)
     * 3- Metodo buscar Id (Se recibe el Id y se piede el user que lo conviete en DTOResponse)
     * 4- Metodo bucar por Alias
     * 5- Metodo buscar por paginacion (Convertir cada User devuelto en DTOResponse)
     * 6- Metodo Actualizar (con Id de usuario)
     * 7- Eliminar usuario
     * 8- Añadir validaciones y excepciones
     */
    private UserRepository userRepository;

    public UserService(UserRepository userRepository)
    {
        this.userRepository = userRepository;
    }
    

    public UserDTOResponse Create(UserDTORequest dtoRequest)
    {
        User user = UserMapper.toEntity(dtoRequest);
        User guardado = userRepository.Create(user);
        Console.WriteLine($"Usuario {dtoRequest.Name} creado");

        return UserMapper.toDTOResponse(guardado);
    }
    
    public UserDTOResponse FindById(int id)
    {
        UserDTOResponse dtoRs = UserMapper.toDTOResponse(id);
        UserDTOResponse encontrado = userRepository.FindById(dtoRs);
        Console.WriteLine($"Id de usuario: {dtoRs.Id} " +
                          $"ALias: {dtoRs.Alias} " +
                          $"Nombre: {dtoRs.Name} " +
                          $"Número de telefono: {dtoRs.Phone} " +
                          $"Email: {dtoRs.Email}");
        
        return 
    }

    public User? FindByAlias(string alias)
    {
        return userRepository.FindByAlias(alias);
    }

    public User? FindByPagination(int page)
    {
        throw new NotImplementedException();
    }

    public User Update(User user)
    {
        
    }
    public User Delete(int id)
    {
        throw new NotImplementedException();
    }
    
    
}