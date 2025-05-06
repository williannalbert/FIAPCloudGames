using Application.DTOs.User;
using Application.Interfaces;
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

public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly AuthController _controller;
    public AuthControllerTests()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Fact]
    public async Task RegisterUserSuccess()
    {
        var registerDTO = new RegisterUserDTO
        {
            Name = "Test",
            Email = "usertest@test.com",
            Password = "Ut123!",
        };

        var expectedToken = "token";

        _authServiceMock.Setup(s => s.RegisterAsync(registerDTO)).ReturnsAsync(expectedToken);

        var result = await _controller.Register(registerDTO);

        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(new { token = expectedToken });
    }
    [Fact]
    public async Task RegisterUserException()
    {
        var registerDTO = new RegisterUserDTO
        {
            Name = "Test",
            Email = "usertest@test.com",
            Password = "Ut123!",
        };

        _authServiceMock
            .Setup(s => s.RegisterAsync(registerDTO))
            .ThrowsAsync(new Exception("Erro inesperado"));

        await Assert.ThrowsAsync<Exception>(() => _controller.Register(registerDTO));
    }

    [Fact]
    public async Task LoginUserSuccess()
    {
        var loginDTO = new LoginUserDTO
        {
            Email = "usertest@test.com",
            Password = "Ut123!",
        };

        var fakeToken = "token";
        _authServiceMock.Setup(s => s.LoginAsync(loginDTO)).ReturnsAsync(fakeToken);

        var result = await _controller.Login(loginDTO);

        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(new { token = fakeToken });
    }

    [Fact]
    public async Task LoginUserNotFound()
    {
        var loginDTO = new LoginUserDTO
        {
            Email = "usertest@test.com",
            Password = "Ut123!",
        };

        _authServiceMock.Setup(s => s.LoginAsync(loginDTO)).ReturnsAsync((string?)null);

        var result = await _controller.Login(loginDTO);

        var notFound = result as NotFoundObjectResult;
        notFound!.StatusCode.Should().Be(404);
        notFound.Value.Should().Be("Usuário e senha inválidos.");
    }

    [Fact]
    public async Task LoginUserException()
    {
        var loginDTO = new LoginUserDTO
        {
            Email = "usertest@test.com",
            Password = "Ut123!",
        };

        _authServiceMock.Setup(s => s.LoginAsync(loginDTO)).ThrowsAsync(new Exception("Erro inesperado"));

        await Assert.ThrowsAsync<Exception>(() => _controller.Login(loginDTO));
    }

    [Fact]
    public async Task ForgotPasswordSuccess()
    {
        var passUserDTO = new ForgotPasswordUserDto { Email = "usertest@test.com" };
        var fakeToken = "token";

        _authServiceMock.Setup(s => s.GenerateResetPasswordTokenAsync(passUserDTO.Email))
                        .ReturnsAsync(fakeToken);

        var result = await _controller.ForgotPassword(passUserDTO);

        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(new { token = fakeToken });
    }

    [Fact]
    public async Task ForgotPasswordNotFound()
    {
        var passUserDTO = new ForgotPasswordUserDto { Email = "usertest@test.com" };

        _authServiceMock.Setup(s => s.GenerateResetPasswordTokenAsync(passUserDTO.Email))
                        .ReturnsAsync((string?)null);

        var result = await _controller.ForgotPassword(passUserDTO);

        result.Should().BeOfType<NotFoundObjectResult>();
        var notFound = result as NotFoundObjectResult;
        notFound!.StatusCode.Should().Be(404);
        notFound.Value.Should().Be("Usuário não encontrado.");
    }

    [Fact]
    public async Task ForgotPasswordException()
    {
        var passUserDTO = new ForgotPasswordUserDto { Email = "usertest@test.com" };

        _authServiceMock.Setup(s => s.GenerateResetPasswordTokenAsync(passUserDTO.Email))
                        .ThrowsAsync(new Exception("Erro interno"));

        await Assert.ThrowsAsync<Exception>(() => _controller.ForgotPassword(passUserDTO));
    }

    [Fact]
    public async Task ResetPasswordSuccess()
    {
        var dto = new ResetPasswordUserDto
        {
            Email = "usertest@test.com",
            Token = "token123",
            NewPassword = "Test123!"
        };

        _authServiceMock.Setup(s => s.ResetPasswordAsync(dto.Email, dto.Token, dto.NewPassword))
                        .ReturnsAsync(true);

        var result = await _controller.ResetPassword(dto);

        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
        okResult.Value.Should().Be("Senha redefinida com sucesso.");
    }

    [Fact]
    public async Task ResetPasswordException()
    {
        var dto = new ResetPasswordUserDto
        {
            Email = "usertest@test.com",
            Token = "token123",
            NewPassword = "Test123!"
        };

        _authServiceMock.Setup(s => s.ResetPasswordAsync(dto.Email, dto.Token, dto.NewPassword))
                        .ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<Exception>(() => _controller.ResetPassword(dto));
        exception.Message.Should().Be("Falha ao redefinir a senha.");
    }

    [Fact]
    public async Task ResetPasswordGenericException()
    {
        var dto = new ResetPasswordUserDto
        {
            Email = "usertest@test.com",
            Token = "token123",
            NewPassword = "Test123!"
        };

        _authServiceMock.Setup(s => s.ResetPasswordAsync(dto.Email, dto.Token, dto.NewPassword))
                        .ThrowsAsync(new Exception("Erro interno"));

        await Assert.ThrowsAsync<Exception>(() => _controller.ResetPassword(dto));
    }
}
