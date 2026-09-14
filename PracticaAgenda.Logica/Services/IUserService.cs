using PracticaAgenda.Logica.Models;
using PracticaAgenda.Logica.DTOs;

namespace PracticaAgenda.Logica.Services;

public interface IUserService
{
    UserDTOResponse Create(UserDTORequest dto);
    UserDTOResponse? FindById(int id);
    UserDTOResponse? FindByAlias(string alias);
    List<UserDTOResponse> FindByPagination(int page);
    UserDTOResponse Update(UserDTORequest dto);
    UserDTOResponse Delete(int id);
}