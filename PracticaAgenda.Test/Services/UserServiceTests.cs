using NUnit.Framework;
using Moq;
using FluentAssertions;

using PracticaAgenda.Logica.Services;
using PracticaAgenda.Logica.DTOs;
using PracticaAgenda.Logica.Exceptions;
using PracticaAgenda.Logica.Mapper;
using PracticaAgenda.Logica.Models;
using PracticaAgenda.Logica.Repositories;

namespace PracticaAgenda.Test.Services;

[TestFixture]
public class UserServiceTests
{
    
    private UserService userService;
    
    private Mock<IUserRepository> userRepository;
    private UserMapper userMapper;
    
    private User user;
    private UserDTOResponse userDTOResponse;
    private UserDTORequest userDTORequest;
    
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}