using NUnit.Framework;
using Moq;
using FluentAssertions;

using PracticaAgenda.Logica.Services;
using PracticaAgenda.Logica.DTOs;
using PracticaAgenda.Logica.Exceptions;
using PracticaAgenda.Logica.Mapper;
using PracticaAgenda.Logica.Models;
using PracticaAgenda.Logica.Repositories;
using PracticaAgenda.Logica.Cache;

namespace PracticaAgenda.Test.Services;

[TestFixture]
public class UserServiceTests
{
    
    private UserService _userService;
    
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IUserCache> _userCacheMock;
    
    private UserMapper _userMapper;
    private User user;
    private UserDTOResponse _userDTOResponse;
    private UserDTORequest _userDTORequest;
    
    
    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userCacheMock = new Mock<IUserCache>();
        
        _userDTORequest = new UserDTORequest()
        {
            Id = 1,
            Alias = "Cristian",
            Name = "Cristiano",
            Phone = "12345678",
            Email = "brabo@gmail.com",
        };
        
        _userDTOResponse = new UserDTOResponse()
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
        
        //Repository
        _userRepositoryMock.Setup(repo => repo.FindById(1)).Returns(user);
        _userRepositoryMock.Setup(repo => repo.FindByAlias("Cristian")).Returns((User?)null); // Para el Create, el alias debe venir libre (null)        var users = new List<User>();
        var users = new List<User>();
        _userRepositoryMock.Setup(repo => repo.FindByPagination(3)).Returns(users);
        
        _userRepositoryMock.Setup(repo => repo.Create(It.IsAny<User>())).Returns(user);
        _userRepositoryMock.Setup(repo => repo.Update(It.IsAny<User>())).Returns(user);

        
        //Cache
        _userCacheMock.Setup(cache => cache.GetId(1)).Returns((UserDTOResponse?)null);
        _userCacheMock.Setup(cache => cache.GetAlias("Cristian")).Returns((UserDTOResponse?)null);
        var usersDto = new List<UserDTOResponse>();
        _userCacheMock.Setup(cache => cache.GetPage(3)).Returns(usersDto);
        //Los demas, Set, Remove, etc no los ponemos porque son void, es decir, que no devuelven nada
        
        _userService = new UserService( // le pasamos al servicios los obj mokeados
            _userRepositoryMock.Object,
            _userCacheMock.Object
        );
    }

    [Test]
    public void Create_Ok()
    {
        //Act
        var crear = _userService.Create(_userDTORequest);
        
        // Assert --> Comprobamos que los datos devueltos sean correctos.
        crear.Should().NotBeNull();
        crear.Id.Should().Be(_userDTORequest.Id); 
        crear.Alias.Should().Be(_userDTORequest.Alias);
        crear.Name.Should().Be(_userDTORequest.Name);
        crear.Phone.Should().Be(_userDTORequest.Phone);
        crear.Email.Should().Be(_userDTORequest.Email);
        
        // Verify: comprobar que se llamó al repositorio
        _userRepositoryMock.Verify(r => r.FindByAlias("Cristian"), Times.Once);
        _userRepositoryMock.Verify(r => r.Create(It.IsAny<User>()), Times.Once);
        //Aunque arriva no se allan puesto, si hace falta que se verifiquen
        _userCacheMock.Verify(c => c.Set(It.IsAny<UserDTOResponse>()), Times.Once);
        _userCacheMock.Verify(c => c.RemoveAllPages(), Times.Once);
    }
    
    [Test]
    public void FindById_Ok()
    {
        Assert.Pass();
    }
    
    [Test]
    public void FindByAlias_Ok()
    {
        Assert.Pass();
    }
    
    [Test]
    public void FindByPagination_Ok()
    {
        Assert.Pass();
    }
    
    [Test]
    public void Update_Ok()
    {
        Assert.Pass();
    }
    
    [Test]
    public void Delete_Ok()
    {
        Assert.Pass();
    }
    
    [Test]
    public void ValidarUsuario_Ok()
    {
        Assert.Pass();
    }
    
}