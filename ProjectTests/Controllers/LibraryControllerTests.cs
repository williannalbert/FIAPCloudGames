using Application.DTOs.Game;
using Application.DTOs.Library;
using Application.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTests.Controllers;

public class LibraryControllerTests
{
    private readonly Mock<ILibraryService> _libraryServiceMock;
    private readonly Mock<ITokenInformationsServices> _tokenServiceMock;
    private readonly LibraryController _controller;
    public LibraryControllerTests()
    {
        _libraryServiceMock = new Mock<ILibraryService>();
        _tokenServiceMock = new Mock<ITokenInformationsServices>();
        _controller = new LibraryController(_tokenServiceMock.Object, _libraryServiceMock.Object);
    }
    [Fact]
    public async Task GetLibrarySuccess()
    {
        var userId = Guid.NewGuid();

        var libraryDTO = new LibraryDTO
        {
            UserId = userId,
            Games = new List<GameDTO>
            {
                new GameDTO { Id = Guid.NewGuid(), Name = "Jogo Teste", Description = "Description test", Price = 99.99m }
            }
        };

        var tokenServiceMock = new Mock<ITokenInformationsServices>();
        var libraryServiceMock = new Mock<ILibraryService>();

        tokenServiceMock.Setup(s => s.GetUserId()).Returns(userId);
        libraryServiceMock.Setup(s => s.GetLibraryByUserIdAsync(userId))
                          .ReturnsAsync(libraryDTO);

        var controller = new LibraryController(tokenServiceMock.Object, libraryServiceMock.Object);

        var result = await controller.Get();

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(libraryDTO);
    }

    [Fact]
    public async Task GetLbraryNoContent()
    {
        var userId = Guid.NewGuid();

        var tokenServiceMock = new Mock<ITokenInformationsServices>();
        var libraryServiceMock = new Mock<ILibraryService>();

        tokenServiceMock.Setup(s => s.GetUserId()).Returns(userId);
        libraryServiceMock.Setup(s => s.GetLibraryByUserIdAsync(userId))
                          .ReturnsAsync((LibraryDTO)null);

        var controller = new LibraryController(tokenServiceMock.Object, libraryServiceMock.Object);

        var result = await controller.Get();

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task GetLibraryException()
    {
        var userId = Guid.NewGuid();

        var tokenServiceMock = new Mock<ITokenInformationsServices>();
        var libraryServiceMock = new Mock<ILibraryService>();

        tokenServiceMock.Setup(s => s.GetUserId()).Returns(userId);
        libraryServiceMock.Setup(s => s.GetLibraryByUserIdAsync(userId))
                          .ThrowsAsync(new Exception("Erro interno"));

        var controller = new LibraryController(tokenServiceMock.Object, libraryServiceMock.Object);

        await Assert.ThrowsAsync<Exception>(() => controller.Get());
    }
}
