using System;
using System.Linq;
using System.Text.Json;
using Canteen.Api.Controllers;
using Canteen.Api.Tests.Fixtures;
using Canteen.Dto;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Canteen.Api.Tests;

public class LoginControllerTest : IClassFixture<ContextFixture>, IClassFixture<ConfigurationFixture>
{
    private readonly LoginController _controller;
    private readonly ContextFixture _contextFixture;

    public LoginControllerTest(ConfigurationFixture configurationFixture, ContextFixture contextFixture)
    {
        _controller = new LoginController(configurationFixture.Configuration, contextFixture.Context);
        _contextFixture = contextFixture;
    }

    [Fact]
    public async void Login_Authorized_ReturnOk()
    {
        var employee = _contextFixture.Context.Employees.First();
        var response = await _controller.LoginAsync(new PasswordDto {Password = employee.Password});

        Assert.Equal(typeof(OkObjectResult), response.GetType());
    }

    [Fact]
    public async void Login_Unauthorized_ReturnUnauthorized()
    {
        var response =
            await _controller.LoginAsync(
                new PasswordDto {Password = Guid.NewGuid().ToString()});

        Assert.Equal(typeof(UnauthorizedResult), response.GetType());
    }

    [Fact]
    public async void Login_Null_ReturnBadRequest()
    {
        var response = await _controller.LoginAsync(new PasswordDto {Password = null});

        Assert.Equal(typeof(BadRequestResult), response.GetType());
    }
}