using Microsoft.Data.Sqlite;
using PracticaAgenda.Logica.Models;

namespace PracticaAgenda.Logica.Repositories;

public class UserRepository : IUserRepository
{

    private readonly string _connectionString =
        $"Data Source={Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName, "agenda.db")}"; 
    
       
    
    
    /// <summary>
    /// Bucar usuario por Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Devuelve Toda la inormacion del usuario</returns>
    public User? FindById(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = """
                     SELECT *
                     FROM  User
                     WHERE Id = @id;
                     """;
        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Alias = reader.GetString(1),
                Name = reader.GetString(2),
                Phone = reader.GetString(3),
                Email = reader.GetString(4)
            };
        }
        return null;
    }
/// <summary>
/// Buscar un usuario por su alias
/// </summary>
/// <param name="alias"></param>
/// <returns>Devuelve toda laa imformacion del usuario</returns>
    public User? FindByAlias(string? alias)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = """
                     SELECT Id, Alias, Name, Phone, Email
                     FROM  User
                     WHERE Alias = @alias;
                     """;
        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@alias", alias);
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Alias = reader.GetString(1),
                Name = reader.GetString(2),
                Phone = reader.GetString(3),
                Email = reader.GetString(4)
            };
        }
        return null;
    }
    /// <summary>
    /// Buscamos a usuarios por su nº de pagina, en cada pagina hay 2 usuarios
    /// </summary>
    /// <param name="page"></param>
    /// <returns>Lista de usuarios por pagina</returns>
    public List<User> FindByPagination(int page)
    {
        var users = new List<User>();
        // usuarios por pagina
        const int pageSize = 2;
        //Aqui hacemos que se salten un nº de paginas hasta llegar a la que queremos
        int registros = (page - 1) * pageSize;
        
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();
        string sql = """
                     SELECT *
                     FROM  users
                     ORDER BY Id
                     LIMIT @pageSize OFFSET @registros;
                     """;
        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@pageSize", pageSize);
        command.Parameters.AddWithValue("@registros", registros);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var user = new User
            {
                Id = reader.GetInt32(0),
                Alias = reader.GetString(1),
                Name = reader.GetString(2),
                Phone = reader.GetString(3),
                Email = reader.GetString(4)
            };

            users.Add(user);
        }
        return users;
    }
/// <summary>
/// Creamos a un nuevo usuario
/// </summary>
/// <param name="user"></param>
/// <returns></returns>
    public User Create(User user)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        string sql = """
                     INSERT INTO User (Name, Alias, Phone, Email)
                     VALUES (@name, @alias, @phone, @email);
                     SELECT last_insert_rowid(); 
                     """;
        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@name", user.Name);
        command.Parameters.AddWithValue("@alias", user.Alias);
        command.Parameters.AddWithValue("@phone", user.Phone);
        command.Parameters.AddWithValue("@email", user.Email);
        
        long id = (long)command.ExecuteScalar()!;
        user.Id = (int)id;

        return user;
    }
/// <summary>
/// Actualizamos a un usuario determinado
/// </summary>
/// <param name="user"></param>
/// <returns></returns>
    public User Update(User user)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        string sql = """
                     UPDATE User
                     SET Name = @name, Alias = @alias, Phone = @phone, Email = @email
                     WHERE Id = @id;
                     """;
        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@id", user.Id);
        command.Parameters.AddWithValue("@name", user.Name);
        command.Parameters.AddWithValue("@alias", user.Alias);
        command.Parameters.AddWithValue("@phone", user.Phone);
        command.Parameters.AddWithValue("@email", user.Email);
        
        command.ExecuteNonQuery();

        Console.WriteLine($"Usuario {user.Name} ha sido atualizado");

        return user;
    }
/// <summary>
/// Eliminamos a un usuario determinado
/// </summary>
/// <param name="user"></param>
    public void Delete(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        string sql = """
                     DELETE 
                     FROM User
                     WHERE Id = @id;
                     """;
        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@id", id);
        
        command.ExecuteNonQuery();

        Console.WriteLine($"Usuario ELIMINADO");
    }

    public void NombreTqblq()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        Console.WriteLine(_connectionString);
    }
    
}