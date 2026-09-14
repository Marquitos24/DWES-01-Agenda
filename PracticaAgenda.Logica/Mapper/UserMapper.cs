using PracticaAgenda.Logica.DTOs;
using PracticaAgenda.Logica.Models;

namespace PracticaAgenda.Logica.Mapper;

public class UserMapper
{
    public static User toEntity(UserDTORequest dto)
    {
        return new User
        {
            Id = dto.Id,
            Alias = dto.Alias,
            Name = dto.Name,
            Phone = dto.Phone,
            Email = dto.Email
        };
    }

    public static UserDTOResponse toDTOResponse(User user)
    {
        return new UserDTOResponse
        {
            Id = user.Id,
            Alias = user.Alias,
            Name = user.Name,
            Phone = user.Phone,
            Email = user.Email
        };
    }
}