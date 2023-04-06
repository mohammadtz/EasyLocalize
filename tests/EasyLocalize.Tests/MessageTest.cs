using EasyLocalize.Implementation;
using EasyLocalize.Models;
using FluentAssertions;

namespace EasyLocalize.Tests;

public class MessageTest
{
    private const string FaValue = "عملیات با موفقیت انجام شد";
    private const string EnValue = "Operation done successfully";
    private readonly Message _message;

    public MessageTest()
    {
        var localizeOptions = new EasyLocalizeOptions
        {
            RegisterMessages = new List<RegisterMessage>
            {
                new()
                {
                    LocalizeKey = "en-US",
                    JsonFilePath = "\\Localizations\\en.json",
                    IsExternal = false
                },
                new()
                {
                    LocalizeKey = "fa-IR",
                    JsonFilePath = "\\Localizations\\fa.json",
                    IsExternal = false
                },
                new()
                {
                    LocalizeKey = "en-UK",
                    JsonFilePath = "https://jsonplaceholder.typicode.com/todos/1",
                    IsExternal = true
                }
            },
            DefaultLanguage = "fa-IR",
        };

        _message = new Message(localizeOptions);
    }

    [Fact]
    public void Get_WhenCalled_ReturnValueSuccess()
    {
        var key = "Success";
        var faResult = _message.Get(key);

        faResult.IsValid.Should().BeTrue();
        faResult.Value.Should().NotBe(key);
        faResult.Value.Should().Be(FaValue);

        _message.SetLanguage("en-Us");
        var enResult = _message.Get(key);

        enResult.IsValid.Should().BeTrue();
        enResult.Value.Should().NotBe(key);
        enResult.Value.Should().Be(EnValue);

        _message.SetLanguage("en-UK");
        var ukResult = _message.Get("title");

        ukResult.IsValid.Should().BeTrue();
        ukResult.Value.Should().NotBe(key);
        ukResult.Value.Should().Be("delectus aut autem");
    }

    [Fact]
    public void Get_WhenPassParameters_ShouldBeExpected()
    {
        var key = "invalid";
        var firstName = _message.Get("first_name");
        var faResult = _message.Get(key, firstName.Value);

        faResult.IsValid.Should().BeTrue();
        faResult.Value.Should().NotBe(key);
        faResult.Value.Should().Contain(firstName.Value);

        _message.SetLanguage("en-Us");
        var enFirstName = _message.Get("first_name");
        var enResult = _message.Get(key, enFirstName.Value);

        enResult.IsValid.Should().BeTrue();
        enResult.Value.Should().NotBe(key);
        enResult.Value.Should().Contain(enFirstName.Value);
    }

    [Fact]
    public void Get_WhenJsonHaveInner_ShouldBeExpected()
    {
        var key = "fields:mobile_number";
        var faResult = _message.Get(key);
        faResult.Value.Should().Be("شماره موبایل");
    }
}