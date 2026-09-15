using Microsoft.Data.Sqlite;
using PracticaAgenda.Logica.Models;
using PracticaAgenda.Logica.Repositories;
using PracticaAgenda.Logica.DTOs;
using PracticaAgenda.Logica.Exceptions;
using PracticaAgenda.Logica.Mapper;
using Serilog;
using PracticaAgenda.Logica.Cache;

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
    private readonly UserCache userCache;

    public UserService(UserRepository userRepository, UserCache userCache)
    {
        this.userRepository = userRepository;
        this.userCache = userCache;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dtoRequest"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="Exception"></exception>
    public UserDTOResponse Create(UserDTORequest dtoRequest)
    {
        // Validador de datos que recibimos
        ValidarUsuario(dtoRequest);
        
        User? usuarioExistente = userRepository.FindByAlias(dtoRequest.Alias);
        
        if (usuarioExistente != null) // En este tipo de validaciones los logs son inecesarios
        {
            throw new InvalidOperationException($"El alias '{dtoRequest.Alias}' ya se esta usando, elija otro.");
        }
        
        try
        {
            User user = UserMapper.toEntity(dtoRequest);
            User guardado = userRepository.Create(user);
            
            Log.Information($"Usuario {guardado.Name} creado");
            Console.WriteLine($"Usuario {guardado.Name} creado correctamente.");
            
            // User -> DTOResponse
            UserDTOResponse response = UserMapper.toDTOResponse(guardado);

            userCache.Set(response);
            
            // Las páginas pueden haber quedado desactualizadas
            userCache.RemoveAllPages();

            Log.Information("Usuario con id {UserId} añadido y guardado en caché", guardado.Id);
            
            return response;
        }
        catch (SqliteException e)
        {
            Log.Error(e,"SQLite lanzó una excepción al intentar crear el usuario");
            throw;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public UserDTOResponse FindById(int id)
    {
        if (id <= 0) throw new ArgumentException("El Id debe ser mayor que 0.");
        
        UserDTOResponse? cachedUser = userCache.GetId(id);
        
        if (cachedUser != null)
        {
            Log.Information("Usuario con Id {UserId} obtenido desde caché", id);
            return cachedUser;
        }
        
        try
        {
            User? user = userRepository.FindById(id);

            if (user == null)
            {
                Log.Warning($"El id del usuario {id} no existe actualmente en la BD");
                throw new NotFoundException($"El usuario con id {id} no existe");
            }
            
            UserDTOResponse response = UserMapper.toDTOResponse(user);

            userCache.Set(response);

            Log.Information("Usuario con Id {UserId} obtenido desde SQLite y guardado en caché", id);
            
            // Aqui no ponemos Log debido a que es innecesario, ya que son datos personales de los usuarios 
            Console.WriteLine($"Id de usuario: {user.Id} \n" +
                              $"ALias: {user.Alias} \n" +
                              $"Nombre: {user.Name} \n" +
                              $"Número de telefono: {user.Phone} \n" +
                              $"Email: {user.Email}");

            // User -> DTOResponse
            return response;
        }
        catch (SqliteException ex)
        {
            Log.Error(ex,"SQLite lanzó una excepción al intentar buscar un usuario por su id");
            throw;
        }
    }
/// <summary>
/// 
/// </summary>
/// <param name="alias"></param>
/// <returns></returns>
/// <exception cref="ArgumentException"></exception>
/// <exception cref="NotFoundException"></exception>
/// <exception cref="Exception"></exception>
    public UserDTOResponse? FindByAlias(string alias)
    {
        if (string.IsNullOrWhiteSpace(alias))
        {
            throw new ArgumentException("El alias no puede estar vacío.");
        }
        
        UserDTOResponse? cachedUser = userCache.GetAlias(alias);
        
        if (cachedUser != null)
        {
            Log.Information("Usuario con alias {UserAlias} obtenido desde caché", alias);
            return cachedUser;
        }
        

        try
        {
            User? user = userRepository.FindByAlias(alias);

            if (user == null)
            {
                Log.Warning("El alias añadido no existe");
                throw new NotFoundException($"No se encontró a ningún usuario con el alias {alias}");
            }

            UserDTOResponse response = UserMapper.toDTOResponse(user);

            userCache.Set(response);

            Log.Information("Usuario con alias {UserAlias} obtenido desde SQLite y guardado en caché", alias);
            
            Console.WriteLine($"Id de usuario: {user.Id} " +
                              $"ALias: {user.Alias} " +
                              $"Nombre: {user.Name} " +
                              $"Número de telefono: {user.Phone} " +
                              $"Email: {user.Email}");

            return response;
        }
        catch (SqliteException ex)
        {
            Log.Error(ex,"SQLite lanzó una excepción al intentar buscar un usuario por su alias");
            throw;
        }
    }
/// <summary>
/// 
/// </summary>
/// <param name="page"></param>
/// <returns></returns>
/// <exception cref="ArgumentException"></exception>
/// <exception cref="NotFoundException"></exception>
/// <exception cref="Exception"></exception>
    public List<UserDTOResponse> FindByPagination(int page)
    {
        if (page <= 0)
        {
            throw new ArgumentException("La pagina debe ser mayor que 0");
        }

        List<UserDTOResponse>? cachedPage = userCache.GetPage(page);
        
        if (cachedPage != null)
        {
            Log.Information("Número de pagina {pagina} obtenida desde caché", page);
            return cachedPage;
        }
        
        try
        {
            List<User> users = userRepository.FindByPagination(page);

            if (users.Count == 0)
            {
                Log.Warning("El número de pagina añadido no existe");
                throw new NotFoundException($"No se encontró ninguna pagina con el número {page}");
            }
            
            
            
            Log.Information($"En la pagina {page} se encontro a {users.Count} usuarios:");
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
            // Guardamos pagina en cache
            userCache.SetPage(page, response);
            Log.Information("Página {Page} obtenida desde SQLite y guardada en caché", page);
            
            return response;
        }
        catch (SqliteException ex)
        {
            Log.Error(ex,"SQLite lanzó una excepción al intentar buscar a los usuarios por pagina");
            throw;
        }
    }
/// <summary>
/// Actualizar usuario
/// </summary>
/// <param name="dtoRequest">datos de la petición</param>
/// <returns>Envia los datos a la BD</returns>
/// <exception cref="ArgumentException">Valor incorrecto</exception>
/// <exception cref="NotFoundException">No existe el valor</exception>
/// <exception cref="InvalidOperationException">Error al poner el mismo alias que otro usuaro</exception>
/// <exception cref="Exception">Error de BD</exception>
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
            Log.Warning("El id del usuario {} no existe", dtoRequest.Id);
            throw new NotFoundException($"El usuario con Id {dtoRequest.Id} no existe");
        }

        try 
        {
            // Comprobar que el usuario existe
            User? usuarioExistente = userRepository.FindById(dtoRequest.Id);
            
            User? usuarioConAlias = userRepository.FindByAlias(dtoRequest.Alias);
            if(usuarioConAlias != null && usuarioConAlias.Id != dtoRequest.Id)
            {
                throw new InvalidOperationException($"El Alias {dtoRequest.Alias} ya existe, porfavor cambielo.");
            }
            
            string aliasAntiguo = usuarioExistente.Alias;
            
            User user = UserMapper.toEntity(dtoRequest);
            User actualizado = userRepository.Update(user);
        
            Log.Information($"Usuario {dtoRequest.Name} atualizado");
            Console.WriteLine($"Usuario {dtoRequest.Name} atualizado");
            
            // Eliminar el alias antiguo del caché
            userCache.RemoveAlias(aliasAntiguo);

            // User -> DTOResponse
            UserDTOResponse response = UserMapper.toDTOResponse(actualizado);

            // Actualizar caché por ID y por alias
            userCache.Set(response);
            userCache.SetAlias(response);

            // Las páginas pueden haber quedado desactualizadas
            userCache.RemoveAllPages();
            
            Log.Information("Usuario con id {UserId} actualizado y guardado en caché", actualizado.Id);

            return response;
        }
        catch (SqliteException ex)
        {
            Log.Error(ex,"SQLite lanzó una excepción al intentar actualizar un usuario");
            throw;
        }
    }
    
/// <summary>
/// Eliminar un usuario
/// </summary>
/// <param name="id">Id del usuario a eliminar</param>
/// <returns>Envia los datos a la BD</returns>
/// <exception cref="Exception">Error de BD</exception>
/// <exception cref="NotFoundException">Error de que no se ha encontrado un valor</exception>
    public UserDTOResponse Delete(int id)
    {
        if (id <= 0)
        {
           throw new ArgumentException("Debes introducir un id mator de 0");
        }
   
        try
        {
            //para ver si existe el user
            User? user = userRepository.FindById(id);
            if (user == null)
            {
                Log.Warning("El id del usuario {} es invalido", id);
                throw new NotFoundException($"El usuario con id {id} no existe");
            }
            
            userRepository.Delete(id);
            
            userCache.Remove(id);
            
            // Eliminar del caché por alias
            userCache.RemoveAlias(user.Alias); 
            
            // Las páginas pueden haber cambiado
            userCache.RemoveAllPages(); 
            
            Log.Information( "Usuario con Id {UserId} y alias {Alias} eliminado correctamente", user.Id, user.Alias);
            
            Console.WriteLine($"Usuario con Id {id} eliminado");
            
            return UserMapper.toDTOResponse(user);
        }
        catch (SqliteException ex)
        {
            Log.Error(ex,"SQLite lanzó una excepción al intentar eliminar un usuario");
            throw;
        }
    }
/// <summary>
/// Validaciones de entrada de valores para acciones como Crear o Actualizar
/// </summary>
/// <param name="dtoRequest"></param>
/// <exception cref="ArgumentNullException">Error por si los valores son nulos</exception>
/// <exception cref="ArgumentException">Error al introducir valores</exception>
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
            throw new ArgumentException($"El nombre, telefono y correo electronico no pueden quedar vacios");
        }

        if (dtoRequest.Name.Length > 80)
        {
            throw new ArgumentException($"El nombre no puede superar los 80 caracteres");
        }
        if (dtoRequest.Alias.Length > 40)
        {
            throw new ArgumentException($"El alias no puede superar los 40 caracteres");
        }
        if (dtoRequest.Phone.Length > 15)
        {
            throw new ArgumentException($"El telefono no puede superar los 15 caracteres");
        }
        if (dtoRequest.Email.Length > 50)
        {
            throw new ArgumentException($"El Email no puede superar los 50 caracteres");
        }
        
    }
    
    
}