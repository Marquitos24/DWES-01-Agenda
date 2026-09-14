using Microsoft.Data.Sqlite;

namespace PracticaAgenda.Logica.Data;

public class Database
{
    private readonly string _connectionString =         $"Data Source={Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName, "agenda.db")}"; // el nombre de la que es nuestra BD

    public SqliteConnection GetConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}