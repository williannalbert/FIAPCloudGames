using Application.DTOs.Game;
using Application.DTOs.Sale;
using Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTests.Controllers;

public class GameControllerTests
{
    private readonly Mock<IGameService> _gameServiceMock;
    private readonly GameController _controller;
    public GameControllerTests()
    {
        _gameServiceMock = new Mock<IGameService>();
        _controller = new GameController(_gameServiceMock.Object);
    }
    [Fact]
    public async Task GetGameSuccess()
    {
        var gameId = Guid.NewGuid();
        var gameDTO = new GameDTO()
        {
            Id = gameId,
            Name = "Game teste",
            Description = "Description Game teste",
            Price = 80.66m
        };

        _gameServiceMock.Setup(s => s.GetAsync(gameId)).ReturnsAsync(gameDTO);

        var result = await _controller.Get(gameId);

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(gameDTO);
    }
    [Fact]
    public async Task GetGameNotFound()
    {
        var gameId = Guid.NewGuid();

        _gameServiceMock.Setup(s => s.GetAsync(gameId)).ReturnsAsync((GameDTO?)null);

        var result = await _controller.Get(gameId);
        Assert.IsType<NotFoundResult>(result);

    }

    [Fact]
    public async Task GetGameException()
    {
        var gameId = Guid.NewGuid();
        _gameServiceMock.Setup(s => s.GetAsync(gameId)).ThrowsAsync(new Exception("Erro inesperado"));

        await Assert.ThrowsAsync<Exception>(() => _controller.Get(gameId));
    }

    [Fact]
    public async Task PostGameSuccess()
    {
        var createGameDTO = new CreateGameDTO
        {
            Name = "Game teste",
            Description = "Description Game teste",
            Price = 100.50m
        };

        var newGameDTO = new GameDTO
        {
            Id = Guid.NewGuid(),
            Name = createGameDTO.Name,
            Description = createGameDTO.Description,
            Price = createGameDTO.Price
        };

        _gameServiceMock.Setup(s => s.CreateAsync(createGameDTO)).ReturnsAsync(newGameDTO);

        var result = await _controller.Post(createGameDTO);

        var createdAtRouteResult = result as CreatedAtRouteResult;
        createdAtRouteResult.Should().NotBeNull();
        createdAtRouteResult!.StatusCode.Should().Be(201);
        createdAtRouteResult.Value.Should().BeEquivalentTo(newGameDTO);
        createdAtRouteResult.RouteName.Should().Be("GetGame"); 
    }

    [Fact]
    public async Task PostGameBadRequest()
    {
        CreateGameDTO? createGameDTO = null;

        var result = await _controller.Post(createGameDTO);

        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().Be("Dados inválidos");
    }

    [Fact]
    public async Task PostGameException()
    {
        var createGameDTO = new CreateGameDTO
        {
            Name = "Game teste",
            Description = "Description Game teste",
            Price = 100.50m
        };

        _gameServiceMock.Setup(s => s.CreateAsync(createGameDTO)).ThrowsAsync(new Exception("Erro inesperado"));

        await Assert.ThrowsAsync<Exception>(() => _controller.Post(createGameDTO));
    }

    [Fact]
    public async Task GetAllGamesSuccess()
    {
        var games = new List<GameDTO>
        {   
            new GameDTO { Id = Guid.NewGuid(), Name = "Test Game 1", Description = "Description 1", Price = 50.0m },
            new GameDTO { Id = Guid.NewGuid(), Name = "Test Game 2", Description = "Description 2", Price = 70.0m }
        };

        _gameServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(games);

        var result = await _controller.GetAll();

        var okResult = result.Result as OkObjectResult; 
        okResult.Should().NotBeNull(); 
        okResult.StatusCode.Should().Be(200); 
        okResult.Value.Should().BeEquivalentTo(games);
    }

    [Fact]
    public async Task GetAllGamesNotFound()
    {
        _gameServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync((IEnumerable<GameDTO>)null);  

        var result = await _controller.GetAll();

        var notFoundResult = result.Result as NotFoundResult;  
        notFoundResult.Should().NotBeNull();  
        notFoundResult.StatusCode.Should().Be(404);  
    }

    [Fact]
    public async Task GetAllGamesNotFoundEmpty()
    {
        var emptyGamesList = new List<GameDTO>(); 
        _gameServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(emptyGamesList);

        var result = await _controller.GetAll();

        var notFoundResult = result.Result as NotFoundResult; 
        notFoundResult.Should().NotBeNull();
        notFoundResult.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task GetAllGamesException()
    {
        _gameServiceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("Erro inesperado"));

        await Assert.ThrowsAsync<Exception>(() => _controller.GetAll());
    }

    [Fact]
    public async Task DeleteGameSuccess()
    {
        var gameId = Guid.NewGuid();
        _gameServiceMock.Setup(s => s.DeleteAsync(gameId)).ReturnsAsync(true);

        var result = await _controller.Delete(gameId);

        (result as NoContentResult)!.StatusCode.Should().Be(204);
    }

    [Fact]
    public async Task DeleteGameNotFound()
    {
        var gameId = Guid.NewGuid();
        _gameServiceMock.Setup(s => s.DeleteAsync(gameId)).ReturnsAsync(false);

        var result = await _controller.Delete(gameId);

        (result as NotFoundResult)!.StatusCode.Should().Be(404);
    }

    [Fact]
    public async Task DeleteGameException()
    {
        var gameId = Guid.NewGuid();
        _gameServiceMock.Setup(s => s.DeleteAsync(gameId)).ThrowsAsync(new Exception("Erro interno"));

        await Assert.ThrowsAsync<Exception>(() => _controller.Delete(gameId));
    }

    [Fact]
    public async Task PutGameSuccess()
    {
        var gameId = Guid.NewGuid();
        var updateGameDTO = new UpdateGameDTO
        {
            Id = gameId,
            Name = "Game teste update",
            Description = "Game teste description",
            Price = 99.99m
        };

        var expectedGameDTO = new GameDTO
        {
            Id = gameId,
            Name = updateGameDTO.Name,
            Description = updateGameDTO.Description,
            Price = updateGameDTO.Price
        };

        _gameServiceMock.Setup(s => s.UpdateAsync(gameId, updateGameDTO)).ReturnsAsync(expectedGameDTO);

        var result = await _controller.Put(gameId, updateGameDTO);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedGameDTO);
    }

    [Fact]
    public async Task PutGameDifferentIdsException()
    {
        var gameId = Guid.NewGuid();
        var differentId = Guid.NewGuid();

        var updateGameDTO = new UpdateGameDTO
        {
            Id = differentId,
            Name = "Game teste",
            Description = "Game teste description",
            Price = 59.99m
        };

        await Assert.ThrowsAsync<Exception>(() => _controller.Put(gameId, updateGameDTO));
    }

    [Fact]
    public async Task PutGameException()
    {
        var gameId = Guid.NewGuid();
        var updateGameDTO = new UpdateGameDTO
        {
            Id = gameId,
            Name = "Error Game",
            Description = "Error",
            Price = 10.0m
        };

        _gameServiceMock.Setup(s => s.UpdateAsync(gameId, updateGameDTO))
            .ThrowsAsync(new Exception("Erro inesperado"));

        await Assert.ThrowsAsync<Exception>(() => _controller.Put(gameId, updateGameDTO));
    }
}
