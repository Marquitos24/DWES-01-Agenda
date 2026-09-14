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
        // Validador de datos que recibimos
        ValidarUsuario(dtoRequest);
        
        User? usuarioExistente = userRepository.FindByAlias(dtoRequest.Alias);
        
        if (usuarioExistente != null)
        {
            throw new Exception($"El alias '{dtoRequest.Alias}' ya se esta usando, elija otro.");
        }
        User user = UserMapper.toEntity(dtoRequest);
        User guardado = userRepository.Create(user);
        
        Console.WriteLine($"Usuario {guardado.Name} creado");

        // User -> DTOResponse
        return UserMapper.toDTOResponse(guardado);
    }
    
    public UserDTOResponse FindById(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("El Id debe ser mayor que 0.");
        }
        User? user = userRepository.FindById(id);
        
        if (user == null)
        {
            throw new Exception($"El usuario con id {id} no existe");
        }
        Console.WriteLine($"Id de usuario: {user.Id} \n" +
                          $"ALias: {user.Alias} \n" +
                          $"Nombre: {user.Name} \n" +
                          $"Número de telefono: {user.Phone} \n" +
                          $"Email: {user.Email}");
        
        // User -> DTOResponse
        return UserMapper.toDTOResponse(user);
    }

    public UserDTOResponse? FindByAlias(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            throw new ArgumentException("El alias no puede estar vacío.");
        }
        
        User? user = userRepository.FindByAlias(alias);
        
        if (user == null)
        {
            return null;
        }
        
        Console.WriteLine($"Id de usuario: {user.Id} " +
                          $"ALias: {user.Alias} " +
                          $"Nombre: {user.Name} " +
                          $"Número de telefono: {user.Phone} " +
                          $"Email: {user.Email}");
        
        return UserMapper.toDTOResponse(user);
    }

    public List<UserDTOResponse> FindByPagination(int page)
    {
        if (page <= 0)
        {
            throw new ArgumentException("La pagina debe ser mator que 0");
        }
        
        List<User> users = userRepository.FindByPagination(page);
        Console.WriteLine($"En la pagina {page} se encontro a {users.Count} usuarios:");

        List<UserDTOResponse> response = new List<UserDTOResponse>();
        
        foreach (User user in users)
        {
            Console.WriteLine($"ALias: {user.Alias} \n" +
                              $"Nombre: {user.Name} \n" +
                              $"Número de telefono: {user.Phone} \n" +
                              $"Email: {user.Email}");
            response.Add(UserMapper.toDTOResponse(user));
        }
        
        return response;
    }

    public UserDTOResponse Update(UserDTORequest dtoRequest)
    {
        // Validador de datos que recibimos
        ValidarUsuario(dtoRequest);
        
        if (dtoRequest.Id <= 0)
        {
            throw new ArgumentException("El Id debe ser mayor que 0");
        }
        if (userRepository.FindById(dtoRequest.Id) == null)
        {
            throw new Exception($"El usuario con Id {dtoRequest.Id} no existe");
        }

        User? usuarioConAlias = userRepository.FindByAlias(dtoRequest.Alias);
        if(usuarioConAlias != null && usuarioConAlias.Id != dtoRequest.Id)
        {
            throw new Exception($"El Alias {dtoRequest.Alias} ya existe, porfavor cambielo.");
        }
        
        User user = UserMapper.toEntity(dtoRequest);
        User actualizado = userRepository.Update(user);
        
        Console.WriteLine($"Usuario {dtoRequest.Name} actualizado");

        return UserMapper.toDTOResponse(actualizado);
    }
    
    public UserDTOResponse Delete(int id)
    {
        if (id <= 0)
        {
           throw new Exception("Debes introducir un id mator de 0");
        }
        
        User user = userRepository.FindById(id);

        if (user == null)
        {
            throw new Exception($"El usuario con id {id} no existe");
        }
        
        userRepository.Delete(id);
        Console.WriteLine($"Usuario con Id {id} eliminado");
        
        return UserMapper.toDTOResponse(user);
    }

    private void ValidarUsuario(UserDTORequest dtoRequest)
    {
        if (dtoRequest == null)
        {
            throw new ArgumentNullException("dtoRequest");
        }
        
        if (string.IsNullOrWhiteSpace(dtoRequest.Alias))
        {
            throw new ArgumentException("El alias es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(dtoRequest.Name) || string.IsNullOrWhiteSpace(dtoRequest.Phone) || string.IsNullOrWhiteSpace(dtoRequest.Email))
        {
            throw new Exception($"El nombre, telefono y correo electronico no pueden quedar vacios");
        }

        if (dtoRequest.Name.Length > 80)
        {
            throw new Exception($"El nombre no puede superar los 80 caracteres");
        }
        if (dtoRequest.Alias.Length > 40)
        {
            throw new Exception($"El alias no puede superar los 40 caracteres");
        }
        if (dtoRequest.Phone.Length > 15)
        {
            throw new Exception($"El telefono no puede superar los 15 caracteres");
        }
        if (dtoRequest.Email.Length > 50)
        {
            throw new Exception($"El Email no puede superar los 50 caracteres");
        }
        
    }
    
    
}