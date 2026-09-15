using NUnit.Framework;
using Moq;
using FluentAssertions;

using PracticaAgenda.Logica.DTOs;
using PracticaAgenda.Logica.Mapper;
using PracticaAgenda.Logica.Models;

namespace PracticaAgenda.Test.Mapper;

[TestFixture]
public class UserMapperTests
{

    private UserMapper _userMapper = new UserMapper();
    
    private UserDTORequest _userDTORequest;
    private User user;
    
    [SetUp]
    public void SetUp()
    {
        _userMapper = new UserMapper();

        _userDTORequest = new UserDTORequest()
        {
            Id = 1,
            Alias = "Cristian",
            Name = "Cristiano",
            Phone = "12345678",
            Email = "brabo@gmail.com",
        };
        
        user = new User()
        {
            Id = 1,
            Alias = "Cristian",
            Name = "Cristiano",
            Phone = "12345678",
            Email = "brabo@gmail.com"
        };
    }
    
    [Test]
    public void ToEntity_Ok()
    {
        // Act (ejecutar)
        var usuario = UserMapper.toEntity(_userDTORequest);
    
        // Assert (Validar con FluentAssertions --> Mas comprensible)
        usuario.Should().NotBeNull();  // Comprobamos que usuario no sea nulo
        // Comprobamos que ningun valor llegue como nulo, vacio o cambiado
        usuario.Id.Should().Be(_userDTORequest.Id); 
        usuario.Alias.Should().Be(_userDTORequest.Alias);
        usuario.Name.Should().Be(_userDTORequest.Name);
        usuario.Phone.Should().Be(_userDTORequest.Phone);
        usuario.Email.Should().Be(_userDTORequest.Email);
    }
    
    [Test]
    public void ToDTOResponse_Ok()
    {
        // Act (ejecutar)
        var usuario = UserMapper.toDTOResponse(user);
        
        usuario.Should().NotBeNull();  
        usuario.Id.Should().Be(user.Id); 
        usuario.Alias.Should().Be(user.Alias);
        usuario.Name.Should().Be(user.Name);
        usuario.Phone.Should().Be(user.Phone);
        usuario.Email.Should().Be(user.Email);
    }
}