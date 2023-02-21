using EasyLocalize.Contracts;
using EasyLocalize.Models;
using Newtonsoft.Json;

namespace EasyLocalize.Implementation;

public class Message : IMessage
{
    private readonly IEnumerable<RegisterMessage> _registerMessages;
    private readonly string _defaultLanguage;
    private string _language;
    private readonly Dictionary<string, Dictionary<string, string>> _messages;
    public string DefaultHeaderName { get; }
    private readonly HttpClient _client;

    public string DefaultLanguage => _language;

    public Message(EasyLocalizeOptions options)
    {
        _registerMessages = options.RegisterMessages;
        _language = options.DefaultLanguage;
        _defaultLanguage = options.DefaultLanguage;
        _messages = new Dictionary<string, Dictionary<string, string>>();
        DefaultHeaderName = options.DefaultHeaderName;
        _client = new HttpClient();
        FillData().Wait();
    }

    private async Task FillData()
    {
        foreach (var registerMessage in _registerMessages)
        {
            switch (registerMessage)
            {
                case null:
                    throw new Exception("EasyLocalize: register Messages Is Not Valid");
                case { IsExternal: true, JsonFilePath: { } } when
                    registerMessage.JsonFilePath.StartsWith("http"):
                    await HttpRequest(registerMessage);
                    continue;
                case { JsonFilePath: { } } when !registerMessage.JsonFilePath.EndsWith(".json"):
                    throw new Exception($"EasyLocalize - {registerMessage.LocalizeKey}: json file path is invalid");
                default:
                    ReadFile(registerMessage);
                    continue;
            }
        }
    }

    private async Task HttpRequest(RegisterMessage registerMessage)
    {
        // Call asynchronous network methods in a try/catch block to handle exceptions.
        try
        {
            using var response = await _client.GetAsync(registerMessage.JsonFilePath);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);

            if (registerMessage.LocalizeKey != null && data != null)
                _messages.Add(registerMessage.LocalizeKey.ToLower(), data);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            throw new Exception($"EasyLocalize - {registerMessage.LocalizeKey}: " + "json file is invalid");
        }
    }

    private void ReadFile(RegisterMessage registerMessage)
    {
        var path = registerMessage.IsExternal
            ? registerMessage.JsonFilePath
            : AppDomain.CurrentDomain.BaseDirectory + registerMessage.JsonFilePath;

        var readFile = File.ReadAllText(path ?? string.Empty);

        if (readFile.Length == 0)
            throw new Exception($"EasyLocalize - {registerMessage.LocalizeKey}: " + "json file is invalid");

        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(readFile);

        if (registerMessage.LocalizeKey != null && data != null)
            _messages.Add(registerMessage.LocalizeKey.ToLower(), data);
    }

    private Dictionary<string, string> GetMessagesByDefault
    {
        get
        {
            _messages.TryGetValue(_language.ToLower(), out var values);
            return values ?? new Dictionary<string, string>();
        }
    }

    public EasyLocalizeResponse Get(string key, params object[] parameters)
    {
        var defaultMessage = GetMessagesByDefault;
        defaultMessage.TryGetValue(key, out var value);

        value = FillParameters(parameters, value);

        return new EasyLocalizeResponse { Value = value ?? key, IsValid = value != null };
    }

    private static string? FillParameters(object[] parameters, string? value)
    {
        for (var i = 0; i < parameters.Length; i++)
        {
            value = value?.Replace("{" + i + "}", parameters[i].ToString());
        }

        return value;
    }

    public void SetLanguage(string? acceptedLanguage)
    {
        if (acceptedLanguage != null &&
           _registerMessages.Any(registerMessage => registerMessage.LocalizeKey?.ToLower() == acceptedLanguage.ToLower()))
            _language = acceptedLanguage;
        else _language = _defaultLanguage;
    }
}