// See https://aka.ms/new-console-template for more information

using EasyLocalize.Implementation;
using EasyLocalize.Models;

var localizeOptions = new EasyLocalizeOptions
{
    RegisterMessages = new List<RegisterMessage>()
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
    // DefaultLanguage = "en-US",
    DefaultLanguage = "fa-IR",
};

var easyLocalize = new Message(localizeOptions);

var successMessage = easyLocalize.Get("Success");

Console.WriteLine(successMessage.Value);

easyLocalize.SetLanguage("en-US");

var newSuccessMessage = easyLocalize.Get("Success");

Console.WriteLine(newSuccessMessage.Value);
