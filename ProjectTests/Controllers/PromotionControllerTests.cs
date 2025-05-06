using Application.DTOs.Game;
using Application.DTOs.GamePromotion;
using Application.DTOs.Promotion;
using Application.Interfaces;
using Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTests.Controllers;

public class PromotionControllerTests
{
    private readonly Mock<IPromotionService> _promotionServiceMock;
    private readonly PromotionController _controller;
    public PromotionControllerTests()
    {
        _promotionServiceMock = new Mock<IPromotionService>();
        _controller = new PromotionController(_promotionServiceMock.Object);
    }
    [Fact]
    public async Task GetPromotionSuccess()
    {
        var promotionId = Guid.NewGuid();
        var promotionDTO = new PromotionDTO
        {
            Id = promotionId,
            Name = "Promo Test",
            Percentage = 15,
            Enable = true,
            InitialDate = DateTime.Now,
            FinalDate = DateTime.Now.AddDays(5)
        };

        _promotionServiceMock.Setup(s => s.GetAsync(promotionId)).ReturnsAsync(promotionDTO);


        var result = await _controller.Get(promotionId);

        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(promotionDTO);
    }

    [Fact]
    public async Task GetPromotionNotFound()
    {
        var promotionId = Guid.NewGuid();
        _promotionServiceMock.Setup(s => s.GetAsync(promotionId)).ReturnsAsync((PromotionDTO)null);

        var result = await _controller.Get(promotionId);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetPromotionException()
    {
        var promotionId = Guid.NewGuid();
        _promotionServiceMock.Setup(s => s.GetAsync(promotionId))
                             .ThrowsAsync(new Exception("Erro interno"));

        await Assert.ThrowsAsync<Exception>(() => _controller.Get(promotionId));
    }
    [Fact]
    public async Task GetAllPromotionsSuccess()
    {
        var promotions = new List<PromotionDTO>
        {
            new PromotionDTO { 
                Id = Guid.NewGuid(),
                Name = "Promo Test",
                Percentage = 15,
                Enable = true,
                InitialDate = DateTime.Now,
                FinalDate = DateTime.Now.AddDays(5) 
            },
            new PromotionDTO { 
                Id = Guid.NewGuid(),
                Name = "Promo Test 2",
                Percentage = 15,
                Enable = true,
                InitialDate = DateTime.Now,
                FinalDate = DateTime.Now.AddDays(1)
            }
        };

        _promotionServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(promotions);

        var result = await _controller.GetAll();

        var okResult = result.Result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(promotions);
    }

    [Fact]
    public async Task GetAllPromotionsNotFound()
    {
        _promotionServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync((IEnumerable<PromotionDTO>)null);

        var result = await _controller.GetAll();

        result.Result.Should().BeOfType<NotFoundResult>();
    }
    [Fact]
    public async Task GetAllPromotionsException()
    {
        _promotionServiceMock.Setup(s => s.GetAllAsync()).ThrowsAsync(new Exception("Erro interno"));

        await Assert.ThrowsAsync<Exception>(() => _controller.GetAll());
    }

    [Fact]
    public async Task PostPromotionSuccess()
    {
        var promotionDTO = new PromotionDTO {
            Id = Guid.NewGuid(),
            Name = "Promo Test",
            Percentage = 15,
            Enable = true,
            InitialDate = DateTime.Now,
            FinalDate = DateTime.Now.AddDays(5)
        };
        _promotionServiceMock.Setup(s => s.CreateAsync(promotionDTO)).ReturnsAsync(promotionDTO);

        var result = await _controller.Post(promotionDTO);

        var createdAtRoute = result.Result as CreatedAtRouteResult;
        createdAtRoute!.StatusCode.Should().Be(201);
        createdAtRoute.Value.Should().BeEquivalentTo(promotionDTO);
    }

    [Fact]
    public async Task PostPromotionExceptionNull()
    {
        await Assert.ThrowsAsync<Exception>(() => _controller.Post(null));
    }
    [Fact]
    public async Task PostPromotionException()
    {
        var promotionDTO = new PromotionDTO {
            Id = Guid.NewGuid(),
            Name = "Promo Test",
            Percentage = 15,
            Enable = true,
            InitialDate = DateTime.Now,
            FinalDate = DateTime.Now.AddDays(5)
        };
        _promotionServiceMock.Setup(s => s.CreateAsync(promotionDTO)).ThrowsAsync(new Exception("Erro inesperado"));

        await Assert.ThrowsAsync<Exception>(() => _controller.Post(promotionDTO));
    }

    [Fact]
    public async Task DeletePromotion()
    {
        var promotionId = Guid.NewGuid();
        _promotionServiceMock.Setup(s => s.DeleteAsync(promotionId)).ReturnsAsync(true);

        var result = await _controller.Delete(promotionId);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeletePromotionNotFound()
    {
        var promotionId = Guid.NewGuid();
        _promotionServiceMock.Setup(s => s.DeleteAsync(promotionId)).ReturnsAsync(false);

        var result = await _controller.Delete(promotionId);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeletePromotion_ReturnsBadRequest_WhenExceptionIsThrown()
    {
        var promotionId = Guid.NewGuid();
        _promotionServiceMock.Setup(s => s.DeleteAsync(promotionId)).ThrowsAsync(new Exception("Erro interno"));

        var result = await _controller.Delete(promotionId);

        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task PutPromotionSuccess()
    {
        var promotionId = Guid.NewGuid();
        var promotionDTO = new PromotionDTO
        {
            Id = promotionId,
            Name = "Promo Test",
            Percentage = 15,
            Enable = true,
            InitialDate = DateTime.Now,
            FinalDate = DateTime.Now.AddDays(5)
        };

        _promotionServiceMock.Setup(s => s.UpdateAsync(promotionId, promotionDTO))
                             .ReturnsAsync(promotionDTO);

        var result = await _controller.Put(promotionId, promotionDTO);

        var okResult = result.Result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(promotionDTO);
    }

    [Fact]
    public async Task PutPromotionExceptionIds()
    {
        var routeId = Guid.NewGuid();
        var bodyId = Guid.NewGuid(); 
        var promotionDTO = new PromotionDTO { Id = bodyId };

        await Assert.ThrowsAsync<Exception>(() => _controller.Put(routeId, promotionDTO));
    }

    [Fact]
    public async Task PutPromotionException()
    {
        var promotionId = Guid.NewGuid();
        var promotionDTO = new PromotionDTO
        {
            Id = promotionId,
            Name = "Promo Test",
            Percentage = 15,
            Enable = true,
            InitialDate = DateTime.Now,
            FinalDate = DateTime.Now.AddDays(5)
        };

        _promotionServiceMock.Setup(s => s.UpdateAsync(promotionId, promotionDTO))
                             .ThrowsAsync(new Exception("Erro interno"));

        await Assert.ThrowsAsync<Exception>(() => _controller.Put(promotionId, promotionDTO));
    }

    [Fact]
    public async Task AddGameSuccess()
    {
        var gameDTO = new GameDTO()
        {
            Id = Guid.NewGuid(),
            Name = "Game teste",
            Description = "Description Game teste",
            Price = 80.66m
        };

        var createdPromotionDTO = new PromotionDTO
        {
            Id = Guid.NewGuid(),
            Name = "Promo Test",
            Percentage = 15,
            Enable = true,
            InitialDate = DateTime.Now,
            FinalDate = DateTime.Now.AddDays(5)
        };
        var gamePromotionDTO = new GamePromotionDTO
        {
            PromotionId = createdPromotionDTO.Id,
            GameId = gameDTO.Id
        };

        
        _promotionServiceMock
            .Setup(s => s.AddGamePromotionAsync(gamePromotionDTO))
            .ReturnsAsync(gameDTO);

        var result = await _controller.AddGame(gamePromotionDTO);

        var createdAtRouteResult = result.Result as CreatedAtRouteResult;
        createdAtRouteResult!.RouteName.Should().Be("GetPromotion");
        createdAtRouteResult.RouteValues!["id"].Should().Be(gameDTO.Id);
        createdAtRouteResult.Value.Should().BeEquivalentTo(gameDTO);
    }

    [Fact]
    public async Task AddGameBadRequestNull()
    {
        var result = await _controller.AddGame(null);

        var badRequest = result.Result as BadRequestObjectResult;
        badRequest!.Value.Should().Be("Dados inválidos");
    }

    [Fact]
    public async Task AddGameException()
    {
        var gamePromotionDTO = new GamePromotionDTO
        {
            PromotionId = Guid.NewGuid(),
            GameId = Guid.NewGuid()
        };

        _promotionServiceMock
            .Setup(s => s.AddGamePromotionAsync(gamePromotionDTO))
            .ThrowsAsync(new Exception("Erro interno"));

        await Assert.ThrowsAsync<Exception>(() => _controller.AddGame(gamePromotionDTO));
    }
    [Fact]
    public async Task DeleteGameSuccess()
    {
        var gamePromotionDTO = new GamePromotionDTO
        {
            GameId = Guid.NewGuid(),
            PromotionId = Guid.NewGuid()
        };

        _promotionServiceMock
            .Setup(s => s.DeleteGamePromotionAsync(gamePromotionDTO))
            .ReturnsAsync(true);

        var result = await _controller.DeleteGame(gamePromotionDTO);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteGameBadRequest()
    {
        var result = await _controller.DeleteGame(null);

        var badRequest = result as BadRequestObjectResult;
        badRequest.Should().NotBeNull();
        badRequest!.Value.Should().Be("Dados inválidos");
    }

    [Fact]
    public async Task DeleteGameNotFound()
    {
        var gamePromotionDTO = new GamePromotionDTO
        {
            GameId = Guid.NewGuid(),
            PromotionId = Guid.NewGuid()
        };

        _promotionServiceMock
            .Setup(s => s.DeleteGamePromotionAsync(gamePromotionDTO))
            .ReturnsAsync(false);

        var result = await _controller.DeleteGame(gamePromotionDTO);

        result.Should().BeOfType<NotFoundResult>();
    }
}
