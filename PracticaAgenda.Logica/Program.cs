using Microsoft.Extensions.Caching.Memory;
using PracticaAgenda.Logica.Repositories;
using Serilog;
using PracticaAgenda.Logica.DTOs;
using PracticaAgenda.Logica.Mapper;
using PracticaAgenda.Logica.Services;
using PracticaAgenda.Logica.Cache;

Console.WriteLine("Hello, World!");

UserRepository userRepository = new UserRepository();
userRepository.NombreTqblq();


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console() // mostrar logs por consola
    .WriteTo.File(Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName, "logs", "app.log")) // guardar logs en un archivo
    .CreateLogger();// crear logger

MemoryCache m = new MemoryCache(new MemoryCacheOptions());
UserCache userMapper = new UserCache(m);
IUserService iService = new UserService(userRepository, userMapper);

bool creado = false;
bool buscadoID = false;
bool buscadoAlias = false;
bool buscadoPage = false;
bool actualizado = false;
bool eliminado = false;

// ========================================== AGENDA =====================================
/*
 *  1. CREAR USUARIO
 */
while (!creado)
{
    try
    {
        Console.WriteLine("============================ CREACIÓN DE USUARIO DE AGENDA ============================");
        Console.WriteLine("Introduzca el alias del usuario de agenda:");
        string? alias = Console.ReadLine();

        Console.WriteLine("Introduzca el nombre del usuario de agenda:");
        string? name = Console.ReadLine();

        Console.WriteLine("Introduzca el número de telefono del usuario de agenda:");
        string? phone = Console.ReadLine();

        Console.WriteLine("Introduzca el email del usuario de agenda:");
        string? email = Console.ReadLine();

        UserDTORequest userDtoRequest = new UserDTORequest
        {
            Alias = alias,
            Name = name,
            Phone = phone,
            Email = email
        };

        UserDTOResponse user = iService.Create(userDtoRequest);

        Console.WriteLine(user);
        
        creado = true;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        Console.WriteLine("Vuelva a introducir los datos del usuario.\n");
    }
}
  
/*
 *  2. BUSCAR USUARIO POR ID
 */
while (!buscadoID)
{
    try
    {
        Console.WriteLine("============================ BUSQUEDA DE USUARIO POR ID ============================");
        Console.WriteLine("Introduzca el ID del usuario que desea buacar:");
        string? idStr = Console.ReadLine();
        int.TryParse(idStr, out int id);

        UserDTOResponse userId = iService.FindById(id);
        Console.WriteLine(userId);
        
        buscadoID = true;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
        Console.WriteLine("Vuelva a introducir los datos para buscar un usuario por ID.\n");
    }
}
 
/*
 *  3. BUSCAR USUARIO POR ALIAS
 */
while (!buscadoAlias)
{
    try
    {
        Console.WriteLine("============================ BUSQUEDA DE USUARIO POR ALIAS ============================");
        Console.WriteLine("Introduzca el Alias del usuario que desea buacar:");
        string? aliasStr = Console.ReadLine();

        UserDTOResponse userAlias = iService.FindByAlias(aliasStr);
        Console.WriteLine(userAlias);
        
        buscadoAlias =  true;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
        Console.WriteLine("Vuelva a introducir los datos del usuario para buscar un usuario por su Alias.\n");
    }
}

/*
 *  4. BUSCAR USUARIOS POR PAGINACIÓN
 */
while (!buscadoPage)
{
    try
    {
        Console.WriteLine("============================ BUSQUEDA DE USUARIO POR PAGINACIÓN ============================");
        Console.WriteLine("Introduzca el número de pagina que desea buscar:");
        string? pageStr = Console.ReadLine();
        int.TryParse(pageStr, out int page);

        List<UserDTOResponse> userPage = iService.FindByPagination(page);
        Console.WriteLine(userPage);

        buscadoPage = true;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
        Console.WriteLine("Vuelva a introducir los datos del usuario para buscar un usuario por Paginación.\n");
    }
}


/*
 *  5. ACTUALIZAR USUARIO
 */
while (!actualizado)
{
    try
    {
        Console.WriteLine("============================ ACTUALIZACIÓN DEL USUARIO DE AGENDA ============================");
        Console.WriteLine("Introduzca el ID del usuiario a modificar:");
        string? idUpdateStr = Console.ReadLine();
        int.TryParse(idUpdateStr, out int idUpdate);

        Console.WriteLine("Introduzca el alias nuevo:");
        string? aliasUpdate = Console.ReadLine();

        Console.WriteLine("Introduzca el nombre nuevo:");
        string? nameUpdate = Console.ReadLine();

        Console.WriteLine("Introduzca el número de telefono nuevo:");
        string? phoneUpdate = Console.ReadLine();

        Console.WriteLine("Introduzca el email del usuario nuevo:");
        string? emailUpdate = Console.ReadLine();

        UserDTORequest DtoRequest = new UserDTORequest
        {
            Id = idUpdate,
            Alias = aliasUpdate,
            Name = nameUpdate,
            Phone = phoneUpdate,
            Email = emailUpdate
        };
 
        UserDTOResponse userUpdate = iService.Update(DtoRequest);

        Console.WriteLine(userUpdate);
        
        actualizado = true;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
        Console.WriteLine("Vuelva a introducir los datos para modificar el usuario.\n");
    }
}
/*
 *  6. ELIMINAR USUARIO
 */
while (!eliminado)
{
    try
    {
        Console.WriteLine("============================ ELIMINACIÓN DEL USUARIO DE AGENDA ============================");
        Console.WriteLine("Introduzca el ID del usuiario a eliminar:");
        string? idRemoveStr = Console.ReadLine();
        int.TryParse(idRemoveStr, out int idRemove);
 
        UserDTOResponse userRemove = iService.Delete(idRemove);

        Console.WriteLine(userRemove);
        
        eliminado = true;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
        Console.WriteLine("Vuelva a introducir los datos para eliminar un usuario.\n");
    }
}


Log.CloseAndFlush();
