using PracticaAgenda.Logica.DTOs;
using PracticaAgenda.Logica.Models;

namespace PracticaAgenda.Logica.Mapper;

public class UserMapper
{
    public static User toEntity(UserDTORequest dto)
    {
        User user = new User();
        user.Alias = dto.Alias;
        user.Name = dto.Name;
        user.Phone = dto.Phone;
        user.Email = dto.Email;

        return user;
    }

    public static UserDTOResponse toDTOResponse(User user)
    {
        UserDTOResponse dto = new UserDTOResponse();
        dto.Id = user.Id;
        dto.Alias = user.Alias;
        dto.Name = user.Name;
        dto.Phone = user.Phone;
        dto.Email = user.Email;
        return dto;
    }
}