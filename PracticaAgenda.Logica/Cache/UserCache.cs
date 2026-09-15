using Microsoft.Extensions.Caching.Memory;
using PracticaAgenda.Logica.DTOs;

namespace PracticaAgenda.Logica.Cache;

public class UserCache
{
    private readonly MemoryCache _cache;
    private readonly List<string> _pageKeys = new();

    public UserCache(MemoryCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// Añadimos el guardado en cache para los ids por si hay actualizaciones o eliminaciones de estos
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public UserDTOResponse? GetId(int id)
    {
        return _cache.Get<UserDTOResponse>($"id:{id}");
    }
    
    /// <summary>
    /// Añadimos el guardado en cache para los alias por si hay actualizaciones o eliminaciones de estos
    /// </summary>
    /// <param name="alias"></param>
    /// <returns></returns>
    public UserDTOResponse? GetAlias(string alias)
    {
        return _cache.Get<UserDTOResponse>($"alias:{alias}");
    }
    public void SetAlias(UserDTOResponse user)
    {
        _cache.Set($"alias:{user.Alias}", user, TimeSpan.FromMinutes(5));
    }
    public void RemoveAlias(string alias)
    {
        _cache.Remove($"alias:{alias}");
    }
    
    /// <summary>
    /// Añadimos el guardado en cache para las paginas por si hay actualizaciones o eliminaciones de estas
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public List<UserDTOResponse>? GetPage(int page)
    {
        return _cache.Get<List<UserDTOResponse>>($"page:{page}");
    }
    // Este metodo get se usa para cuando se crea o actualiza que no tenga problemas deen las paginas
    public void SetPage(int page, List<UserDTOResponse> users)
    {
        string key = $"page:{page}";

        _cache.Set(key, users, TimeSpan.FromMinutes(5));

        if (!_pageKeys.Contains(key))
        {
            _pageKeys.Add(key);
        }
    }
    public void RemovePage(int page)
    {
        _cache.Remove($"page:{page}");
    }
    // Para cuando se crea un nuevo usuario y se borren todo del cache
    public void RemoveAllPages()
    {
        foreach (string key in _pageKeys)
        {
            _cache.Remove(key);
        }

        _pageKeys.Clear();
    }
    
    /// <summary>
    /// Guarda el ussuario hasta 5 minutos. Ademas tambien vale para actualizzar ya que reemplaza el valor
    /// </summary>
    /// <param name="user"></param>
    public void Set(UserDTOResponse user)
    {
        _cache.Set(user.Id, user, TimeSpan.FromMinutes(5));
    }

    /// <summary>
    /// Elimina el usuario del cache
    /// </summary>
    /// <param name="id"></param>
    public void Remove(int id)
    {
        _cache.Remove(id);
    }
}