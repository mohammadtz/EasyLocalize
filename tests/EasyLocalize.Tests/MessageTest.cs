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
    }
}