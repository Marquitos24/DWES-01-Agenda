using PracticaAgenda.Logica.DTOs;
namespace PracticaAgenda.Logica.Cache;

public interface IUserCache
{
    UserDTOResponse GetId(int id);
    UserDTOResponse GetAlias(string alias);
    void SetAlias(UserDTOResponse user);
    void RemoveAlias(string alias);
    List<UserDTOResponse>? GetPage(int page);
    void SetPage(int page, List<UserDTOResponse> users);
    void RemovePage(int page);
    void RemoveAllPages();
    void Set(UserDTOResponse user);
    void Remove(int id);
}